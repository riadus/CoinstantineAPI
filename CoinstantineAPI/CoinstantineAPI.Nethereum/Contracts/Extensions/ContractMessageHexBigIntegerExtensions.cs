﻿using System.Numerics;
using Nethereum.Contracts.CQS;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using Nethereum.Web3;

namespace Nethereum.Contracts.Extensions
{
    public static class ContractMessageHexBigIntegerExtensions
    {
        public static HexBigInteger GetHexMaximumGas(this ContractMessageBase contractMessage)
        {
            return GetDefaultValue(contractMessage.Gas);
        }

        public static HexBigInteger GetHexValue(this ContractMessageBase contractMessage)
        {
            return GetDefaultValue(contractMessage.AmountToSend);
        }

        public static HexBigInteger GetHexGasPrice(this ContractMessageBase contractMessage)
        {
            return GetDefaultValue(contractMessage.GasPrice);
        }

        public static void SetGasPriceFromGwei(this ContractMessageBase contractMessage, decimal gweiAmount)
        {
            contractMessage.GasPrice = UnitConversion.Convert.ToWei(gweiAmount, UnitConversion.EthUnit.Gwei);
        }

        public static HexBigInteger GetHexNonce(this ContractMessageBase contractMessage)
        {
            return GetDefaultValue(contractMessage.Nonce);
        }

        public static HexBigInteger GetDefaultValue(BigInteger? bigInteger)
        {
            return bigInteger == null ? null : new HexBigInteger(bigInteger.Value);
        }

        public static string SetDefaultFromAddressIfNotSet(this ContractMessageBase contractMessage, string defaultFromAdddress)
        {
            if (string.IsNullOrEmpty(contractMessage.FromAddress))
            {
                contractMessage.FromAddress = defaultFromAdddress;
            }
            return contractMessage.FromAddress;
        }
    }
}