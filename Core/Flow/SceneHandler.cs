using System;
using System.Collections.Generic;
using Puppet;
using Puppet.Core.Model;

namespace Puppet.Core.Flow
{
    public class SceneHandler : BaseSingleton<SceneHandler>
    {
        IScene _currentScene;
        EScene _lastScene = EScene.LoginScreen;
        List<IScene> allSceneInGame = new List<IScene>();

        public override void Init()
        {
            Current = SceneLogin.Instance;

            IScene temp = _currentScene;
            while (temp != null && !allSceneInGame.Contains(temp))
            {
                allSceneInGame.Add(temp);
                temp = temp.NextScene;
            }
        }

        public IScene Current
        {
            get { return _currentScene; }
            set
            {
                _lastScene = _currentScene.SceneType;
                Logger.Log("EndScene - {0}", Current.SceneName);
                _currentScene.EndScene();
                _currentScene = value;
                Logger.Log("BeginScene - {0}", Current.SceneName);
                _currentScene.BeginScene();
            }
        }

        public EScene LastScene
        {
            get { return _lastScene; }
        }

        public void Scene_Back()
        {
            if(_currentScene.PrevScene != null)
            {
                ChangeScene(_currentScene.PrevScene.SceneName);
                Current = _currentScene.PrevScene;
            }
        }

        public void Scene_Next()
        {
            if(_currentScene.NextScene != null)
            {
                ChangeScene(_currentScene.NextScene.SceneName);
                Current = _currentScene.NextScene;
            }
        }

        public void Scene_GoTo(EScene sceneType)
        {
            IScene scene = allSceneInGame.Find(s =>s.SceneType == sceneType);
            if (scene != null)
            {
                ChangeScene(scene.SceneName);
                Current = scene;
            }
            else
                Logger.LogError("Did not find scene fit");
        }

        void ChangeScene(string sceneName)
        {
            PuMain.Setting.ActionChangeScene(Current.SceneName, sceneName);
        }
    }
}
