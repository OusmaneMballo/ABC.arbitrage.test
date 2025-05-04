using AbcArbitrage.Homework.Models;
using AbcArbitrage.Homework.Repositories;
using System;

namespace AbcArbitrage.Homework.Services
{
    public static class RoutableMessages
    {
        public class InstrumentAdded : IRoutableMessage
        {
            public int ExchangeId { get; set; }
            public int InstrumentId { get; set; }
            public string? Symbol { get; set; }
            public string? InstrumentType { get; set; }

            public MessageRoutingContent GetContent() => new(ExchangeId.ToString());
        }

        public class InstrumentDelisted : IRoutableMessage
        {
            public int ExchangeId { get; set; }
            public int InstrumentId { get; set; }

            public MessageRoutingContent GetContent() => new(ExchangeId.ToString());
        }

        public class PriceUpdated : IRoutableMessage
        {
            public string? ExchangeCode { get; set; }
            public string? Symbol { get; set; }
            public double Value { get; set; }

            public MessageRoutingContent GetContent()
            {
                if (string.IsNullOrEmpty(ExchangeCode) || string.IsNullOrEmpty(Symbol)) return MessageRoutingContent.Empty;

                return new(ExchangeCode, Symbol);
            }
        }

        public class TradingHalted : IRoutableMessage
        {
            public string? ExchangeCode { get; set; }
            public string? Symbol { get; set; }
            public DateTime TimestampUtc { get; set; }

            public MessageRoutingContent GetContent()
            {
                if (string.IsNullOrEmpty(ExchangeCode) || string.IsNullOrEmpty(Symbol)) return MessageRoutingContent.Empty;

                return new(ExchangeCode, string.Empty, Symbol);
            }
        }

        public class InstrumentConnected : IRoutableMessage
        {
            public string? ExchangeCode { get; set; }
            public int ProviderId { get; set; }
            public string? Sector { get; set; }
            public char SymbolRangeStart { get; set; }
            public string? Symbol { get; set; }
            public DateTime TimestampUtc { get; set; }

            public MessageRoutingContent GetContent()
            {
                if (string.IsNullOrEmpty(ExchangeCode) || string.IsNullOrEmpty(Sector) || string.IsNullOrEmpty(Symbol)) return MessageRoutingContent.Empty;

                return new(ExchangeCode, ProviderId.ToString(), Sector, SymbolRangeStart.ToString(), Symbol);
            }
        }
    }
}
