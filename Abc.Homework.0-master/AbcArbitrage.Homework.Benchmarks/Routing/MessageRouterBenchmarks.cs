using AbcArbitrage.Homework.Models;
using AbcArbitrage.Homework.Repositories;
using AbcArbitrage.Homework.Services;
using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace AbcArbitrage.Homework.Routing
{
    public class MessageRouterBenchmarks
    {
        private readonly MessageRouter _router;
        private readonly MessageRouterPerformance _routerPerformance;

        public MessageRouterBenchmarks()
        {
            var subscriptionIndex = BuildSubscriptionIndex();
            var subscriptionIndexPerformance = BuildSubscriptionIndexPerformaance();

            _router = new MessageRouter(subscriptionIndex);
            _routerPerformance = new MessageRouterPerformance(subscriptionIndexPerformance);
        }

        private static SubscriptionIndex BuildSubscriptionIndex()
        {
            var subscriptionIndex = new SubscriptionIndex();
            var baseTypeName = typeof(RoutableMessage0).FullName!.TrimEnd('0');

            var subscriptions = from clientIndex in Enumerable.Range(0, 30)
                                let clientId = new ClientId($"Client.{clientIndex}")
                                from typeIndex in Enumerable.Range(0, 10)
                                let messageTypeId = new MessageTypeId($"{baseTypeName}{typeIndex}")
                                from contentIndex in Enumerable.Range(0, 4_000)
                                select new Subscription(clientId, messageTypeId, new ContentPattern(contentIndex.ToString()));

            subscriptionIndex.AddSubscriptions(subscriptions);
            return subscriptionIndex;
        }

        private static SubscriptionIndexPerformance BuildSubscriptionIndexPerformaance()
        {
            var subscriptionIndexPerformance = new SubscriptionIndexPerformance();
            var baseTypeName = typeof(RoutableMessage0).FullName!.TrimEnd('0');

            var subscriptions = from clientIndex in Enumerable.Range(0, 30)
                                let clientId = new ClientId($"Client.{clientIndex}")
                                from typeIndex in Enumerable.Range(0, 10)
                                let messageTypeId = new MessageTypeId($"{baseTypeName}{typeIndex}")
                                from contentIndex in Enumerable.Range(0, 4_000)
                                select new Subscription(clientId, messageTypeId, new ContentPattern(contentIndex.ToString()));

            subscriptionIndexPerformance.AddSubscriptions(subscriptions);
            return subscriptionIndexPerformance;
        }

        [Benchmark]
        public List<ClientId> GetConsumers() => _router.GetConsumers(new RoutableMessage0 { Id = 999, Value = 1234m }).ToList();


        [Benchmark]
        public List<ClientId> GetConsumersPerformance() => _routerPerformance.GetConsumersImproved(new RoutableMessage0 { Id = 999, Value = 1234m }).ToList();

        public class RoutableMessage0 : IRoutableMessage
        {
            public int Id { get; set; }
            public decimal Value { get; set; }

            public MessageRoutingContent GetContent() => new(Id.ToString(), Value.ToString());
        }
    }
}
