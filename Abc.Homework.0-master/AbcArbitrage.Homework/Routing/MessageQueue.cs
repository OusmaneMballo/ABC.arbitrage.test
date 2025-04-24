// Copyright (C) Abc Arbitrage Asset Management - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// Written by Olivier Coanet <o.coanet@abc-arbitrage.com>, 2020-10-06

using System.Diagnostics.CodeAnalysis;

namespace AbcArbitrage.Homework.Routing
{
    public class MessageQueue
    {
        public void EnqueueForClient(ClientId clientId, IMessage message, MessagePriority priority = MessagePriority.Normal)
        {
            // TODO
        }

        public bool TryDequeueForClient(ClientId clientId, [MaybeNullWhen(false)] out IMessage message)
        {
            // TODO

            message = default;

            return false;
        }
    }
}
