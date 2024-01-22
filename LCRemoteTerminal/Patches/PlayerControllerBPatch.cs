using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteTerminal.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatch
    {
        private static bool disableLookInput = false;

        [HarmonyPatch("PlayerLookInput")]
        [HarmonyPrefix]
        static void playerLookInputPatch(ref bool ___disableLookInput)
        {
            ___disableLookInput = disableLookInput;
        }

        public static void SetLookInputLock(bool _disableLookInput)
        {
            disableLookInput = _disableLookInput;
        }
    }
}
