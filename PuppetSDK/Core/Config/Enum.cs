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

    public enum EAssets
    {
        [AttributeAsset("chip")]
        Chip,
        [AttributeAsset("gold")]
        Gold,
    }

    public enum ECardRank
    {
        Undefined = 0,
        Ace = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
    }

    public enum ECardSuit
    {
        Undefined = -1,
        Bitch = 0,
        Spade = 1,
        Diamond = 2,
        Heart = 3,
    }

    
}
