// Copyright (C) Abc Arbitrage Asset Management - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// Written by Olivier Coanet <o.coanet@abc-arbitrage.com>, 2020-10-06

using AbcArbitrage.Homework.Models;
using AbcArbitrage.Homework.Utilities;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace AbcArbitrage.Homework.Queuing
{
    public class MessageQueue
    {
        private readonly ConcurrentDictionary<(MessagePriority, ClientId), ConcurrentQueue<IMessage>> _queues = new();

        /// <summary>
        /// Enqueue a message for a specific client with a given priority.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="message"></param>
        /// <param name="priority"></param>
        public void EnqueueForClient(ClientId clientId, IMessage message, MessagePriority priority = MessagePriority.Normal)
        {
            // TODO

            var key = (priority, clientId);
            var queue = _queues.GetOrAdd(key, _ => new ConcurrentQueue<IMessage>()); //thread safe without lock more faster 
            queue.Enqueue(message);

        }

        /// <summary>
        /// Dequeue a message for a specific client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool TryDequeueForClient(ClientId clientId, [MaybeNullWhen(false)] out IMessage message)
        {
            // TODO

            foreach (var priority in MessageQueueHelper.SortPrioritiesByDescending())
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
    }
}
