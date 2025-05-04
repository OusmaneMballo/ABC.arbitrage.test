using AbcArbitrage.Homework.Models;
using AbcArbitrage.Homework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AbcArbitrage.Homework.Routing
{
    public class MessageRouterPerformanceTest
    {
        private readonly SubscriptionIndexPerformance _subscriptionIndexPerformance;
        private readonly MessageRouterPerformance _routerPerformance;

        public MessageRouterPerformanceTest()
        {
            _subscriptionIndexPerformance = new SubscriptionIndexPerformance();
            _routerPerformance = new MessageRouterPerformance(_subscriptionIndexPerformance);
        }

        [Fact]
        public void ShouldIncludeSingleMatchingSubscription()
        {
            // Arrange
            var clientId = new ClientId("Client.1");
            _subscriptionIndexPerformance.AddSubscriptions(new[]
            {
                Subscription.Of<SimpleMessages.ExchangeAdded>(clientId),
            });

            // Act
            var clientIds = _routerPerformance.GetConsumersImproved(new SimpleMessages.ExchangeAdded()).ToList();

            // Assert
            Assert.Equal(new[] { clientId }, clientIds);
        }

        [Fact]
        public void ShouldIncludeMatchingClientForTwoMessages()
        {
            // Arrange
            var clientId = new ClientId("Client.1");
            _subscriptionIndexPerformance.AddSubscriptions(new[]
            {
                Subscription.Of<SimpleMessages.ExchangeAdded>(clientId),
                Subscription.Of<SimpleMessages.ExchangeTradingPhaseChanged>(clientId),
            });

            // Act
            var clientIdsForMessage1 = _routerPerformance.GetConsumersImproved(new SimpleMessages.ExchangeAdded()).ToList();
            var clientIdsForMessage2 = _routerPerformance.GetConsumersImproved(new SimpleMessages.ExchangeTradingPhaseChanged()).ToList();

            // Assert
            Assert.Equal(new[] { clientId }, clientIdsForMessage1);
            Assert.Equal(new[] { clientId }, clientIdsForMessage2);
        }

        [Fact]
        public void ShouldExcludeSubscriptionWithOtherMessageType()
        {
            // Arrange
            var clientId1 = new ClientId("Client.1");
            var clientId2 = new ClientId("Client.2");
            _subscriptionIndexPerformance.AddSubscriptions(new[]
            {
                Subscription.Of<SimpleMessages.ExchangeAdded>(clientId1),
                Subscription.Of<SimpleMessages.ExchangeTradingPhaseChanged>(clientId2),
            });

            // Act
            var clientIds = _routerPerformance.GetConsumersImproved(new SimpleMessages.ExchangeAdded()).ToList();

            // Assert
            Assert.Equal(new[] { clientId1 }, clientIds);
        }

        [Fact]
        public void ShouldIncludeRoutableSubscriptionsForTwoClients()
        {
            // Arrange
            var clientId1 = new ClientId("Client.1");
            var clientId2 = new ClientId("Client.2");
            var clientId3 = new ClientId("Client.3");

            _subscriptionIndexPerformance.AddSubscriptions(new[]
            {
                Subscription.Of<RoutableMessages.PriceUpdated>(clientId1, new ContentPattern("NASDAQ", "*")),
                Subscription.Of<RoutableMessages.PriceUpdated>(clientId2, new ContentPattern("NYSE", "*")),
                Subscription.Of<RoutableMessages.PriceUpdated>(clientId3, new ContentPattern("NASDAQ", "*")),
            });

            var routableMessage = new RoutableMessages.PriceUpdated { ExchangeCode = "NASDAQ", Symbol = "MSFT" };

            // Act
            var clientIds = _routerPerformance.GetConsumersImproved(routableMessage).ToList();

            // Assert
            Assert.Equal(2, clientIds.Count);
        }
        [Fact]
        public void ShouldExcludeRoutableSubscriptionWithOtherMessageType()
        {
            // Arrange
            var clientId = new ClientId("Client.1");
            _subscriptionIndexPerformance.AddSubscriptions(new[]
            {
                Subscription.Of<RoutableMessages.InstrumentAdded>(clientId, new ContentPattern("9")),
            });

            var routableMessage = new RoutableMessages.InstrumentDelisted { ExchangeId = 9 };

            // Act
            var clientIds = _routerPerformance.GetConsumersImproved(routableMessage).ToList();

            // Assert
            Assert.Empty(clientIds);
        }

        [Theory]
        [InlineData("NASDAQ")]
        [InlineData("NASDAQ.MSFT")]
        [InlineData("*")]
        [InlineData("*.MSFT")]
        [InlineData("*.*")]
        public void ShouldIncludeMatchingRoutableSubscriptionWithPattern(string contentPattern)
        {
            // Arrange
            var clientId = new ClientId("Client.1");
            _subscriptionIndexPerformance.AddSubscriptions(new[]
            {
                Subscription.Of<RoutableMessages.PriceUpdated>(clientId, ContentPattern.Split(contentPattern)),
            });

            var routableMessage = new RoutableMessages.PriceUpdated { ExchangeCode = "NASDAQ", Symbol = "MSFT" };

            // Act
            var clientIds = _routerPerformance.GetConsumersImproved(routableMessage).ToList();

            // Assert
            Assert.Equal(new[] { clientId }, clientIds);
        }
    }
}
