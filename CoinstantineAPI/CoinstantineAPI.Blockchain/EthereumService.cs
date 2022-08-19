using System;
using System.Numerics;
using System.Threading.Tasks;
using CoinstantineAPI.Blockchain.Web3;
using CoinstantineAPI.Core.Blockchain;
using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Data;
using Microsoft.Extensions.Logging;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;

namespace CoinstantineAPI.Blockchain
{
    public class EthereumService : IEthereumService
    {
        private readonly INotificationCenter _notificationCenter;
        private readonly IWeb3Provider _web3Provider;
        private readonly ILogger _logger;
        private DateTime _updateDate;
        private decimal _gasPrice;

        public EthereumService(INotificationCenter notificationCenter,
                               IWeb3Provider web3Provider,
                               ILoggerFactory loggerFactory)
        {
            _notificationCenter = notificationCenter;
            _web3Provider = web3Provider;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task<decimal> GetMaximumUsableEtherFor(BlockchainUser user, decimal gas)
        {
            var web3 = await _web3Provider.GetWeb3ForUser(user);
            var currentBalance = await web3.Eth.GetBalance.SendRequestAsync(user.Address);
            var gasPrice = await GetGasPrice();
            var gweiPrice = gas * gasPrice;
            var price = UnitConversion.Convert.ToWei(gweiPrice, UnitConversion.EthUnit.Gwei);

            var hasEnough = currentBalance.Value > price;
            if(hasEnough)
            {
                var funds = currentBalance.Value - price;
                return UnitConversion.Convert.FromWei(funds);
            }
            return 0;
        }

        public async Task<string> SendFunds(BlockchainUser from, string to)
        {
            try
            {
                var web3 = await _web3Provider.GetWeb3ForUser(from);
                var currentBalance = await web3.Eth.GetBalance.SendRequestAsync(from.Address);
                var (Value, HasEnoughFunds) = await SubstractGas(currentBalance);
                if(!HasEnoughFunds)
                {
                    return string.Empty;
                }
                var gasNeeded = GasNeeded();
                var gasPrice = await GasPrice();
                var transactionInput = new TransactionInput(null, to, from.Address, gasNeeded, gasPrice, Value);
                return await web3.TransactionManager.SendTransactionAsync(transactionInput);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in SendFunds()", from.Address, to);
                throw ex;
            }
        }

        private HexBigInteger GasNeeded()
        {
            return new HexBigInteger(21000);
        }

        private async Task<HexBigInteger> GasPrice()
        {
            return new HexBigInteger(new BigInteger(await GetGasPrice()));
        }

        private async Task<(HexBigInteger Value, bool HasEnoughFunds)> SubstractGas(HexBigInteger value)
        {
            var gasPrice = await GasPrice(); 
            var price = GasNeeded().Value * gasPrice.Value;
            return value.Value > price ? (new HexBigInteger(value.Value - price), true) : (new HexBigInteger(0), false);
        }

        public async Task<decimal> GetGasPrice()
        {
            if((DateTime.Now - _updateDate).TotalMinutes > 10 || _gasPrice == 0)
            {
                var web3 = _web3Provider.Web3;
                var gas = await web3.Eth.GasPrice.SendRequestAsync();
                _updateDate = DateTime.Now;
                return _gasPrice = UnitConversion.Convert.FromWei(gas, UnitConversion.EthUnit.Gwei);
            }
            return _gasPrice;
        }
    }
}
