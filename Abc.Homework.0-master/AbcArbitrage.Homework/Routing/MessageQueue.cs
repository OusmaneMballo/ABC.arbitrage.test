// Copyright (C) Abc Arbitrage Asset Management - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// Written by Olivier Coanet <o.coanet@abc-arbitrage.com>, 2020-10-06

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AbcArbitrage.Homework.Routing
{
    public class MessageQueue
    {
        private readonly ConcurrentDictionary<(MessagePriority, ClientId), ConcurrentQueue<IMessage>> _queues = new();
        
        public void EnqueueForClient(ClientId clientId, IMessage message, MessagePriority priority = MessagePriority.Normal)
        {
            // TODO

            var key = (priority, clientId);
            var queue = _queues.GetOrAdd(key, _ => new ConcurrentQueue<IMessage>());
            queue.Enqueue(message);

        }

        public bool TryDequeueForClient(ClientId clientId, [MaybeNullWhen(false)] out IMessage message)
        {
            // TODO

            foreach (var priority in SortPrioritiesByDescending())
            {
                var key = (priority, clientId);

                if (_queues.TryGetValue(key, out var queue) && queue != null)
                {
                    if (queue.TryDequeue(out message))
                    {
                        return true;
                    }
                }
            }

            message = default;
            return false;
        }

        private static IEnumerable<MessagePriority> SortPrioritiesByDescending()
        {
            return Enum.GetValues<MessagePriority>().OrderByDescending(p => (int)p);
        }
    }
}
