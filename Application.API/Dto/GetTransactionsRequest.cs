namespace Application.API.Dto
{
    public class GetTransactionsRequest
    {
        public long IdAccount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
