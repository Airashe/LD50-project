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

namespace LD50.Core
{
    public sealed class Application : UnitySingleton<Application>
    {
        private const string GameModeIntializationError = "{0} initialization error: {1}";

        public ApplicationConfiguration applicationConfiguration;
        public ISystemManager InputManager => inputManager;
        private ISystemManager inputManager;

        public ISystemManager DialogueManager => dialogueManager;
        private ISystemManager dialogueManager;

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
                    ChangeGameMode(value);
                }
            }
        }
        private GameMode gameMode;

        public GameObject PlayerGO => playerGo;
        private GameObject playerGo;

        protected override void SingletonIntialized()
        {
            InitializeApplication();
        }

        private void Update()
        {
            if (!IsAllManagersReady)
                return;

            GameMode = GameMode.InGame;
        }

        private void InitializeApplication()
        {
            IntializeManagers();
        }

        private void IntializeManagers()
        {
            inputManager = ManagerFactory.InitializeManager<InputManager>();
            dialogueManager = ManagerFactory.InitializeManager<DialoguesManager>();
        }

        private bool IsManagersReady()
        {
            bool result = true;
            result &= inputManager.Initialized;
            return result;
        }

        public void ChangeGameMode(GameMode mode)
        {
            switch (mode)
            {
                case GameMode.InGame:
                    IntializeIngameMode();
                    break;
                default:
                    throw new System.ArgumentException("Invalid game mode");
            }

            this.gameMode = mode;
        }

        private void IntializeIngameMode()
        {
            this.playerGo = GameObject.FindGameObjectWithTag("Player");
            if (this.playerGo == null) throw new System.Exception(string.Format(GameModeIntializationError, GameMode.InGame, "Can not find any game objects by Player tag."));

            this.playerGo.IntializeComponent<IngameLogicController>();
        }
    }
}
