// Copyright (C) Abc Arbitrage Asset Management - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// Written by Olivier Coanet <o.coanet@abc-arbitrage.com>, 2020-10-01

using System.Collections.Generic;

namespace AbcArbitrage.Homework.Routing
{
    public class MessageRouter
    {
        private readonly ISubscriptionIndex _subscriptionIndex;

        public MessageRouter(ISubscriptionIndex subscriptionIndex)
        {
            _subscriptionIndex = subscriptionIndex;
        }

        public IEnumerable<ClientId> GetConsumers(IMessage message)
        {
            var messageTypeId = MessageTypeId.FromMessage(message);
            var messageContent = MessageRoutingContent.FromMessage(message);

            foreach (var subscription in _subscriptionIndex.FindSubscriptions(messageTypeId, messageContent))
            {
                yield return subscription.ConsumerId;
            }
        }
    }
}
