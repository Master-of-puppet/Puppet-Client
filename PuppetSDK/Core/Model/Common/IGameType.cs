using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Model
{
    public interface IGameType
    {
        string Name { get; set; }
        string Icon { get; set; }
        IGameplay Gameplay { get; set; }
    }
}
