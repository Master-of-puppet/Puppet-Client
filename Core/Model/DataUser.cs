using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Puppet.Core.Model
{
    public class DataUser : DataModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullAddress { get; set; }
        public string Avatar { get; set; }
        public string Role { get; set; }
        public string CreateTime { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int ExpMinCurrentLevel { get; set; }
        public int ExpMinNextLevel { get; set; }

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
