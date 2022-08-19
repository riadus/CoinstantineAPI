using System;
using System.Threading.Tasks;
using CoinstantineAPI.Core.External;
using CoinstantineAPI.Scan.Interfaces;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.Scan
{
    public class PriceService : IPriceService
    {
        private readonly ICryptoCompare _cryptoCompare;
        private readonly ILogger _logger;

        public PriceService(ICryptoCompare cryptoCompare,
                            ILoggerFactory loggerFactory)
        {
            _cryptoCompare = cryptoCompare;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task<Price> GetEtherPrice()
        {
            try
            {
                var etherPrice = await _cryptoCompare.GetEtherPrice();
                return new Price
                {
                    Usd = etherPrice.Usd,
                    Eur = etherPrice.Eur
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetEtherPrice()");
                throw ex;
            }
        }
    }
}
