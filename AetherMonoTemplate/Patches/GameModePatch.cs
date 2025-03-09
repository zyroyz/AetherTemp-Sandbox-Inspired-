using System;
using HarmonyLib;

namespace AxiomMasterMods
{
    [HarmonyPatch(typeof(GorillaGameManager), "ValidGameMode")]
    public class GameModePatch
    {
        public static bool Prefix(ref bool __result)
        {
            bool flag = GameModePatch.enabled;
            bool result;
            if (flag)
            {
                __result = true;
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        public static bool enabled;
    }
}
