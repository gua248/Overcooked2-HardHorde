using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameModes.Horde;
using HarmonyLib;
using UnityEngine;

namespace OC2HardHorde
{
    public static class Patch
    {
        private static HordeWavesData waves;
        private static int waveIndex;
        private static int[] menuCnt;
        private static RecipeList.Entry[] entries;

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(ServerHordeFlowController), "RunWaves", MethodType.Enumerator)]
        public static IEnumerable<CodeInstruction> ServerHordeFlowControllerRunWavesPatch(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();
            codes.RemoveRange(229, 7);
            return codes.AsEnumerable();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(HordeFlowMessage), "EntryAdded")]
        public static void HordeFlowMessageEntryAddedPatch(ref HordeFlowMessage message)
        {
            if (HardHordeSettings.enabled)
            {
                int totalCnt = menuCnt.Collapse((int x, int total) => total + x);
                float num = (totalCnt + 2.0f) / waves[waveIndex].m_recipes.m_recipes.Length;
                float[] weight = menuCnt.ConvertAll(x => Mathf.Max(num - x, 0f));
                KeyValuePair<int, RecipeList.Entry> weightedRandomElement = waves[waveIndex].m_recipes.m_recipes.GetWeightedRandomElement((int i, RecipeList.Entry e) => weight[i]);
                menuCnt[weightedRandomElement.Key]++;
                message.m_entry = weightedRandomElement.Value;
            }
            entries[message.m_index] = message.m_entry;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(HordeFlowMessage), "BeginWave")]
        public static void HordeFlowMessageBeginWavePatch(int waveIndex)
        {
            Patch.waveIndex = waveIndex;
            menuCnt = new int[waves[waveIndex].m_recipes.m_recipes.Length];
        }

        private static readonly FieldInfo fieldInfo_ServerHordeFlowController_m_levelConfig = AccessTools.Field(typeof(ServerHordeFlowController), "m_levelConfig");
        private static readonly FieldInfo fieldInfo_ServerHordeFlowController_m_entries = AccessTools.Field(typeof(ServerHordeFlowController), "m_entries");
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ServerHordeFlowController), "StartSynchronising")]
        public static void ServerHordeFlowControllerStartSynchronisingPatch(ServerHordeFlowController __instance)
        {
            waves = ((HordeLevelConfig)fieldInfo_ServerHordeFlowController_m_levelConfig.GetValue(__instance)).m_waves;
            entries = (RecipeList.Entry[])fieldInfo_ServerHordeFlowController_m_entries.GetValue(__instance);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(ServerHordeFlowController), "AnyAlive")]
        public static void ServerHordeFlowControllerAnyAlivePatch(ServerHordeFlowController __instance, ref bool __result)
        {
            if (HardHordeSettings.enabled && waveIndex != waves.Count - 1) __result = false;
        }

        private static readonly FieldInfo fieldInfo_ServerHordeEnemy_m_recipeCount = AccessTools.Field(typeof(ServerHordeEnemy), "m_recipeCount");
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ServerHordeEnemy), "StartSynchronising")]
        public static void ServerHordeEnemyStartSynchronisingPatch(ServerHordeEnemy __instance)
        {
            if (HardHordeSettings.enabled)
            {
                int x = (int)fieldInfo_ServerHordeEnemy_m_recipeCount.GetValue(__instance);
                fieldInfo_ServerHordeEnemy_m_recipeCount.SetValue(__instance, x + 1);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(T17TabPanel), "OnTabSelected")]
        public static void T17TabPanelOnTabSelectedPatch()
        {
            HardHordeSettings.AddUI();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ToggleOption), "OnToggleButtonPressed")]
        public static bool ToggleOptionOnToggleButtonPressedPatch(ToggleOption __instance, bool bValue)
        {
            if (__instance == HardHordeSettings.hardHordeOption)
            {
                HardHordeSettings.enabled = bValue;
                return false;
            }
            return true;
        }
    }
}
