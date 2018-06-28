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
            var instance = Traverse.Create(__instance);
            var HUD = instance.Field("HUD").GetValue<CombatHUD>();
            var combat = instance.Field("Combat").GetValue<CombatGameState>();

            bool flag = HUD.SelectionHandler.ActiveState.SelectionType == SelectionType.FireMorale;
            var attackModifier = combat.ToHit.GetMoraleAttackModifier(target, flag);

            //attackModifier =  Patch_AddToolTipDetail.Postfix(instance.GetValue<CombatHUDWeaponSlot>(), "Our Value", attackModifier);
            instance.Method("AddToolTipDetail", "OurValue", attackModifier);
            Logger.LogLine($"in UpdateToolTipsFiring.Postfix, attackModifier == {attackModifier}");
        }
    }
}