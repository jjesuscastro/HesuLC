using GameNetcodeStuff;
using System.Collections.Generic;
using System.Linq;

namespace HesuLC
{
    public class Utils
    {
        static HUDManager hudManager;

        public static void displayMessage(string header, string message)
        {
            if (hudManager == null)
                hudManager = (HUDManager)UnityEngine.Object.FindObjectOfType(typeof(HUDManager));

            hudManager.DisplayTip(header, message);
        }

        public static int getPlayerIndex(string playerName)
        {
            if (playerName.Length < 3)
                playerName += "0";

            List<TransformAndName> players = StartOfRound.Instance.mapScreen.radarTargets;
            return players.FindIndex(p => p.name.ToLower() == playerName.ToLower());
        }

        public static PlayerControllerB getPlayerObject(string playerName)
        {
            if (playerName.Length < 3)
                playerName += "0";

            PlayerControllerB[] players = UnityEngine.Object.FindObjectsByType<PlayerControllerB>(UnityEngine.FindObjectsSortMode.None);

            return players.FirstOrDefault(p => p.playerUsername.ToLower() == playerName.ToLower());
        }

        public static PlayerControllerB getPlayerClient()
        {
            PlayerControllerB[] players = (PlayerControllerB[])UnityEngine.Object.FindObjectsByType<PlayerControllerB>(UnityEngine.FindObjectsSortMode.None);

            return GameNetworkManager.Instance.localPlayerController ?? players.FirstOrDefault(p => p.IsLocalPlayer);
        }
    }
}
