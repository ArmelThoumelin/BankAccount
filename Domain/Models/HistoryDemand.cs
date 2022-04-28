using System;

namespace Domain.Models
{
    public class HistoryDemand
    {
        public decimal IdAccount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
