using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataChat : DataModel
    {
        public DataUser Sender { get; set; }
        public string Content { get; set; }
        public long TimeSent { get; set; }
        public bool Readed { get; set; }
        public string ReceiverName { get; set; }
        public int Type { get; set; }

        public enum ChatType
        {
            Public = 0,
            Private = 1,
            System = 2,
        }

        public DataChat() 
            : base()
        {
        }

        public DataChat(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
    	{				
   	 	}

        public DataChat(string content, ChatType type) : this ()
        {
            this.Sender = UserHandler.Instance.Self.info;
            this.Content = content;
            this.TimeSent = DateTime.Now.Ticks;
            this.Readed = false;
            this.Type = (int)type;
        }

        public DataChat(string content, string privateMessageTo) : this (content, ChatType.Private)
        {
            this.ReceiverName = privateMessageTo;
        }

        public ChatType GetChatType()
        {
            return (ChatType)this.Type;
        }

        public DateTime GetTimeSent()
        {
            return new DateTime(this.TimeSent);
        }
    }
}
