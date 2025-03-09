using StupidTemplate.Classes;
using StupidTemplate.Mods;
using static StupidTemplate.Settings;

namespace StupidTemplate.Menu
{
    internal class Buttons
    {
        public static ButtonInfo[][] buttons = new ButtonInfo[][]
        {
            new ButtonInfo[] { // Main Mods
                new ButtonInfo { buttonText = "thank you for choosing wave", method =() => Mods.placeholder(), isTogglable = false,},
            },

            new ButtonInfo[] { // Settings
                new ButtonInfo { buttonText = "Return to Main", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Returns to the main page of the menu."},
                new ButtonInfo { buttonText = "Menu", method =() => SettingsMods.MenuSettings(), isTogglable = false, toolTip = "Opens the settings for the menu."},
                new ButtonInfo { buttonText = "Movement", method =() => SettingsMods.MovementSettings(), isTogglable = false, toolTip = "Opens the movement settings for the menu."},
            },

            new ButtonInfo[] { // Advantages
                new ButtonInfo { buttonText = "placeholder", method =() => Mods.placeholder(), isTogglable = true,},
                new ButtonInfo { buttonText = "Gun Template", method =() => Mods.AetherGunTemplate(), isTogglable = true,},
            },

            new ButtonInfo[] { // Movement 
                new ButtonInfo { buttonText = "placeholder", method =() => Mods.placeholder(), isTogglable = true,},       
            },

            new ButtonInfo[] { // Fun
                new ButtonInfo { buttonText = "placeholder", method =() => Mods.placeholder(), isTogglable = true,},
            },

             new ButtonInfo[] { // Miscilaneous
                new ButtonInfo { buttonText = "placeholder", method =() => Mods.placeholder(), isTogglable = true,},
            },

             new ButtonInfo[] { // OP
                new ButtonInfo { buttonText = "Master Mods", method =() => SettingsMods.Master(), isTogglable = false },
                new ButtonInfo { buttonText = "placeholder", method =() => Mods.placeholder(), isTogglable = true,},
            },

             new ButtonInfo[] { // Visual
                new ButtonInfo { buttonText = "placeholder", method =() => Mods.placeholder(), isTogglable = true,},
            },

             new ButtonInfo[] { // Master
                new ButtonInfo { buttonText = "placeholder", method =() => Mods.placeholder(), isTogglable = true,},
            },

            new ButtonInfo[] { // Guardian Mods
                new ButtonInfo { buttonText = "placeholder", method =() => Mods.placeholder(), isTogglable = true,},

            },



             new ButtonInfo[] { // Selection
                new ButtonInfo { buttonText = "Advantages", method =() => SettingsMods.MenuSettings(), isTogglable = false,},
                new ButtonInfo { buttonText = "Movement", method =() => SettingsMods.MovementSettings(), isTogglable = false,},
                new ButtonInfo { buttonText = "Fun", method =() => SettingsMods.Fun(), isTogglable = false,},
                new ButtonInfo { buttonText = "Miscellaneous", method =() => SettingsMods.LeaderBoard(), isTogglable = false,},
                new ButtonInfo { buttonText = "Overpowered", method =() => SettingsMods.OP(), isTogglable = false,},
                new ButtonInfo { buttonText = "Visual", method =() => SettingsMods.Visual(), isTogglable = false,},
                new ButtonInfo { buttonText = "Disconnect", method =() => Mods.Disconnect(), isTogglable = false,},
            },

        };
    }
}

    