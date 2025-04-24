// Copyright (C) Abc Arbitrage Asset Management - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// Written by Olivier Coanet <o.coanet@abc-arbitrage.com>, 2020-10-01

using System.Collections.Generic;

namespace AbcArbitrage.Homework.Routing
{
    /// <summary>
    /// Stores subscriptions.
    /// </summary>
    public interface ISubscriptionIndex
    {
        void AddSubscriptions(IEnumerable<Subscription> subscriptions);
        void RemoveSubscriptions(IEnumerable<Subscription> subscriptions);

        void RemoveSubscriptionsForConsumer(ClientId consumer);

        IEnumerable<Subscription> FindSubscriptions(MessageTypeId messageTypeId, MessageRoutingContent routingContent);
    }
}
