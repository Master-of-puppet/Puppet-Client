using System;
using System.Collections.Generic;

namespace Puppet.Model
{
    public interface IDataModel
    {
        Dictionary<string, object> ToDictionary();

        string ToString();
    }
}
