using System;

namespace AbcArbitrage.Homework.Models
{
    internal static class SimpleMessages
    {
        public class ExchangeAdded : IMessage
        {
            public int ExchangeId { get; set; }
            public string? Code { get; set; }
            public string? Name { get; set; }
        }

        public class ExchangeTradingPhaseChanged : IMessage
        {
            public int ExchangeId { get; set; }
            public int TradingPhaseId { get; set; }
            public DateTime TimestampUtc { get; set; }
        }
    }
}
