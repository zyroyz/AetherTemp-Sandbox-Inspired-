using HarmonyLib;
using UnityEngine;

namespace StupidTemplate.Patches 
{
    [HarmonyPatch(typeof(GorillaLocomotion.Player), "ApplyKnockback")]
    public class KnockPatch
    {
        public static bool enabled = false;

        public static bool Prefix(Vector3 direction, float speed)
        {
            return !enabled; 
        }
    }
}
