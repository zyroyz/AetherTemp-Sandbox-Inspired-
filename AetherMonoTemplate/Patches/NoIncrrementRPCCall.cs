using System;
using HarmonyLib;
using Photon.Pun;
using UnityEngine;

namespace Patches
{
    [HarmonyPatch(typeof(GorillaNot), "IncrementRPCCall", new Type[]
    {
        typeof(PhotonMessageInfo),
        typeof(string)
    })]
    public class NoIncrementRPCCall : MonoBehaviour
    {
        private static bool Prefix(PhotonMessageInfo info, string callingMethod = "")
        {
            return false;
        }
    }
}
