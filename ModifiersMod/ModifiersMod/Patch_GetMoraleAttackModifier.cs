using BattleTech;
using Harmony;

namespace ModifiersMod
{
 /*   [HarmonyPatch(typeof(ToHit), "GetMoraleAttackModifier")]
    public static class Patch_GetMoraleAttackModifier
    {
        public static void PostFix(ToHit __instance, ICombatant target, bool isMoraleAttack, ref float __result)
        {
            CombatGameState combat = Traverse.Create(__instance).Field("combat").GetValue<CombatGameState>();
            Logger.Debug($"in GetMoraleAttackModifier");
            if (!isMoraleAttack)
            {
                Logger.Debug($"not a called shot.");
                __result = 0f;
            }
            else
            {
                Logger.Debug($"is a called shot.");
                __result = combat.Constants.ToHit.ToHitOffensivePush;
            }
        }
    }*/
}