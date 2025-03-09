using System;
using HarmonyLib;
using UnityEngine;

namespace Patches
{
    [HarmonyPatch(typeof(GorillaNot), "GetRPCCallTracker")]
    internal class NoGetRPCCallTracker : MonoBehaviour
    {
        private static bool Prefix()
        {
            return false;
        }
    }
}
