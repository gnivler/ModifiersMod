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

        public static void Postfix(CombatHUDWeaponSlot __instance, ICombatant target, CombatHUD ___HUD)
        {
            Logger.Debug($"----- Start UpdateToolTipsFiring -----");
            bool isMoraleAttack = ___HUD.SelectionHandler.ActiveState.SelectionType == SelectionType.FireMorale;
            Logger.Debug($"Called shot: {isMoraleAttack}");
            Logger.Debug($"Setting attackModifier");

            var combat = Traverse.Create(__instance).Field("Combat").GetValue<CombatGameState>();
            var attackModifier = combat.ToHit.GetMoraleAttackModifier(target, isMoraleAttack);


            Logger.Debug($"Trying to apply modifier");
            try
            {
                Traverse.Create(__instance).Method("AddToolTipDetail", new Type[] { typeof(string), typeof(int) }, new object[] { "OffPush MOD", attackModifier });
                Logger.Debug($"Modifier applied: {attackModifier}");

                Logger.Debug("Buffs:");
                __instance.ToolTipHoverElement.BuffStrings.ForEach(x => Logger.Debug(x));
                Logger.Debug("Debuffs:");
                __instance.ToolTipHoverElement.DebuffStrings.ForEach(x => Logger.Debug(x));

            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
            Logger.Debug($"----- End UpdateToolTipsFiring -------{Environment.NewLine}");
        }
    }
}