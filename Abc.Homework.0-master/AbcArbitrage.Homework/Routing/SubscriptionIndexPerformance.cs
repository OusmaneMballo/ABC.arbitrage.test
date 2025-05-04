using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AbcArbitrage.Homework.Routing
{
    public class SubscriptionIndexPerformance : ISubscriptionIndex
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
        public void RemoveSubscriptions(IEnumerable<Subscription> subscriptions)
        {
        }
        public void RemoveSubscriptionsForConsumer(ClientId consumer)
        {
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

                if (isMatchesRoutingContent(subscription.ContentPattern, routingContent))
                    result.Add(subscription);
            });

            var finalResult = result.ToList();
            if (finalResult.Count == 0)
                return Enumerable.Empty<Subscription>();

            _cache[key] = finalResult;
            return finalResult;

            //if (!_subscriptionsByType.TryGetValue(messageTypeId, out var subscriptions))
            //    return Enumerable.Empty<Subscription>();

            //return subscriptions.AsParallel().Where(s =>
            //{
            //    if (!s.MessageTypeId.Equals(messageTypeId))
            //        return false;

            //    if (!s.ContentPattern.Equals(ContentPattern.Any))
            //        return false;

            //    var subscriptionParts = s.ContentPattern.Parts;
            //    var routingParts = routingContent.Parts;

            //    if (subscriptionParts.Count == 0 || routingParts == null)
            //        return false;

            //    if (subscriptionParts[0] != "*" && subscriptionParts[0] != routingParts.ElementAtOrDefault(0))
            //        return false;

            //    if (subscriptionParts.Count == 2)
            //    {
            //        if (routingParts.Count < 2)
            //            return false;

            //        if (subscriptionParts[1] != "*" && subscriptionParts[1] != routingParts[1])
            //            return false;
            //    }

            //    return true;
            //});
        }

        private static bool isMatchesRoutingContent(ContentPattern pattern, MessageRoutingContent content)
        {
            var subscriptionParts = pattern.Parts;
            var routingParts = content.Parts;

            if (subscriptionParts.Count == 0 || routingParts == null)
                return false;

            if (subscriptionParts[0] != "*" && subscriptionParts[0] != routingParts.ElementAtOrDefault(0))
                return false;

            if (subscriptionParts.Count == 2)
            {
                if (routingParts.Count < 2)
                    return false;

                if (subscriptionParts[1] != "*" && subscriptionParts[1] != routingParts[1])
                    return false;
            }

            return true;
        }
    }
}
