using BattleTech;
using Harmony;

namespace ModifiersMod
{
    // credit to FlukeyFiddler's original example
    [HarmonyPatch(typeof(ToHit), "GetMoraleAttackModifier")]
    public static class Patch_GetMoraleAttackModifier
    {
        public static bool Prefix(bool isMoraleAttack, CombatGameState ___combat, ref float __result)
        {
            #region cruft
            // not used yet this is a new thing on StatCollection (code may be totally wrong)
            //float calculatedModifier = offensivePushModifier + target.StatCollection.GetValue<float>("ToHitOffensivePushModifier");  /// 
            //__result = (!isMoraleAttack) ? 0f : offensivePushModifier + target.StatCollection.GetValue<float>("ToHitOffensivePushModifier");
            #endregion

            if (isMoraleAttack)
            {
                //Logger.Debug($"ToHitOffensivePush: {offensivePushModifier}, ChangeAmount: {ModifiersMod.settings.ChangeAmount}");
                Logger.Debug($"Offensive Push modifier should be" +
                             $" {___combat.Constants.ToHit.ToHitOffensivePush + ModifiersMod.settings.ChangeAmount}");
                __result = ___combat.Constants.ToHit.ToHitOffensivePush + ModifiersMod.settings.ChangeAmount;
            }
            else
            {
                __result = 0f;
                Logger.Debug($"Not a called shot.");
            }
            return false;
        }
    }
}
