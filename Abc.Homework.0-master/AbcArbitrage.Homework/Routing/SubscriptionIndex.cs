// Copyright (C) Abc Arbitrage Asset Management - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// Written by Olivier Coanet <o.coanet@abc-arbitrage.com>, 2020-10-01

using System.Collections.Generic;
using System.Linq;

namespace AbcArbitrage.Homework.Routing
{
    public class SubscriptionIndex : ISubscriptionIndex
    {

        private IEnumerable<Subscription> _subscriptions = new List<Subscription>();

        public void AddSubscriptions(IEnumerable<Subscription> subscriptions)
        {
            // TODO
            _subscriptions = _subscriptions.Concat(subscriptions);
        }

        public IEnumerable<Subscription> GetSubscriptions()
        {
            return _subscriptions;
        }

        public void RemoveSubscriptions(IEnumerable<Subscription> subscriptions)
        {
            // TODO

            if(subscriptions == null) return;
            _subscriptions = _subscriptions.Except(subscriptions);
        }

        public void RemoveSubscriptionsForConsumer(ClientId consumer)
        {
            // TODO

            if (consumer.Equals(null)) return;
            _subscriptions = _subscriptions.Where(s => !s.ConsumerId.Equals(consumer));
        }

        public IEnumerable<Subscription> FindSubscriptions(MessageTypeId messageTypeId, MessageRoutingContent routingContent)
        {
            // TODO

            if (string.IsNullOrEmpty(messageTypeId.ToString()) && routingContent.Parts!.Count() == 0)
                yield break;

            foreach (var s in _subscriptions)
            {
                if (!s.MessageTypeId.Equals(messageTypeId))
                    continue;

                if (s.ContentPattern.Equals(ContentPattern.Any))
                {
                    yield return s;
                    continue;
                }

                var subscriptionParts = s.ContentPattern.Parts;
                var routingParts = routingContent.Parts;

                if (subscriptionParts.Count == 0)
                    continue;

                if (routingParts == null)
                    continue;

                if (subscriptionParts[0] != "*" && subscriptionParts[0] != routingParts!.ElementAtOrDefault(0))
                    continue;

                if (subscriptionParts.Count == 2)
                {
                    if (routingParts!.Count < 2)
                        continue;

                    if (subscriptionParts[1] != "*" && subscriptionParts[1] != routingParts[1])
                        continue;
                }

                yield return s;
            }

            yield break;
        }

        public IEnumerable<Subscription> FindSubscriptionsImproved(MessageTypeId messageTypeId, MessageRoutingContent routingContent)
        {

            if (string.IsNullOrEmpty(messageTypeId.ToString()) && routingContent.Parts!.Count() == 0)
                yield break;

            // Use PLINQ for parallel processing
            var matchingSubscriptions = _subscriptions.AsParallel().Where(s =>
            {
                if (!s.MessageTypeId.Equals(messageTypeId))
                    return false;

                if (s.ContentPattern.Equals(ContentPattern.Any))
                    return true;

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

            foreach (var subscription in matchingSubscriptions)
            {
                yield return subscription;
            }
        }


    }
}
