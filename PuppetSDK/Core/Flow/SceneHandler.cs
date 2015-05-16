using System;
using System.Collections.Generic;
using Puppet;
using Puppet.Core.Model;
using Sfs2X.Entities;
using Puppet.Core.Network.Socket;
using Sfs2X.Core;
using Puppet.Utils;

namespace Puppet.Core.Flow
{
    internal class SceneHandler : BaseSingleton<SceneHandler>
    {
        IScene _currentScene;
        EScene _lastScene = EScene.LoginScreen;
        List<IScene> allSceneInGame = new List<IScene>();

        protected override void Init()
        {
            SceneGeneric.Instance.StartListenerEvent();
            Current = SceneLogin.Instance;

            IScene temp = _currentScene;
            while (temp != null && !allSceneInGame.Contains(temp))
            {
                allSceneInGame.Add(temp);
                temp = temp.NextScene;
            }
        }

        internal IScene Current
        {
            get { return _currentScene; }
            set
            {
                if (_currentScene != null)
                {
                    _lastScene = _currentScene.SceneType;
                    Logger.Log(ELogColor.GREEN, "EndScene - {0}", _currentScene.SceneName);
                    _currentScene.EndScene();
                }
                _currentScene = value;
                Logger.Log(ELogColor.GREEN, "BeginScene - {0}", _currentScene.SceneName);
                _currentScene.BeginScene();
            }
        }

        internal EScene LastScene
        {
            get { return _lastScene; }
        }

        internal void Scene_Back(string serverSceneName)
        {
            if(_currentScene.PrevScene != null)
            {
                Current = _currentScene.PrevScene;
                ChangeScene(_currentScene.PrevScene, serverSceneName);
            }
        }

        internal void Scene_Next(string serverSceneName)
        {
            if(_currentScene.NextScene != null)
            {
                Current = _currentScene.NextScene;
                ChangeScene(_currentScene.NextScene, serverSceneName);
            }
        }

        internal void Scene_GoTo(string serverSceneName)
        {
            IScene scene = allSceneInGame.Find(s => s.ServerScene == serverSceneName);
            if (scene != null)
            {
                Current = scene;
                ChangeScene(scene, serverSceneName);
            }
            else
                Logger.LogError("Did not find scene fit :1");
        }

        internal void Scene_GoTo(EScene sceneType, string serverSceneName)
        {
            IScene scene = allSceneInGame.Find(s => s.SceneType == sceneType);
            if (scene != null)
            {
                Current = scene;
                ChangeScene(scene, serverSceneName);
            }
            else
                Logger.LogError("Did not find scene fit :2");
        }

        void ChangeScene(IScene scene, string serverSceneName)
        {
            if (string.IsNullOrEmpty(serverSceneName))
                Logger.Log(ELogColor.GREEN, "Server don't want going to anywhere - So client change to: " + scene.SceneName);
            else
                Logger.Log(ELogColor.GREEN, "Server want going to scene: " + serverSceneName);

            ThreadHandler.QueueOnMainThread(() =>
            {
                PuMain.Dispatcher.SetChangeScene(_lastScene, scene.SceneType);
            });
        }
    }
}
