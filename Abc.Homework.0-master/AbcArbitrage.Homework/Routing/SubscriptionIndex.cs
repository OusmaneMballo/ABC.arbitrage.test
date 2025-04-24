// Copyright (C) Abc Arbitrage Asset Management - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// Written by Olivier Coanet <o.coanet@abc-arbitrage.com>, 2020-10-01

using System.Collections.Generic;

namespace AbcArbitrage.Homework.Routing
{
    public class SubscriptionIndex : ISubscriptionIndex
    {
        public void AddSubscriptions(IEnumerable<Subscription> subscriptions)
        {
            // TODO
        }

        public void RemoveSubscriptions(IEnumerable<Subscription> subscriptions)
        {
            // TODO
        }

        public void RemoveSubscriptionsForConsumer(ClientId consumer)
        {
            // TODO
        }

        public IEnumerable<Subscription> FindSubscriptions(MessageTypeId messageTypeId, MessageRoutingContent routingContent)
        {
            // TODO

            yield break;
        }
    }
}
