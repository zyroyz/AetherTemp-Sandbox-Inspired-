using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;

namespace wave.Menu
{
    internal class InputLib : BaseUnityPlugin
    {

        public static bool RT()
        {
            return InputLib.instance().rightControllerIndexFloat == 1f;
        }


        public static bool LT()
        {
            return InputLib.instance().leftControllerIndexFloat == 1f;
        }


        public static bool RG()
        {
            return InputLib.instance().rightControllerGripFloat == 1f;
        }


        public static bool LG()
        {
            return InputLib.instance().leftControllerGripFloat == 1f;
        }


        public static bool X()
        {
            return InputLib.instance().leftControllerPrimaryButton;
        }

        public static bool Y()
        {
            return InputLib.instance().leftControllerSecondaryButton;
        }

        public static bool B()
        {
            return InputLib.instance().rightControllerSecondaryButton;
        }


        public static bool A()
        {
            return InputLib.instance().rightControllerPrimaryButton;
        }

        private static ControllerInputPoller instance()
        {
            return ControllerInputPoller.instance;
        }
    }
}
