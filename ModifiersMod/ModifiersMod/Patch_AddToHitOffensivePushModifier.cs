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


            var calculatedModifier = offensivePushModifier + target.StatCollection.GetValue<float>("ToHitOffensivePushModifier");
            __result = (!isMoraleAttack) ? 0f : offensivePushModifier + target.StatCollection.GetValue<float>("ToHitOffensivePushModifier");

            if (isMoraleAttack)
            {

                Logger.LogLine($"Constants.ToHit.ToHitOffensivePush == {offensivePushModifier}, settings.ChangeAmount == {ModifiersMod.settings.ChangeAmount}");
                Logger.LogLine($"Offensive Push modifier should be {modifiedToHitModifier}.{Environment.NewLine}");
            }
            else
            {
                Logger.LogLine($"Not a called shot.{Environment.NewLine}");

            }
        }
    }
}
