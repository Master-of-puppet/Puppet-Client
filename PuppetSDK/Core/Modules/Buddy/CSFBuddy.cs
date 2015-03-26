using Puppet.Core.Network.Socket;
using Puppet.Utils;
using Sfs2X.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Sfs2X.Entities;
using sfBuddy = Sfs2X.Entities.Buddy;
using Puppet.Core.Model;

namespace Puppet.Core.Modules.Buddy
{
    internal class CSFBuddy : IBuddyManager, ISocketAddOn
    {
        CSmartFox socket;

        #region for ISocketAddOn
        public bool Initialized { get { return socket != null; } }

        public void Begin(ISocket socket)
        {
            this.socket = (CSmartFox)socket;

            foreach (FieldInfo info in Utility.GetFieldInfo(typeof(SFSBuddyEvent), BindingFlags.Public | BindingFlags.Static))
                this.socket.SmartFox.AddEventListener(info.GetValue(null).ToString(), ListenerDelegate);
        }

        public void End()
        {
            foreach (FieldInfo info in Utility.GetFieldInfo(typeof(SFSBuddyEvent), BindingFlags.Public | BindingFlags.Static))
                this.socket.SmartFox.RemoveEventListener(info.GetValue(null).ToString(), ListenerDelegate);
        }

        public void ProcessMessage(string type, ISocketResponse response)
        {
            
        }
        #endregion

        void ListenerDelegate(BaseEvent evt)
        {
            if (PuMain.Setting.IsDebug)
            {
                Logger.Log("SFBuddy: type [" + evt.Type + "]", JsonUtil.Serialize(evt.Params), ELogColor.CYAN);
            }

            if (evt.Type == SFSBuddyEvent.BUDDY_ADD)
            {
            }
            else if(evt.Type == SFSBuddyEvent.BUDDY_ONLINE_STATE_UPDATE || evt.Type == SFSBuddyEvent.BUDDY_VARIABLES_UPDATE)
            {
                foreach(sfBuddy b in socket.SmartFox.BuddyManager.BuddyList)
                {
                    SFSBuddy buddy = b as SFSBuddy;
                }
            }
        }

        #region for IBuddyManager
        public void AddBuddy(DataBuddy buddy)
        {
            foreach(Sfs2X.Entities.User user in socket.SmartFox.RoomManager.GetRoomById(PuGlobal.Instance.SelectedRoomJoin.roomId).UserList)
            {
                Logger.Log("User In Room: " + UserHandler.Instance.ConvertUser(user).ToString());
            }
            //socket.SmartFox.BuddyManager.AddBuddy(new SFSBuddy(buddy.id, buddy.name));
        }

        public void RemoveBuddy(DataBuddy buddy)
        {
            if (!string.IsNullOrEmpty(buddy.name))
                socket.SmartFox.BuddyManager.RemoveBuddyByName(buddy.name);
            else
                socket.SmartFox.BuddyManager.RemoveBuddyById(buddy.id);
        }

        public List<DataBuddy> Buddies
        {
            get { return ConvertBuddies(socket.SmartFox.BuddyManager.BuddyList); }
        }

        public List<DataBuddy> BuddyOnline
        {
            get { return ConvertBuddies(socket.SmartFox.BuddyManager.OnlineBuddies); }
        }

        public List<DataBuddy> BuddyOffline
        {
            get { return ConvertBuddies(socket.SmartFox.BuddyManager.OfflineBuddies); }
        }

        public List<DataBuddy> BuddyWaiting
        {
            get { throw new NotImplementedException(); }
        }

        public List<DataBuddy> BuddyInvited
        {
            get { throw new NotImplementedException(); }
        }
        #endregion

        public DataBuddy ConvertBuddy(sfBuddy buddy)
        {
            DataBuddy b = new DataBuddy(buddy.Id, buddy.Name);
            b.isBlocked = buddy.IsBlocked;
            b.isOnline = buddy.IsOnline;
            b.nickName = buddy.NickName;
            b.state = buddy.State;
            return b;
        }

        public List<DataBuddy> ConvertBuddies(List<sfBuddy> listBuddy)
        {
            List<DataBuddy> list = new List<DataBuddy>();
            foreach(sfBuddy b in listBuddy)
                list.Add(ConvertBuddy(b));
            return list;
        }
    }
}
