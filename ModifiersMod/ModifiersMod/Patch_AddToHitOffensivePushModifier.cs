using BattleTech;
using Harmony;
using System;

namespace ModifiersMod
{
    // credit to FlukeyFiddler's original example
    [HarmonyPatch(typeof(ToHit), "GetMoraleAttackModifier")]
    public static class EnableOffensivePushModifier
    {
        public static void Prefix(ToHit __instance, ICombatant target, bool isMoraleAttack, ref float __result)
        {
            float offensivePushModifier = 0f;
            CombatGameState combat = Traverse.Create(__instance).Field("combat").GetValue<CombatGameState>();

            offensivePushModifier = combat.Constants.ToHit.ToHitOffensivePush;
            float modifiedToHitModifier = offensivePushModifier + ModifiersMod.settings.ChangeAmount;
            __result = modifiedToHitModifier;
            
            // not used yet this is a new thing on StatCollection
            //float calculatedModifier = offensivePushModifier + target.StatCollection.GetValue<float>("ToHitOffensivePushModifier");  /// 
            //__result = (!isMoraleAttack) ? 0f : offensivePushModifier + target.StatCollection.GetValue<float>("ToHitOffensivePushModifier");

            if (isMoraleAttack)
            {
                Logger.Debug($"Constants.ToHit.ToHitOffensivePush == {offensivePushModifier}, settings.ChangeAmount == {ModifiersMod.settings.ChangeAmount}");
                Logger.Debug($"Offensive Push modifier should be {modifiedToHitModifier}");
            }
            else
            {
                //Logger.Debug($"Not a called shot.");
            }
        }
    }
}
