using AbcArbitrage.Homework.Models;
using System.Linq;

namespace AbcArbitrage.Homework.Utilities
{
    public static class RoutingContendHelper
    {
        public static bool IsMatchesRoutingContent(Subscription subscription, MessageTypeId messageTypeId, MessageRoutingContent content)
        {
            if (!subscription.MessageTypeId.Equals(messageTypeId))
                return false;

            if (subscription.ContentPattern.Equals(ContentPattern.Any))
                return true;

            var subscriptionParts = subscription.ContentPattern.Parts;
            var routingParts = content.Parts;

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
        }
    }
}
