using Puppet.Core.Model;
using Puppet.Core.Network.Socket;
using System;
using System.Collections.Generic;

namespace Puppet.Core.Modules.Buddy
{
    public interface IBuddyManager : ISocketAddOn
    {
        void AddBuddy(DataBuddy buddy);
        void RemoveBuddy(DataBuddy buddy);

        List<DataBuddy> Buddies { get; }
        List<DataBuddy> BuddyOnline { get; }
        List<DataBuddy> BuddyOffline { get; }
        List<DataBuddy> BuddyWaiting { get; }
        List<DataBuddy> BuddyInvited { get; }
    }
}
