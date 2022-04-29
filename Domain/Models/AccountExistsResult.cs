namespace Domain.Models
{
    public class AccountExistsResult
    {
        public enum AccountStatus
        {
            Ok,
            Invalid
        }

        public AccountStatus Result { get; set; }
        public string Message { get; set; }
    }
}
