using BepInEx;
using BalloonFloater.Scripts;
using BalloonFloater.ComputerInterface;
using Bepinject;
using Utilla;

namespace BalloonFloater
{
    [ModdedGamemode] // Requires Utilla
    [BepInDependency("tonimacaroni.computerinterface", "1.5.3")] // For the Computer Interface view
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.6")] // For the modded gamemodes
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance;
        public BFManager bfmanager;
        public BFRecovery recovery;
        public BFData data;

        public bool inRoom;
        public int EquippedBalloons = 0;

        internal void Awake()
        {
            Instance = this;
            BFLogger.LogMessage("BalloonFloater has been awoken", BFLogger.LogType.Default);

            HarmonyPatches.ApplyHarmonyPatches();
            BFLogger.LogMessage("Applied harmony patches", BFLogger.LogType.Default);

            Zenjector.Install<MainInstaller>().OnProject();
            BFLogger.LogMessage("Ran MainInstaller", BFLogger.LogType.Default);
        }

        internal void Start()
        {
            bfmanager = gameObject.AddComponent<BFManager>();
            BFLogger.LogMessage("Created BFManager", BFLogger.LogType.Default);

            recovery = new BFRecovery();
            data = recovery.GetData();
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
