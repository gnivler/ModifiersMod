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
    public static class Patch_EnableOffensivePushModifier
    {
        private static void Postfix(CombatHUDWeaponSlot __instance, ICombatant target)
        {
            var HUD = Traverse.Create(__instance).Field("HUD").GetValue<CombatHUD>();
            bool flag = HUD.SelectionHandler.ActiveState.SelectionType == SelectionType.FireMorale;
            
            var combatState = new CombatGameState();
            var toHit = new ToHit(combatState);
            
            // calling the vanilla method which is already patched here?!
            float modifier = toHit.GetMoraleAttackModifier(target, flag);

            //this.AddToolTipDetail(this.Combat.Constants.CombatUIConstants.MoraleAttackDescription.Name, this.modifier);

            __instance.AddToolTipDetail(__instance.Combat.Constants.CombatUIConstants.MoraleAttackDescription.Name, modifier);
        }
    }
}