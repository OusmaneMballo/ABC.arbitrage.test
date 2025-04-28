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
            {
                yield break;
            }
            foreach (Subscription s in _subscriptions)
            {
                if (s.MessageTypeId!.Equals(messageTypeId))
                {
                    if (s.ContentPattern.Equals(ContentPattern.Any))
                    {
                        yield return s;
                    }

                    if (!s.ContentPattern.Equals(ContentPattern.Any) && !routingContent.Equals(MessageRoutingContent.Empty))
                    {
                        if ((s.ContentPattern.Parts!.ElementAtOrDefault(0) == "*") ||
                            (s.ContentPattern.Parts!.ElementAtOrDefault(0) == routingContent.Parts!.ElementAt(0)))
                        {
                            if (s.ContentPattern.Parts!.Count == 2 && routingContent.Parts!.Count == 2)
                            {
                                if ((s.ContentPattern.Parts!.ElementAtOrDefault(1) == "*") ||
                                    (s.ContentPattern.Parts!.ElementAtOrDefault(1) == routingContent.Parts!.ElementAt(1)))
                                {
                                    yield return s;
                                }
                            }
                            else
                            {
                                yield return s;
                            }

                        }
                    }
                }
            }

            yield break;
        }
    }
}
