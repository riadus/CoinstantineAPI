using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoinstantineAPI.Data
{
    public class BuyTokensResult : Entity
    {
        public decimal AmountBought { get; set; }
        public decimal Value { get; set; }
        [ForeignKey("TransactionReceiptId")]
        public TransactionReceiptData TransactionReceipt { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int BuyerId { get; set; }
        public SaleType SaleType { get; set; }
    }
}
