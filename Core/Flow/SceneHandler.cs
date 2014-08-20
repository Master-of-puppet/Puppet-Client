using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Patterns;
using Puppet.Core.Model;

namespace Puppet.Core.Flow
{
    public class SceneHandler : BaseSingleton<SceneHandler>
    {
        public Action<string, string, bool, float> onChangeScene;
        public IScene _currentScene;

        public override void Init()
        {
            
        }

        public IScene CurrentScene
        {
            get { return _currentScene; }
        }

        public void Scene_Back()
        {
            if(_currentScene.PrevScene != null)
            {
                IScene oldScene = _currentScene;
                ChangeScene(_currentScene.PrevScene.SceneName);
                _currentScene = _currentScene.PrevScene;
                _currentScene.OldScene = oldScene;
            }
        }

        public void Scene_Next()
        {
            if(_currentScene.NextScene != null)
            {
                IScene oldScene = _currentScene;
                ChangeScene(_currentScene.NextScene.SceneName);
                _currentScene = _currentScene.NextScene;
                _currentScene.OldScene = oldScene;
            }
        }

        public void Scene_GoTo(IScene scene)
        {
            if (scene != null)
            {
                IScene oldScene = _currentScene;
                ChangeScene(scene.SceneName);
                _currentScene = scene;
                _currentScene.OldScene = oldScene;
            }
        }

        void ChangeScene(string sceneName)
        {
            if (PuMain.Setting.Engine == EEngine.Unity)
            {
                UnityEngine.AsyncOperation asyn = UnityEngine.Application.LoadLevelAdditiveAsync(sceneName);

                if (onChangeScene != null)
                    onChangeScene(_currentScene.SceneName, sceneName, asyn.isDone, asyn.progress);
            }
        }
    }
}
