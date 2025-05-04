using AbcArbitrage.Homework.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbcArbitrage.Homework.Utilities
{
    public static class MessageQueueHelper
    {
        public static IEnumerable<MessagePriority> SortPrioritiesByDescending()
        {
            return Enum.GetValues<MessagePriority>().OrderByDescending(p => (int)p);
        }
    }
}
