using System.Collections.Generic;

namespace Domain.Models
{
    public class HistoryResult
    {
        public enum HistoryStatus
        {
            Ok,
            Invalid,
            InvalidDateRange,
            UnknownAccount

        }

        public HistoryStatus Result { get; set; }
        public List<Transaction> Transactions { get; set; }
        public string Message { get; set; }
    }
}
