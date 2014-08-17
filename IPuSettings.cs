using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet
{
    public enum EPlatform
    {
        Editor,
        iOS,
        Android,
        WebPlayer,
    }

    public enum EEngine
    {
        Base,
        Unity,
    }

    public interface IPuSettings
    {
        EPlatform Platform { get; }
        EEngine Engine { get; }
        ServerEnvironment Environment { get; }
        IServerMode ServerModeWeb { get; }
        IServerMode ServerModeBundle { get; }
        IServerMode ServerModeSocket { get; }
        string PathCache { get; }
    }
}
