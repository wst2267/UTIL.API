namespace util.API.Models.Request
{
    public class InsertLedgerRequest
    {
        public string LedgerDate { get; set; } = null!;
        public string LedgerType { get; set; } = null!;
        public string LedgerNote { get; set; } = null!;
        public int LedgerValue { get; set; }
    }
}
