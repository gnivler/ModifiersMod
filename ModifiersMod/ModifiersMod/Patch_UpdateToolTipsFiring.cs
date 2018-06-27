using BattleTech;
using BattleTech.UI;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModifiersMod
{
    [HarmonyPatch(typeof(CombatHUDWeaponSlot), "UpdateToolTipsFiring")]
    public static class Patch_EnableOffensivePushModifier
    {
        private static void Postfix(CombatHUDWeaponSlot __instance, ICombatant target)
        {
            var HUD = Traverse.Create(__instance).Field("HUD").GetValue<CombatHUD>();
            bool flag = HUD.SelectionHandler.ActiveState.SelectionType == SelectionType.FireMorale;
           
            var modifier = Traverse.Create(__instance).Field("modifier").GetValue<int>();
            var toHitCombatState = new ToHit(new CombatGameState());
            toHitCombatState.GetMoraleAttackModifier(target, flag);
            this.AddToolTipDetail(this.Combat.Constants.CombatUIConstants.MoraleAttackDescription.Name, this.modifier);
        }
    }
}