using System;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.WebApi.Responses
{
    public class BuyTokensResultResponse
    {
        public decimal AmountBought { get; set; }
        public decimal Value { get; set; }
        public TransactionReceiptDataResponse TransactionReceipt { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int BuyerId { get; set; }
        public SaleType SaleType { get; set; }
    }
}
