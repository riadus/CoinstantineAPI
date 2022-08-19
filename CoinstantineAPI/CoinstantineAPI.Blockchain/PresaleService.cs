using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using CoinstantineAPI.Blockchain.DTO.Presale;
using CoinstantineAPI.Blockchain.Web3;
using CoinstantineAPI.Core.Blockchain;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Extensions;
using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Data;
using CoinstantineAPI.Scheduler;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3;

namespace CoinstantineAPI.Blockchain
{
    public class PresaleService : IPresaleService
    {
        private readonly ISmartContractExecutor _smartContractExecutor;
        private readonly INotificationCenter _notificationCenter;
        private readonly IWeb3AccountService _web3AccountService;
        private readonly IScheduler _scheduler;
        private readonly IEthereumService _ethereumService;
        private readonly IContextProvider _contextProvider;
        private readonly ILogger _logger;

        public PresaleService(ISmartContractExecutor smartContractExecutor,
                              INotificationCenter notificationCenter,
                              IWeb3AccountService web3AccountService,
                              IScheduler scheduler,
                              IEthereumService ethereumService,
                              IContextProvider contextProvider,
                              ILoggerFactory loggerFactory)
        {
            _smartContractExecutor = smartContractExecutor;
            _notificationCenter = notificationCenter;
            _web3AccountService = web3AccountService;
            _scheduler = scheduler;
            _ethereumService = ethereumService;
            _contextProvider = contextProvider;
            _smartContractExecutor.SetSmartContractType(SmartContractType.Presale);
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task<BuyTokensResult> Buy(BlockchainUser buyer, decimal amount, string email)
        {
            try
            {
                var hasParticipated = await HasParticipated(buyer.Address);
                var gas = hasParticipated ? 86000 : 280000;
                var maxBalance = await _ethereumService.GetMaximumUsableEtherFor(buyer, gas);
                var weiToUse = Math.Min(maxBalance, amount);
                var amountInWei = UnitConversion.Convert.ToWei(weiToUse);

                var transactionHash = await _smartContractExecutor.PostAndDontWaitForTransactionReceipt("buyTokens", new Parameters { Sender = buyer, Value = amountInWei });

                var receiptData = new TransactionReceiptData
                {
                    TransactionHash = transactionHash,
                };

                var buyTokenResult = new BuyTokensResult
                {
                    AmountBought = 0,
                    Value = 0,
                    TransactionReceipt = receiptData,
                    PurchaseDate = DateTime.UtcNow,
                    SaleType = SaleType.Presale,
                    BuyerId = buyer.Id
                };

                using (var context = _contextProvider.CoinstantineContext)
                {
                    await context.BuyTokensResults.AddAsync(buyTokenResult);
                    await context.SaveChangesAsync();
                }

                await CheckReceiptAndNotifyUser(transactionHash, buyer, "buyTokens", new Parameters { Sender = buyer, Value = amountInWei }, email, buyTokenResult.Id);

                return buyTokenResult;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in Buy()", buyer, amount);
                throw ex;
            }
        }

        private async Task CheckReceiptAndNotifyUser(string transactionHash, BlockchainUser user, string functionName, Parameters parameters, string email, int buyTokenResultId)
        {
            var scheduledTask = new ScheduledTask
            {
                Task = () => CheckReceipt(transactionHash),
                SuccessTask = () => BuyingDone(user, transactionHash, functionName, parameters, email, buyTokenResultId),
                FailedTask = () => Task.FromResult(0),
                TimeoutTask = () => Task.FromResult(0),
                Timeout = 3600
            };

            await _scheduler.ScheduleTask(scheduledTask);
        }

        private async Task<bool?> CheckReceipt(string transactionHash)
        {
            var transactionReceipt = await _smartContractExecutor.GetReceiptFromHash(transactionHash);
            if(transactionReceipt == null)
            {
                return null;
            }
            var succeeded = transactionReceipt.Status.Value == BigInteger.One;
            return succeeded;
        }

        private async Task BuyingDone(BlockchainUser buyer, string transactionHash, string functionName, Parameters parameters, string email, int buyTokenResultId)
        {
            TransactionReceiptData receiptData = null;
            var transactionReceipt = await _smartContractExecutor.GetReceiptFromHash(transactionHash);
            var (eventDTO, transactionReceiptDTO) = await _smartContractExecutor.GetResultOfPreviousFunction<BuyTokensEventDTO>(transactionReceipt, functionName, parameters);
            if (eventDTO != null)
            {
                receiptData = new TransactionReceiptData
                {
                    BlockHash = transactionReceiptDTO.BlockHash,
                    BlockNumber = transactionReceiptDTO.BlockNumber?.Value.ToString(),
                    ContractAddress = transactionReceiptDTO.ContractAddress,
                    CumulativeGasUsed = transactionReceiptDTO.CumulativeGasUsed?.Value.ToString(),
                    GasUsed = transactionReceiptDTO.GasUsed?.Value.ToString(),
                    Status = transactionReceiptDTO.Status?.Value.ToString(),
                    TransactionHash = transactionReceiptDTO.TransactionHash,
                    TransactionIndex = transactionReceiptDTO.TransactionIndex?.ToString()
                };
            }

            using(var context = _contextProvider.CoinstantineContext)
            {
                var buyTokensResult = await context.BuyTokensResults.Include(x => x.TransactionReceipt)
                                                              .FirstAsync(x => x.Id == buyTokenResultId);

                context.TransactionReceipts.Remove(buyTokensResult.TransactionReceipt);

                buyTokensResult.AmountBought = (decimal)eventDTO?.Amount;
                buyTokensResult.Value = UnitConversion.Convert.FromWei(eventDTO?.Value ?? 0);
                buyTokensResult.TransactionReceipt = receiptData;
                buyTokensResult.PurchaseDate = DateTime.UtcNow;
                buyTokensResult.SaleType = SaleType.Presale;
                buyTokensResult.BuyerId = buyer.Id;

                context.BuyTokensResults.Update(buyTokensResult);
                await context.SaveChangesAsync();
            }

            await _notificationCenter.SendSilentNotification(email, "Notification_BuyingDone", PartToUpdate.Wallet);
        }

        public async Task<IEnumerable<BuyTokensResult>> GetPurchases(BlockchainUser user)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    var purchases = await context.BuyTokensResults
                                                    .Include(b => b.TransactionReceipt)
                                                    .Where(x => x.BuyerId == user.Id)
                                                    .ToListAsync();
                    return purchases;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetPurchases()", user);
                throw ex;
            }
        }

        public async Task<TransactionReceipt> ClaimBack(BlockchainUser buyer)
        {
            try
            {
                var (Event, Receipt) = await _smartContractExecutor.PostWithResult<ClaimBackEventDTO>("claimBack", buyer);
                return Receipt;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in ClaimBack()", buyer);
                throw ex;
            }
        }

        private async Task<bool> HasParticipated(string address)
        {
            try
            {
                var containsParticipant = await _smartContractExecutor.Get<bool>("Contains", address);
                return containsParticipant;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in HasParticipated()");
                throw ex;
            }
        }

        public async Task<decimal> GetAmountRaised()
        {
            try
            {
                var amountRaised = await _smartContractExecutor.Get<BigInteger>("weiRaised");
                return UnitConversion.Convert.FromWei(amountRaised);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetAmountRaised()");
                throw ex;
            }
        }

        public async Task<IEnumerable<BuyersDTO>> GetBuyers()
        {
            try
            {
                var owner = await _web3AccountService.GetOwnerAndCreateIfNeeded();
                var maxIndex = await _smartContractExecutor.Get<int>("GetMaxIndex");
                var result = new List<BuyersDTO>();
                for (var i = 1; i <= maxIndex; i++)
                {
                    var participant = await _smartContractExecutor.GetForTuple<ParticipantDTO>("GetParticipant", i);
                    var buyerDTO = new BuyersDTO
                    {
                        Address = participant.Address,
                        AmountInvested = UnitConversion.Convert.FromWei(participant.Participation),
                        TokensToReceive = participant.Tokens,
                        LastPurchageDate = participant.Timestamp.ToDateTime()
                    };
                    result.Add(buyerDTO);
                }
                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetBuyers()");
                throw ex;
            }
        }

        public Task<int> GetCurrentRate()
        {
            try
            {
                return _smartContractExecutor.Get<int>("getRate");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetCurrentRate()");
                throw ex;
            }
        }

        public Task<PresaleStatus> GetStatus()
        {
            throw new System.NotImplementedException();
        }

        public Task<TransactionReceipt> GetReceipt(string transactionHash)
        {
            return _smartContractExecutor.GetReceiptFromHash(transactionHash);
        }
    }
}
