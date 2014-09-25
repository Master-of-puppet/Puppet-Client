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

    public enum EScene
    {
        None = 0,
        SplashScreen = 1,
        LoginScreen = 2,
        World_Game = 3,
        Pocker_Plaza = 4,
        Pocker_Lobby = 5,
        Pocker_Gameplay = 6,
    }

    public enum EMessage
    {
        Critical,
        Notify,
    }

    public enum EUpgrade
    {
        None,
        ForceUpdate = 100,
        MaybeUpdate = 200,
        NotUpdate = 300,
    }
}
