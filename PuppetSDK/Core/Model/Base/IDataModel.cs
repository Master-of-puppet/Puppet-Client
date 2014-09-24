using System;
using System.Collections.Generic;

namespace Puppet.Core.Model
{
    public interface IDataModel
    {
        Dictionary<string, object> ToDictionary();

        string ToString();
    }
}
