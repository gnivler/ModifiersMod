using BattleTech;
using Harmony;

namespace ModifiersMod
{
    // credit to FlukeyFiddler's original example
    [HarmonyPatch(typeof(ToHit), "GetMoraleAttackModifier")]
    public static class Patch_GetMoraleAttackModifier
    {
        public static void Prefix(ToHit __instance, ICombatant target, bool isMoraleAttack, CombatGameState ___combat, ref float __result)
        {
            //CombatGameState combat = Traverse.Create(__instance).Field("combat").GetValue<CombatGameState>();

            __result = ___combat.Constants.ToHit.ToHitOffensivePush + ModifiersMod.settings.ChangeAmount;

            // not used yet this is a new thing on StatCollection
            //float calculatedModifier = offensivePushModifier + target.StatCollection.GetValue<float>("ToHitOffensivePushModifier");  /// 
            //__result = (!isMoraleAttack) ? 0f : offensivePushModifier + target.StatCollection.GetValue<float>("ToHitOffensivePushModifier");

            if (isMoraleAttack)
            {
                //Logger.Debug($"ToHitOffensivePush: {offensivePushModifier}, ChangeAmount: {ModifiersMod.settings.ChangeAmount}");
                Logger.Debug($"Offensive Push modifier should be" +
                             $" {___combat.Constants.ToHit.ToHitOffensivePush + ModifiersMod.settings.ChangeAmount}");
            }
            else
            {
                Logger.Debug($"Not a called shot.");
            }
        }
    }
}
