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
            var fields = instance.Fields();
            Logger.Debug($"----- UpdateToolTipsFiring -----");
            //Logger.Debug($"----- Fields -----");
            //fields.ForEach(x => Logger.Debug(x));

            //var methods = instance.Fields();
            //Logger.Debug($"----- Methods -----");
            //methods.ForEach(x => Logger.Debug(x));
            //
            //var buffs = __instance.ToolTipHoverElement.BuffStrings;
            //Logger.Debug($"----- Buffs   -----");
            //buffs.ForEach(x => Logger.Debug(x));
            //
            //var debuffs = __instance.ToolTipHoverElement.DebuffStrings;
            //Logger.Debug($"----- Debuffs -----");
            //debuffs.ForEach(x => Logger.Debug(x));

            var HUD = instance.Field("HUD").GetValue<CombatHUD>();
            var combat = instance.Field("Combat").GetValue<CombatGameState>();
            bool flag = HUD.SelectionHandler.ActiveState.SelectionType == SelectionType.FireMorale;
            var attackModifier = combat.ToHit.GetMoraleAttackModifier(target, flag);

            if (instance.Method("AddToolTipDetail").MethodExists())
            {
                Logger.Debug($"method exists");
                Logger.Debug($"{instance.Field("BuffStrings").GetValue<List<string>>().ToArray()}");
                Logger.Debug($"{instance.Field("DebuffStrings").GetValue<List<string>>().ToArray()}");
            }
            else
            {
                Logger.Debug($"method does not exist");
            }

            //attackModifier =  Patch_AddToolTipDetail.Postfix(instance.GetValue<CombatHUDWeaponSlot>(), "Our Value", attackModifier);
            instance.Method("AddToolTipDetail", new Type[] { typeof(string), typeof(int) }, new object[2] { "OurValue", attackModifier});
            Logger.Debug($"have tried tooltip detail with {attackModifier}");
            Logger.Debug($"{instance.Field("BuffStrings").GetValue<List<string>>().ToArray()}");
            Logger.Debug($"{instance.Field("DebuffStrings").GetValue<List<string>>().ToArray()}");

            //Logger.Debug($"in UpdateToolTipsFiring.Postfix, attackModifier == {attackModifier}");
        }
    }
}