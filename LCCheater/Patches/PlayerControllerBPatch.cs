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

        public static void toggleGodMode()
        {
            godMode = !godMode;
            Utils.displayMessage("GodMode Status", $"GodMode {(godMode ? "enabled" : "disabled")}");
        }
        public static void toggleInfiniteSprint()
        {
            infSprint = !infSprint;
            Utils.displayMessage("Infinite Sprint Status", $"Infinite Sprint {(infSprint ? "enabled" : "disabled")}");
        }
    }
}
