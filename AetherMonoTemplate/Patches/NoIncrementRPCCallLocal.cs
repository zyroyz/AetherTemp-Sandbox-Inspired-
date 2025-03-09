using System;
using HarmonyLib;
using UnityEngine;

namespace Patches
{
    [HarmonyPatch(typeof(GorillaNot), "IncrementRPCCallLocal")]
    public class NoIncrementRPCCallLocal : MonoBehaviour
    {
        private static bool Prefix(PhotonMessageInfoWrapped infoWrapped, string rpcFunction)
        {
            return false;
        }
    }
}
