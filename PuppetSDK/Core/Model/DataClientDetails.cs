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
        public string distributor { get; set; }

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
            version.LoadDefault();
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
        }

        public Version(SerializationInfo info, StreamingContext ctxt) 
            : base(info, ctxt) 
        { 
        }

        public void LoadDefault()
        {
            this.major = 0;
            this.minor = 1;
            this.patch = 0;
            this.build = 1;
        }

        public Version(int major, int minor, int patch, int build)
        {
            this.major = major;
            this.minor = minor;
            this.patch = patch;
            this.build = build;
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}.{3}", major, minor, patch, build);
        }
    }
}
