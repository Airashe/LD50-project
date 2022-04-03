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
using LD50Application = LD50.Core.Application;
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
                    IntitalizeLogicController<IngameLogicController>(mode);
                    break;
                default:
                    throw new Exception($"Unkown game mode {mode}");
                    break;
            }
        }

        private static void IntitalizeLogicController<TLogicController>(GameMode mode) where TLogicController : Component, IInitializable, ILogicController
        {
            var playerGO = LD50Application.Instance.PlayerGO;
            if (playerGO == null) throw new System.Exception(string.Format(GameModeIntializationError, mode, "Can not find any game objects by Player tag."));

            playerGO.IntializeComponent<TLogicController>();
        }
    }
}
