using AbcArbitrage.Homework.Models;
using AbcArbitrage.Homework.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace AbcArbitrage.Homework.Routing
{
    public class MessageRouterPerformance
    {
        private readonly ISubscriptionIndexPerformance _subscriptionIndexPerformance;

        public MessageRouterPerformance(ISubscriptionIndexPerformance subscriptionIndexPerformance)
        {
            _subscriptionIndexPerformance = subscriptionIndexPerformance;
        }

        /// <summary>
        /// Gets the consumers for a given message.
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
            return _subscriptionIndexPerformance.FindSubscriptions(messageTypeId, messageContent)
                .Where(subscription => consumerIds.Add(subscription.ConsumerId))
                .Select(s => s.ConsumerId);
        }
    }
}
