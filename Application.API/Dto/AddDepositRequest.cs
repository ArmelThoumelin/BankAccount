namespace Application.API.Dto
{
    public class AddDepositRequest
    {
        public long IdAccount { get; set; }
        public decimal Amount { get; set; }
    }
}
