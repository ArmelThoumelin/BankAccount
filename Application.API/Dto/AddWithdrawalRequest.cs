namespace Application.API.Dto
{
    public class AddWithdrawalRequest
    {
        public long IdAccount { get; set; }
        public decimal Amount { get; set; }
    }
}
