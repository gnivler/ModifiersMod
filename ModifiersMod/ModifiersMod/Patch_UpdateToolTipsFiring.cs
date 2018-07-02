using BattleTech;
using BattleTech.UI;
using Harmony;
using System;

namespace ModifiersMod
{
    [HarmonyPatch(typeof(CombatHUDWeaponSlot), "UpdateToolTipsFiring")]
    public static class Patch_UpdateToolTipsFiring
    {
        public static bool Prefix()
        {
            return true;
        }

        public static void Postfix(CombatHUDWeaponSlot __instance, ICombatant target, CombatHUD ___HUD, CombatGameState ___Combat,
                                   ref CombatHUDTooltipHoverElement ___ToolTipHoverElement)
        {
            Logger.Debug($"----- UpdateToolTipsFiring -----");
            Logger.Debug($"Setting flag");
            bool flag = ___HUD.SelectionHandler.ActiveState.SelectionType == SelectionType.FireMorale;
            Logger.Debug($"Setting attackModifier");
            
            // using fields directly has an effect
            //var attackModifier = ___Combat.ToHit.GetMoraleAttackModifier(target, flag);

            // anything I try here causes problems.  only one weapon lights up, the modifier doesn't apply below, pretty much breaks
            var attackModifier = Traverse.Create(__instance)
                                         .Method("GetMoraleAttackModifier",
                                         new Type[] { typeof(ICombatant), typeof(bool) },
                                         new object[] { target, flag })
                                         .GetValue<float>();

            Logger.Debug($"Trying to apply modifier");
            try
            {
                Traverse.Create(__instance).Method("AddToolTipDetail", new Type[] { typeof(string), typeof(int) }, new object[] { "OffPush MOD", attackModifier });
                Logger.Debug($"Modifier applied: {attackModifier}");
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
            Logger.Debug($"-----  Completed  Update   -----");
        }
    }
}