using BepInEx;
using HarmonyLib;
using System;
using UnityEngine.SceneManagement;

namespace OC2HardHorde
{
    [BepInPlugin("dev.gua.overcooked.hardhorde", "Overcooked2 HardHorde Plugin", "1.0")]
    [BepInProcess("Overcooked2.exe")]
    public class HardHordePlugin : BaseUnityPlugin
    {
        public static HardHordePlugin pluginInstance;
        private static Harmony patcher;

        public void Awake()
        {
            pluginInstance = this;
            patcher = new Harmony("dev.gua.overcooked.hardhorde");
            patcher.PatchAll(typeof(Patch));
            foreach (var patched in Harmony.GetAllPatchedMethods())
                Log("Patched: " + patched.FullDescription());
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
        }

        public static void Log(String msg) { pluginInstance.Logger.LogInfo(msg); }
    }
}