using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataClientDetails : DataModel
    {
        public Version version { get; set; }
        public string platform { get; set; }
        public string bundleId { get; set; }
        public string uniqueId { get; set; }

        public DataClientDetails()
            : base()
        {
            LoadDefault();
        }

        public DataClientDetails(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }

        void LoadDefault()
        {
            version = new Version();
            platform = "web-html5";
            bundleId = "com.puppet.game.foxpoker";
            uniqueId = PuMain.Setting.UniqueDeviceIdentification;
        }
    }

    public class Version : DataModel
    {
        public int major { get; set; }
        public int minor { get; set; }
        public int patch { get; set; }
        public int build { get; set; }

        public Version()
            : base()
        {
            major = 0;
            minor = 1;
            patch = 0;
            build = 1;
        }

        public Version(SerializationInfo info, StreamingContext ctxt) 
            : base(info, ctxt) 
        { 
        }
    }
}
