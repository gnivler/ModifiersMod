using BattleTech;
using Harmony;

namespace ModifiersMod
{
    [HarmonyPatch(typeof(ToHit), "GetMoraleAttackModifier")]
    class Patch_GetMoraleAttackModifier
    {
        static void PostFix(ToHit __instance, ICombatant target, bool isMoraleAttack, ref float __result)
        {
            CombatGameState combat = Traverse.Create(__instance).Field("combat").GetValue<CombatGameState>();

            if (!isMoraleAttack)
            {
                __result = 0f;
            }
            else
            {
                __result = combat.Constants.ToHit.ToHitOffensivePush;
            }
        }
    }
}