using HarmonyLib;
using UnityEngine;

namespace iiMenu.Patches
{
    [HarmonyPatch(typeof(BuilderPieceInteractor), "UpdateHandState")]
    public class BuilderPiecePatch
    {
        public static bool isEnabled = false;
        public static float previous = 0f;

        private static void SetScaleFactor(VRRig rig, float value)
        {
            var property = typeof(VRRig).GetProperty("scaleFactor");
            if (property != null && property.CanWrite)
            {
                property.SetValue(rig, value);
            }
        }

        private static void Prefix()
        {
            if (isEnabled)
            {
                previous = GorillaTagger.Instance.offlineVRRig.scaleFactor;
                SetScaleFactor(GorillaTagger.Instance.offlineVRRig, 1f);
            }
        }

        private static void Postfix()
        {
            if (isEnabled)
            {
                SetScaleFactor(GorillaTagger.Instance.offlineVRRig, previous);
            }
        }
    }
}