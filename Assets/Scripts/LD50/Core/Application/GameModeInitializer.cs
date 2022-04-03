using Assets.Scripts.LD50.Controllers.VisualNovelControllers;
using LD50.Controllers.Interfaces;
using LD50.Core.Controllers;
using LD50.Core.Enums;
using LD50.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LD50Application = LD50.Core.GameApplication;
namespace LD50.Core
{ 
    public static class GameModeInitializer
    {
        private const string GameModeIntializationError = "{0} initialization error: {1}";
        public static void IntitalizeMode(GameMode mode)
        {
            switch(mode)
            {
                case GameMode.InGame:
                    IntitalizeLogicController<IngameLogicController>(mode);
                    break;
                case GameMode.VisualNovel:
                    IntitalizeLogicController<VNLogicController>(mode);
                    break;
                default:
                    throw new Exception($"Unkown game mode {mode}");
            }
        }

        private static void IntitalizeLogicController<TLogicController>(GameMode mode) where TLogicController : Component, IInitializable, ILogicController
        {
            var playerGO = LD50Application.Instance.PlayerGO;

            var logicController = playerGO.IntializeComponent<TLogicController>();
            logicController.ControlledUnit = LD50Application.Instance.CurrentSceneContext.PlayerUnit;

            int modeMask = 0;
            switch (mode)
            {
                case GameMode.InGame:
                    modeMask = LD50Application.Instance.applicationConfiguration.IngameLayerMask.value;
                    break;
                case GameMode.VisualNovel:
                    modeMask = LD50Application.Instance.applicationConfiguration.VisualNovelLayerMask.value;
                    break;
                default:
                    throw new Exception($"Unkown game mode {mode}");
            }

            playerGO.GetComponentInChildren<Camera>().cullingMask = modeMask;
        }
    }
}
