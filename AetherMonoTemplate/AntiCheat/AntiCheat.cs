using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Anti.cheat
{
    // Token: 0x02000030 RID: 48
    internal class AntiCheat : MonoBehaviour
    {
        // Token: 0x06000125 RID: 293 RVA: 0x00016D78 File Offset: 0x00014F78
        public void Update(ref Photon.Realtime.Player newMasterClient, ref string playerID, ref string _suspiciousReason, ref string _suspiciousPlayerName, ref string _suspiciousPlayerId, ref string _sendReport, ref string suspiciousReason, ref string suspiciousPlayerName, ref string suspiciousPlayerId, ref string sendReport, ref string suspicousNick, ref string suspicousReason, ref string suspicousID, ref string susReason, ref string susId, ref string susNick)
        {
            GorillaNot.instance.rpcCallLimit = 6942069;
            if (AntiCheat.start2)
            {
                susReason = null;
                susReason = null;
                susId = null;
                susNick = null;
                suspicousNick = null;
                suspicousReason = null;
                suspicousID = null;
                suspiciousPlayerName = null;
                suspiciousPlayerId = null;
                suspiciousReason = null;
                _sendReport = null;
                _suspiciousPlayerId = null;
                _suspiciousPlayerName = null;
                _suspiciousReason = null;
                playerID = null;
                GorillaNot.instance.enabled = false;
                this.muteButton = null;
                this.reportButton = null;
                this.reportedCheating = false;
                this.reportedCheating = false;

                Photon.Realtime.Player localPlayer = PhotonNetwork.LocalPlayer;  // Fully qualify Photon.Realtime.Player
                GorillaComputer.instance.currentName = "";
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
                PhotonNetwork.NickName = "";
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
                PhotonNetwork.LocalPlayer.NickName = "";
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
                GorillaLocomotion.Player.Instance.name = "";  // Fully qualify GorillaLocomotion.Player
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
                PhotonNetwork.LocalPlayer.NickName = "";
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
            }
        }

        public static int ActorNum;
        public static int twoInt;
        public static int MasterId;
        public static bool start;
        public static bool start2;
        public static bool start3;
        public GorillaPlayerLineButton muteButton;
        public GorillaPlayerLineButton reportButton;
        private bool reportedCheating;
    }
}
