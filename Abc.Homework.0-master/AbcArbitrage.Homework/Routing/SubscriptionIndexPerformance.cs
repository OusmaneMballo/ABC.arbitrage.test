using System.Collections.Generic;
using System.Linq;

namespace AbcArbitrage.Homework.Routing
{
    public class SubscriptionIndexPerformance : ISubscriptionIndex
    {
        private readonly Dictionary<MessageTypeId, List<Subscription>> _subscriptionsByType = new();
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

        public IEnumerable<Subscription> GetSubscriptionsByMessageType(MessageTypeId key)
        {
            return _subscriptionsByType.Where(kvp => kvp.Key.Equals(key))
                .SelectMany(kvp => kvp.Value);
        }
        public void RemoveSubscriptions(IEnumerable<Subscription> subscriptions)
        {
        }
        public void RemoveSubscriptionsForConsumer(ClientId consumer)
        {
        }
        public IEnumerable<Subscription> FindSubscriptions(MessageTypeId messageTypeId, MessageRoutingContent routingContent)
        {
            // Check if the dictionary contains subscriptions for the given MessageTypeId
            if (!_subscriptionsByType.TryGetValue(messageTypeId, out var subscriptions))
                return Enumerable.Empty<Subscription>();

            return subscriptions.AsParallel().Where(s =>
            {
                if (!s.MessageTypeId.Equals(messageTypeId))
                    return false;

                if (!s.ContentPattern.Equals(ContentPattern.Any))
                    return false;

                var subscriptionParts = s.ContentPattern.Parts;
                var routingParts = routingContent.Parts;

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
            });
        }
    }
}
