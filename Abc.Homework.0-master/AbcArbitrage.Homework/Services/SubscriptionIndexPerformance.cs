using AbcArbitrage.Homework.Models;
using AbcArbitrage.Homework.Repositories;
using AbcArbitrage.Homework.Utilities;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbcArbitrage.Homework.Routing
{
    public class SubscriptionIndexPerformance : ISubscriptionIndexPerformance
    {
        private readonly Dictionary<MessageTypeId, List<Subscription>> _subscriptionsByType = new();

        // Use a ConcurrentDictionary to cache the results of FindSubscriptions
        // avoid the recomputation
        private readonly ConcurrentDictionary<(MessageTypeId, string), List<Subscription>> _cache = new();

        public void AddSubscriptions(IEnumerable<Subscription> subscriptions)
        {
            foreach (var subscription in subscriptions)
            {
                if (!_subscriptionsByType.TryGetValue(subscription.MessageTypeId, out var list))
                {
                    list = new List<Subscription>();
                    _subscriptionsByType[subscription.MessageTypeId] = list;
                }
                list.Add(subscription);
            }
        }

        public IEnumerable<Subscription> GetSubscriptions()
        {
            return _subscriptionsByType.Values.SelectMany(subscriptions => subscriptions);
        }

        public IEnumerable<Subscription> FindSubscriptions(MessageTypeId messageTypeId, MessageRoutingContent routingContent)
        {
            var key = (messageTypeId, string.Join(",", routingContent.Parts ?? Enumerable.Empty<string>()));

            if (_cache.TryGetValue(key, out var cachedResult))
                return cachedResult;

            // Check if the dictionary contains subscriptions for the given MessageTypeId
            if (!_subscriptionsByType.TryGetValue(messageTypeId, out var subscriptions))
                return Enumerable.Empty<Subscription>();

            //use a ConcurrentBag to store the results and avoid to lock the the access
            var result = new ConcurrentBag<Subscription>();

            //Use Parallel loop to process the subscriptions in paralleland enable multi-threaded capacity
            Parallel.ForEach(subscriptions, subscription =>
            {

                if (RoutingContendHelper.IsMatchesRoutingContent(subscription, messageTypeId, routingContent))
                {
                    result.Add(subscription);
                }
            });

            var finalResult = result.ToList();
            if (finalResult.Count == 0)
                return Enumerable.Empty<Subscription>();

            _cache[key] = finalResult;
            return finalResult;

        }
    }
}
