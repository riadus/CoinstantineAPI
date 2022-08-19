namespace CoinstantineAPI.WebApi.Responses
{
    public class TransactionReceiptDataResponse
    {
        public string TransactionHash { get; set; }
        public string TransactionIndex { get; set; }
        public string BlockHash { get; set; }
        public string BlockNumber { get; set; }
        public string CumulativeGasUsed { get; set; }
        public string GasUsed { get; set; }
        public string ContractAddress { get; set; }
        public string Status { get; set; }
    }
}
