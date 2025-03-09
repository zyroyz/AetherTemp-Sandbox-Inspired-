using System;
using HarmonyLib;
using UnityEngine;

namespace Patches
{
    [HarmonyPatch(typeof(GorillaNot), "SendReport")]
    internal class AntiCheat : MonoBehaviour
    {
        private static bool Prefix(string susReason, string susId, string susNick)
        {
            return false;
        }
    }
}
