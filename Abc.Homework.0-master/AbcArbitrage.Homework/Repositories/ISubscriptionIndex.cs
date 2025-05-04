// Copyright (C) Abc Arbitrage Asset Management - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// Written by Olivier Coanet <o.coanet@abc-arbitrage.com>, 2020-10-01

using AbcArbitrage.Homework.Models;
using System.Collections.Generic;

namespace AbcArbitrage.Homework.Repositories
{
    /// <summary>
    /// Stores subscriptions.
    /// </summary>
    public interface ISubscriptionIndex
    {
        /// <summary>
        /// Adds subscriptions to the IEnumerable<Subscription>.
        /// </summary>
        /// <param name="subscriptions"></param>
        void AddSubscriptions(IEnumerable<Subscription> subscriptions);

        /// <summary>
        /// Gets all subscriptions.
        /// </summary>
        /// <returns>IEnumerable<Subscription></returns>
        IEnumerable<Subscription> GetSubscriptions();

        /// <summary>
        /// Removes subscriptions from the IEnumerable<Subscription>.
        /// </summary>
        /// <param name="subscriptions"></param>
        void RemoveSubscriptions(IEnumerable<Subscription> subscriptions);

        /// <summary>
        /// Removes subscriptions for a specific consumer.
        /// </summary>
        /// <param name="consumer"></param>
        void RemoveSubscriptionsForConsumer(ClientId consumer);

        /// <summary>
        /// Finds subscriptions for a specific message type and routing content.
        /// </summary>
        /// <param name="messageTypeId"></param>
        /// <param name="routingContent"></param>
        /// <returns></returns>
        IEnumerable<Subscription> FindSubscriptions(MessageTypeId messageTypeId, MessageRoutingContent routingContent);
    }
}
