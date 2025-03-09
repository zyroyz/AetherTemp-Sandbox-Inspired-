using static StupidTemplate.Menu.Main;
using static StupidTemplate.Settings;

namespace StupidTemplate.Mods
{
    internal class SettingsMods
    {
        public static void EnterSettings()
        {
            buttonsType = 1;
        }

        public static void MenuSettings()
        {
            buttonsType = 2;
        }

        public static void MovementSettings()
        {
            buttonsType = 3;
        }

        public static void Fun()
        {
            buttonsType = 4;
        }

        public static void LeaderBoard()
        {
            buttonsType = 5;
        }

        public static void OP()
        {
            buttonsType = 6;
        }

        public static void Visual()
        {
            buttonsType = 7;
        }

        public static void Master()
        {
            buttonsType = 8;
        }

        public static void GuardianMods()
        {
            buttonsType = 9;
        }

        public static void RightHand()
        {
            rightHanded = true;
        }

        public static void LeftHand()
        {
            rightHanded = false;
        }

        public static void EnableFPSCounter()
        {
            fpsCounter = true;
        }

        public static void DisableFPSCounter()
        {
            fpsCounter = false;
        }

        public static void EnableNotifications()
        {
            disableNotifications = false;
        }

        public static void DisableNotifications()
        {
            disableNotifications = true;
        }

        public static void EnableDisconnectButton()
        {
            disconnectButton = true;
        }

        public static void DisableDisconnectButton()
        {
            disconnectButton = false;
        }
    }
}
