using GameNetcodeStuff;
using HarmonyLib;
using HesuLC;

namespace LethalCheater.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatch
    {
        public static bool godMode = false;
        public static bool infSprint = false;
        public static bool ignDeath = false;
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

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        static void infiniteHealth(ref int ___health, ref bool ___criticallyInjured)
        {
            if (!godMode)
                return;

            ___health = 100;
            ___criticallyInjured = false;
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void infiniteStamina(ref float ___sprintMeter)
        {
            if (!infSprint)
                return;

            ___sprintMeter = 1f;
        }

        [HarmonyPatch("AllowPlayerDeath")]
        [HarmonyPrefix]
        static bool ignoreDeath()
        {
            return !ignDeath;
        }

        public static void setGodMode(bool _godMode)
        {
            godMode = _godMode;
            Utils.displayMessage("GodMode Status", $"GodMode {(godMode ? "enabled" : "disabled")}");
        }

        public static void setInfSprint(bool _infSprint)
        {
            infSprint = _infSprint;
            Utils.displayMessage("Infinite Sprint Status", $"Infinite Sprint {(infSprint ? "enabled" : "disabled")}");
        }

        public static void setIgnoreDeath(bool _ignoreDeath)
        {
            ignDeath = _ignoreDeath;
            Utils.displayMessage("Ignore Death Status", $"Ignore Death {(ignDeath ? "enabled" : "disabled")}");
        }
    }
}
