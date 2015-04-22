using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Puppet.Core.Model
{
    public class DataUser : DataModel
    {
        public int id { get; set; }
        public string userName { get; set; }
        public int gender { get; set; }
        public string avatar { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string facebookId { get; set; }
        public int numberTotalGames { get; set; }
        public int numberWinGames { get; set; }
        public int numberLossGames { get; set; }
        
        public DataUser()
            : base()
        {
        }

        public DataUser(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
    	{				
   	 	}
    }
}
