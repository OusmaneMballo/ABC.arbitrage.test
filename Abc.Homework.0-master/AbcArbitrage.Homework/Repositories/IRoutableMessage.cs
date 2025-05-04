// Copyright (C) Abc Arbitrage Asset Management - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// Written by Olivier Coanet <o.coanet@abc-arbitrage.com>, 2020-10-01


// Copyright (C) Abc Arbitrage Asset Management - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// Written by Olivier Coanet <o.coanet@abc-arbitrage.com>, 2020-10-01

using AbcArbitrage.Homework.Models;

namespace AbcArbitrage.Homework.Repositories
{
    public interface IRoutableMessage : IMessage
    {
        /// <summary>
        /// Returns the routable content of the message.
        /// </summary>
        /// <returns>MessageRoutingContent</returns>
        MessageRoutingContent GetContent();
    }
}
