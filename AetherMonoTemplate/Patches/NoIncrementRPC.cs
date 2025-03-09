using System;
using HarmonyLib;
using UnityEngine;

namespace Patches
{
    [HarmonyPatch(typeof(VRRig), "IncrementRPC", new Type[]
    {
        typeof(PhotonMessageInfoWrapped),
        typeof(string)
    })]
    public class NoIncrementRPC : MonoBehaviour
    {
        // Token: 0x06000017 RID: 23 RVA: 0x000021CC File Offset: 0x000003CC
        private static bool Prefix(PhotonMessageInfoWrapped info, string sourceCall)
        {
            return false;
        }
    }
}
