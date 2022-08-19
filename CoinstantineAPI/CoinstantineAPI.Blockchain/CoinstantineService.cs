using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;
using CoinstantineAPI.Blockchain.DTO.Coinstantine;
using CoinstantineAPI.Blockchain.Web3;
using CoinstantineAPI.Core.Blockchain;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nethereum.Hex.HexConvertors.Extensions;

namespace CoinstantineAPI.Blockchain
{
    public class CoinstantineService : ICoinstantineService
    {
        private readonly ICoinstantineSmartContractExecutor _client;
        private readonly IContextProvider _contextProvider;
        private readonly ISmartContractProvider _smartContractProvider;
        private readonly IWeb3AccountService _web3AccountService;
        private readonly ILogger _logger;

        public CoinstantineService(ICoinstantineSmartContractExecutor client,
                                   IContextProvider contextProvider,
                                   ISmartContractProvider smartContractProvider,
                                   IWeb3AccountService web3AccountService,
                                   ILoggerFactory loggerFactory)
        {
            _client = client;
            _contextProvider = contextProvider;
            _smartContractProvider = smartContractProvider;
            _web3AccountService = web3AccountService;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task<string> CreateAirdrop(Airdrop airdrop)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    if (!await context.Tokens.AnyAsync(x => x.Address == airdrop.Token.Address))
                    {
                        await context.Tokens.AddAsync(airdrop.Token);
                    }
                    var param = new Parameters
                    {
                        Sender = airdrop.Creator
                    };

                    var (Event, Receipt) = await _client.PostWithResult<CreateAirdropEvent>("createAirdrop", param, airdrop.Token.Address, airdrop.Creator.Address, airdrop.Amount, airdrop.NumberOfUsers + 1);
                    var airdropIdentifier = Event.Id;
                    airdrop.AirdropId = airdropIdentifier.ToHex(true);
                    airdrop.AirdropIdBytes = airdropIdentifier;
                    await context.Airdrops.AddAsync(airdrop);
                    await context.SaveChangesAsync();

                    return airdrop.AirdropId;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in CreateAirdrop()", airdrop);
                throw ex;
            }
        }

        public async Task<bool> Deposit(string airdropId, int amount)
        {
            try
            {
                var airdrop = await GetAirdrop(airdropId).ConfigureAwait(false);
                var mdt = await _smartContractProvider.GetCoinstantine(airdrop.Creator).ConfigureAwait(false);
                var smartContract = await _smartContractProvider.GetSmartContract(airdrop.Token).ConfigureAwait(false);
                var amountToDeposit = amount;//new BigDecimal(new BigInteger(amount), airdrop.Token.Decimals);
                await _client.PostAndForgetOnSpecificToken(airdrop.Token, "approve", airdrop.Creator, mdt.Address, amountToDeposit).ConfigureAwait(false);
                var (Event, Receipt) = await _client.PostWithResult<DepositEvent>("deposit", airdrop.Creator, airdrop.AirdropIdBytes, amountToDeposit).ConfigureAwait(false);
                var airdropIdentifier = Event.Id.ToHex(true);
                return airdropIdentifier == airdropId;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in Deposit()", airdropId, amount);
                throw ex;
            }
        }

        private async Task<Airdrop> GetAirdrop(string airdropId, bool withSubscribers = false)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    var prefix = "0x";
                    if (!airdropId.StartsWith(prefix, StringComparison.CurrentCulture))
                    {
                        airdropId = $"{prefix}{airdropId}";
                    }

                    var query = context.Airdrops.Include(a => a.Creator)
                                       .Include(a => a.Token);
                    if (withSubscribers)
                    {
                        query.Include(a => a.Subscribers)
                                .ThenInclude(s => s.User);
                    }
                    return await query.FirstOrDefaultAsync(x => x.AirdropId == airdropId);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetAirdrop()", airdropId, withSubscribers);
                throw ex;
            }
        }

        public async Task<string> CheckDeposit(string airdropId)
        {
            try
            {
                var airdrop = await GetAirdrop(airdropId).ConfigureAwait(false);
                var balance = await _client.Get<BigInteger>("_tokens", airdrop.AirdropIdBytes);
                return balance.ToString();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in CheckDeposit()", airdropId);
                throw ex;
            }
        }

        public async Task<bool> Subscribe(string userId, string airdropId)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    var user = await context.BlockchainUsers.FirstAsync(x => x.Username == userId);
                    var airdrop = await GetAirdrop(airdropId).ConfigureAwait(false);
                    if (airdrop.Subscribers?.Any(x => x.User == user) ?? false)
                    {
                        return false;
                    }

                    var subscribeResult = await _client.PostWithResult<SubscribeEvent>("subscribe", user, airdrop.Token.Address, user.Address, user.Username, airdrop.AirdropIdBytes);

                    if (subscribeResult.Event?.AirdropId?.ToHex(true) == airdrop.AirdropId)
                    {
                        var subscriber = new Subscriber
                        {
                            User = user,
                            IdentifierBytes = subscribeResult.Event.UserId,
                            Identifier = subscribeResult.Event.UserId.ToHex(true)
                        };
                        if (airdrop.Subscribers == null)
                        {
                            airdrop.Subscribers = new List<Subscriber>();
                        }
                        airdrop.Subscribers.Add(subscriber);

                        await context.Airdrops.AddAsync(airdrop);
                        await context.SaveChangesAsync();

                        return true;
                    }
                    return false;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in Subscribe()", userId, airdropId);
                throw ex;
            }
        }

        private async Task<string> ReadFile(string file)
        {
            try
            {
                var assembly = typeof(CoinstantineService).GetTypeInfo().Assembly;
                var stream = assembly.GetManifestResourceStream($"CoinstantineAPI.Blockchain.ContractInfo.{file}");
                string text;
                using (var reader = new StreamReader(stream))
                {
                    text = await reader.ReadToEndAsync().ConfigureAwait(false);
                }
                return text;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in ReadFile()", file);
                throw ex;
            }
        }

        public async Task<IEnumerable<Subscriber>> Subscribers(string airdropId)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    var airdrop = await GetAirdrop(airdropId, true).ConfigureAwait(false);
                    var ids = airdrop.Subscribers.Select(x => x.Id).ToList();
                    return await context.Subscribers
                                            .Include(s => s.User)
                                            .Where(x => ids.Contains(x.Id))
                                            .ToListAsync();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in Subscribers()", airdropId);
                throw ex;
            }
        }

        public async Task<int> Validate(string airdropId, IEnumerable<Subscriber> validatedUsers, int amount)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    var airdrop = await GetAirdrop(airdropId, true).ConfigureAwait(false);
                    var subscribers = await Subscribers(airdropId).ConfigureAwait(false);
                    var existingUsers = subscribers.Intersect(validatedUsers);
                    var owner = await _web3AccountService.GetOwnerAndCreateIfNeeded();
                    var nbValidatedUsers = 0;
                    foreach (var user in existingUsers)
                    {
                        var (Event, Receipt) = await _client.PostWithResult<ValidateEvent>("validate", owner,
                                                                                   user.IdentifierBytes, airdrop.AirdropIdBytes, amount).ConfigureAwait(false);
                        if (Event == null)
                        {
                            continue;
                        }
                        var subscriber = airdrop.Subscribers.FirstOrDefault(x => x.Identifier == Event.UserId.ToHex(true));
                        subscriber.Validated = true;
                        await context.Subscribers.AddAsync(subscriber);
                        nbValidatedUsers++;
                    }
                    await context.SaveChangesAsync();
                    return nbValidatedUsers;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in Validate()", airdropId, validatedUsers, amount);
                throw ex;
            }
        }

        public async Task<bool> Withdraw(string airdropId, string userId)
        {
            try
            {
                var airdrop = await GetAirdrop(airdropId, true).ConfigureAwait(false);
                var subscribers = await Subscribers(airdropId).ConfigureAwait(false);
                var subscriber = subscribers.FirstOrDefault(x => x.User.Username == userId);
                if (subscriber == null)
                {
                    return false;
                }
                var completeUser = subscriber.User;

                var fees = await _client.Get<BigInteger>("_fees").ConfigureAwait(false);
                var (Event, Receipt) = await _client.PostWithResult<WithdrawEvent>("withdraw", new Parameters { Sender = completeUser, Value = fees },
                                                        airdrop.AirdropIdBytes, subscriber.IdentifierBytes).ConfigureAwait(false);
                if (Event == null)
                {
                    return false;
                }
                return Event.UserId.ToHex(true) == subscriber.Identifier;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in Withdraw()", airdropId, userId);
                throw ex;
            }
        }

        public async Task<bool> CloseAirdrop(string airdropId)
        {
            try
            {
                var airdrop = await GetAirdrop(airdropId, true).ConfigureAwait(false);
                var (Event, Receipt) = await _client.PostWithResult<CloseAirdropEvent>("closeAirdrop", airdrop.Creator, airdrop.AirdropIdBytes).ConfigureAwait(false);
                if (Event == null)
                {
                    return false;
                }
                return Event.AirdropId.ToHex(true) == airdropId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CloseAirdrop()", airdropId);
                throw ex;
            }
        }

        public async Task<bool> StartDistribution(string airdropId)
        {
            try
            {
                var owner = await _web3AccountService.GetOwnerAndCreateIfNeeded();
                var airdrop = await GetAirdrop(airdropId, true).ConfigureAwait(false);
                await _client.PostAndForget("startDistribution", owner, airdrop.AirdropIdBytes).ConfigureAwait(false);
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in StartDistribution()", airdropId);
                return false;
            }
        }
    }
}
