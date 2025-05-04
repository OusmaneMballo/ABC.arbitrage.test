using AbcArbitrage.Homework.Models;
using System.Collections.Generic;

namespace AbcArbitrage.Homework.Repositories
{
    public interface ISubscriptionIndexPerformance
    {
        /// <summary>
        /// Adds subscriptions to the dictionary.
        /// </summary>
        /// <param name="subscriptions"></param>
        void AddSubscriptions(IEnumerable<Subscription> subscriptions);

        /// <summary>
        /// Gets all subscriptions.
        /// </summary>
        /// <returns>IEnumerable<Subscription></returns>
        IEnumerable<Subscription> GetSubscriptions();

        /// <summary>
        /// Finds subscriptions for a given message type and routing content.
        /// </summary>
        /// <param name="messageTypeId"></param>
        /// <param name="routingContent"></param>
        /// <returns>IEnumerable<Subscription></returns>
        IEnumerable<Subscription> FindSubscriptions(MessageTypeId messageTypeId, MessageRoutingContent routingContent);
    }
}
