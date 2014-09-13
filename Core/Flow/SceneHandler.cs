using System;
using System.Collections.Generic;
using Puppet;
using Puppet.Core.Model;
using Sfs2X.Entities;
using Puppet.Core.Network.Socket;
using Sfs2X.Core;

namespace Puppet.Core.Flow
{
    public class SceneHandler : BaseSingleton<SceneHandler>
    {
        IScene _currentScene;
        EScene _lastScene = EScene.LoginScreen;
        List<IScene> allSceneInGame = new List<IScene>();

        protected override void Init()
        {
            SceneGeneric.Instance.Start();
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
                if (_currentScene != null)
                {
                    _lastScene = _currentScene.SceneType;
                    Logger.Log("EndScene - {0}", _currentScene.SceneName);
                    _currentScene.EndScene();
                }
                _currentScene = value;
                Logger.Log("BeginScene - {0}", _currentScene.SceneName);
                _currentScene.BeginScene();
            }
        }

        public EScene LastScene
        {
            get { return _lastScene; }
        }

        public void Scene_Back(string serverSceneName)
        {
            if(_currentScene.PrevScene != null)
            {
                ChangeScene(_currentScene.PrevScene, serverSceneName);
                Current = _currentScene.PrevScene;
            }
        }

        public void Scene_Next(string serverSceneName)
        {
            if(_currentScene.NextScene != null)
            {
                ChangeScene(_currentScene.NextScene, serverSceneName);
                Current = _currentScene.NextScene;
            }
        }

        public void Scene_GoTo(EScene sceneType, string serverSceneName)
        {
            IScene scene = allSceneInGame.Find(s =>s.SceneType == sceneType);
            if (scene != null)
            {
                ChangeScene(scene, serverSceneName);
                Current = scene;
            }
            else
                Logger.LogError("Did not find scene fit");
        }

        void ChangeScene(IScene scene, string serverSceneName)
        {
            if (string.IsNullOrEmpty(serverSceneName))
                PuMain.Setting.ActionChangeScene(Current.SceneName, scene.SceneName);
            else
            {
                Logger.Log("Server want going to scene: " + serverSceneName);
                PuMain.Setting.ActionChangeScene(Current.SceneName, serverSceneName);
            }
        }
    }
}
