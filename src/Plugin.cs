using BepInEx;
using BepInEx.Configuration;

using System.IO;
using System.ComponentModel;

using BalloonFloater.Utils;
using BalloonFloater.Scripts;

using Utilla;
using UnityEngine;

namespace BalloonFloater
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance;
        public BFManager bfmanager;
        public BFSettings settings;

        public bool inRoom;
        public int EquippedBalloons = 0;

        internal ConfigEntry<float> playerGain;
        internal ConfigEntry<float> playerMaxGain;
        internal ConfigEntry<float> balloonGain;
        internal ConfigEntry<float> destroyTime;

        internal void Awake()
        {
            Instance = this;
            BFLogger.LogMessage("BalloonFloater has been awoken", BFLogger.LogType.Default);

            HarmonyPatches.ApplyHarmonyPatches();
            BFLogger.LogMessage("Applied harmony patches", BFLogger.LogType.Default);
        }

        internal void Start()
        {
            bfmanager = gameObject.AddComponent<BFManager>();
            BFLogger.LogMessage("Created BFManager", BFLogger.LogType.Default);
            // BF, boyfriend, balloonfloater, idk

            ConfigFile BFConfig = new ConfigFile(Path.Combine(Paths.ConfigPath, "DevBalloonFloater.cfg"), true);
            playerGain = BFConfig.Bind("Configuration", "Player Gain", 0.5f, "How much veleocity does the player gain when you're grabbing a balloon");
            playerMaxGain = BFConfig.Bind("Configuration", "Player Maximum Gain", 3f, "The maximum velocity the player gains when grabbing a balloon");
            balloonGain = BFConfig.Bind("Configuration", "Balloon Gain", 1.5f, "How much velocity does the balloon gain when you're grabbing it");
            destroyTime = BFConfig.Bind("Configuration", "Destroy Time", 0.5f, "How long does it take for the balloon to explode after you release it");
            BFLogger.LogMessage("Loaded configuration files", BFLogger.LogType.Default);

            settings = new BFSettings();
            settings.playerGain = playerGain.Value;
            settings.playerMaxGain = playerMaxGain.Value;
            settings.balloonGain = balloonGain.Value;
            settings.destroyTime = destroyTime.Value;
            BFLogger.LogMessage("Generated default settings", BFLogger.LogType.Default);
        }

        [ModdedGamemodeJoin] internal void OnJoin()
        {
            inRoom = true;
            BFLogger.LogMessage("Entered modded lobby", BFLogger.LogType.Default);
        }

        [ModdedGamemodeLeave] internal void OnLeave()
        {
            inRoom = false;
            BFLogger.LogMessage("Left modded lobby", BFLogger.LogType.Default);
        }
    }
}
