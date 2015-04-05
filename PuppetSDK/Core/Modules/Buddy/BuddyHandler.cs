using Puppet.Core.Model;
using Puppet.Core.Modules.Buddy;
using Puppet.Core.Network.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Modules.Buddy
{
    public class BuddyHandler : BaseSingleton<BuddyHandler>
    {
        IBuddyManager buddyManager;

        protected override void Init()
        {
            if (buddyManager == null)
                buddyManager = new CSFBuddy();
        }

        internal IBuddyManager GetAddOn
        {
            get
            {
                return buddyManager;
            }
        }

        public void AddBuddy(DataBuddy buddy)
        {
            buddyManager.AddBuddy(buddy);
        }

        public void RemoveBuddy(DataBuddy buddy)
        {
            buddyManager.RemoveBuddy(buddy);
        }

        public List<DataBuddy> Buddies { get { return buddyManager.Buddies; } }

        public List<DataBuddy> BuddyOnline { get { return buddyManager.BuddyOnline; } }

        public List<DataBuddy> BuddyOffline { get { return buddyManager.BuddyOffline; } }

        public List<DataBuddy> BuddyWaiting { get { return buddyManager.BuddyWaiting; } }

        public List<DataBuddy> BuddyInvited { get { return buddyManager.BuddyInvited; } }
    }
}
