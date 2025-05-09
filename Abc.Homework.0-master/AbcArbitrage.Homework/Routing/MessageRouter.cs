// Copyright (C) Abc Arbitrage Asset Management - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// Written by Olivier Coanet <o.coanet@abc-arbitrage.com>, 2020-10-01

using AbcArbitrage.Homework.Models;
using AbcArbitrage.Homework.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace AbcArbitrage.Homework.Routing
{
    public class MessageRouter
    {
        private readonly ISubscriptionIndex _subscriptionIndex;
        public MessageRouter(ISubscriptionIndex subscriptionIndex)
        {
            _subscriptionIndex = subscriptionIndex;
        }

        /// <summary>
        /// Gets the consumers for a given message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>IEnumerable<ClientId></returns>
        public IEnumerable<ClientId> GetConsumers(IMessage message)
        {
            var messageTypeId = MessageTypeId.FromMessage(message);
            var messageContent = MessageRoutingContent.FromMessage(message);

            foreach (var subscription in _subscriptionIndex.FindSubscriptions(messageTypeId, messageContent))
            {
                yield return subscription.ConsumerId;
            }
        }

        /// <summary>
        /// Gets the consumers for a given message, improved version.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>IEnumerable<ClientId></returns>
        public IEnumerable<ClientId> GetConsumersImproved(IMessage message)
        {
            var messageTypeId = MessageTypeId.FromMessage(message);
            var messageContent = MessageRoutingContent.FromMessage(message);

            // Use a HashSet to avoid duplicate ConsumerIds and
            // replace yield return by Select methode
            var consumerIds = new HashSet<ClientId>();
            return _subscriptionIndex.FindSubscriptions(messageTypeId, messageContent)
                .Where(subscription => consumerIds.Add(subscription.ConsumerId))
                .Select(s => s.ConsumerId);
        }
    }
}
