using BattleTech;
using BattleTech.UI;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBS;

namespace ModifiersMod
{
    [HarmonyPatch(typeof(CombatHUDWeaponSlot), "UpdateToolTipsFiring")]
    public static class Patch_UpdateToolTipsFiring
    {
        public static bool Prefix()
        {
            return true;
        }

        public static void Postfix(CombatHUDWeaponSlot __instance, ICombatant target, CombatHUD ___HUD, CombatGameState ___Combat)
        {
            Logger.Debug($"----- UpdateToolTipsFiring -----");
            Logger.Debug($"Setting flag");
            bool flag = ___HUD.SelectionHandler.ActiveState.SelectionType == SelectionType.FireMorale;
            Logger.Debug($"Setting attackModifier");
            var attackModifier = ___Combat.ToHit.GetMoraleAttackModifier(target, flag);

            Logger.Debug($"Trying to apply modifier");
            try
            {
                Traverse.Create(__instance).Method("AddToolTipDetail", new object[] { "Offensive Push MOD", attackModifier });
                Logger.Debug($"Modifier applied: {attackModifier}");
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }
    }
}