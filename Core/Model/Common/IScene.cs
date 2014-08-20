using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Model
{
    public interface IScene
    {
        string SceneName { get; }
        EScene SceneType { get; }
        IScene NextScene { get; }
        IScene PrevScene { get; }
        IScene OldScene { get; set; }
        void BeginScene();
        void EndScene();
    }
}
