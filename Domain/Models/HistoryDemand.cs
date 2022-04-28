using System;

namespace Domain.Models
{
    public class HistoryDemand
    {
        private DateTime _StartDate;
        private DateTime _EndDate;

        public long IdAccount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
