using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Model
{
    public interface IGameplay
    {
        void BeginRound();
        void EndRound();
        void ChangeState(string oldState, string newState);
    }
}
