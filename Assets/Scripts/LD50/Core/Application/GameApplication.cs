using Airashe.UCore.Common.Behaviours;
using Airashe.UCore.Systems;
using Airashe.UCore.Systems.Input;
using LD50.Core.Controllers;
using LD50.Core.Enums;
using LD50.Core.Managers;
using System.Collections;
using System.Collections.Generic;
using LD50.Core.Extensions;
using UnityEngine;
using LD50.Core.ScriptableObjects;
using LD50.DialogueSystem.Managers;
using Assets.Scripts.LD50.Core.Application.Structs;
using UnityApplication = UnityEngine.Application;
using UnityEngine.SceneManagement;
using Assets.Scripts.LD50.EventSystem.Manager;
using Assets.Scripts.LD50.VisualNovelSystem.Managers;
using Assets.Scripts.LD50.TickSystem.Managers;
using Assets.Scripts.LD50.GameContextSystem.Managers;

namespace LD50.Core
{
    public sealed class GameApplication : UnitySingleton<GameApplication>
    {
        public ApplicationConfiguration applicationConfiguration;

        public UnitySceneContext CurrentSceneContext => currentSceneContext;
        private UnitySceneContext currentSceneContext;
        public ISystemManager InputManager => inputManager;
        private ISystemManager inputManager;

        public ISystemManager DialogueManager => dialogueManager;
        private ISystemManager dialogueManager;

        public ISystemManager EventManager => eventManager;
        private ISystemManager eventManager;

        public ISystemManager VisualNovelManager => visualNovelManager;
        public ISystemManager visualNovelManager;

        public ISystemManager TickManager => tickManager;
        public ISystemManager tickManager;

        public ISystemManager GameContextManager => gameContextManager;
        public ISystemManager gameContextManager;

        private bool IsAllManagersReady
        {
            get
            {
                if (!isAllManagerReady)
                {
                    isAllManagerReady = IsManagersReady();
                }
                return isAllManagerReady;
            }
            set => isAllManagerReady = value;
        }
        private bool isAllManagerReady;

        public GameMode GameMode
        {
            get => gameMode;
            set
            {
                if (gameMode != value)
                {
                    GameModeInitializer.IntitalizeMode(value);
                    this.gameMode = value;
                }
            }
        }
        private GameMode gameMode;

        public GameObject PlayerGO => playerGo;
        private GameObject playerGo;

        private bool sceneIntialized;
        private bool sceneLoaded;

        protected override void SingletonIntialized()
        {
            InitializeApplication();
        }

        private void Update()
        {
            if (!IsAllManagersReady)
                return;

            if (!sceneIntialized && sceneLoaded)
                IntitalizeScene();
        }

        private void InitializeApplication()
        {
            sceneLoaded = true;
            sceneIntialized = false;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            IntializeManagers();
        }

        private void OnActiveSceneChanged(Scene arg0, Scene arg1)
        {
            sceneLoaded = true;
            IntitalizeScene();
        }

        public void LoadScene(string sceneName)
        {
            sceneLoaded = false;
            SceneManager.LoadScene(sceneName);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }

        public void IntitalizeScene()
        {
            sceneIntialized = false;

            var contextGO = GameObject.FindGameObjectWithTag("SceneContext");
            if (contextGO == null) throw new System.Exception("Scene context could not be found");

            currentSceneContext = contextGO.GetComponent<UnitySceneContext>();

            if (currentSceneContext == null) throw new System.Exception("Scene context script not found.");

            if (!IsAllManagersReady)
                return;

            playerGo = GameObject.FindGameObjectWithTag("Player");

            sceneIntialized = true;

            GameMode = CurrentSceneContext.IntitalGameMode;
        }

        private void IntializeManagers()
        {
            inputManager = ManagerFactory.InitializeManager<InputManager>();
            dialogueManager = ManagerFactory.InitializeManager<DialoguesManager>();
            eventManager = ManagerFactory.InitializeManager<EventManager>();
            visualNovelManager = ManagerFactory.InitializeManager<VisualNovelManager>();
            tickManager = ManagerFactory.InitializeManager<TickManager>();
            gameContextManager = ManagerFactory.InitializeManager<GameContextManager>();
        }

        private bool IsManagersReady()
        {
            bool result = true;
            result &= inputManager.Initialized;
            result &= dialogueManager.Initialized;
            return result;
        }
    }
}
