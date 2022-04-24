namespace Domain.Models
{
    public class TransactionResult
    {
        public enum TransactionStatus
        {
            Ok,
            Invalid,
            Unauthorized
        }

        public TransactionStatus Result { get; set; }
        public string Message { get; set; }
    }
}
