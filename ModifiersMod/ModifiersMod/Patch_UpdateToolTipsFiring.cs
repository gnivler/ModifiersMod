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

        public static void Postfix(CombatHUDWeaponSlot __instance, ICombatant target)
        {
            // THANK YOU JO!
            var _this = Traverse.Create(__instance);
            var HUD = _this.Field("HUD").GetValue<CombatHUD>();
            var combat = _this.Field("Combat").GetValue<CombatGameState>();
  
            bool flag = HUD.SelectionHandler.ActiveState.SelectionType == SelectionType.FireMorale;


            var combatState = new CombatGameState();
            var toHit = new ToHit(combatState);
            
            // calling the vanilla method which is already patched here?!
            //float modifier = toHit.GetMoraleAttackModifier(target, flag);

            var attackModifier = combat.ToHit.GetMoraleAttackModifier(target, flag);

            //this.AddToolTipDetail(this.Combat.Constants.CombatUIConstants.MoraleAttackDescription.Name, this.modifier);

            _this.Method("AddToolTipDetail", combat.Constants.CombatUIConstants.MoraleAttackDescription.Name, attackModifier);
        }
    }
}