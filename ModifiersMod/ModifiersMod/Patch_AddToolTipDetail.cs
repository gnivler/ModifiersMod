using BattleTech.UI;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModifiersMod
{
    [HarmonyPatch(typeof(CombatHUDWeaponSlot), "AddToolTipDetail")]
    public static class Patch_AddToolTipDetail
    {
        public static bool Prefix()
        {
            return false;
        }

        public static void Postfix(CombatHUDWeaponSlot __instance, string description, int modifier)
        {
            //Logger.Debug($"in AddToolTipDetail.Postfix, description == {description}, modifier == {modifier}");

            var instance = Traverse.Create(__instance);
            var hoverElement = instance.Field("ToolTipHoverElement").GetValue<CombatHUDTooltipHoverElement>();
            
            if (modifier < 0)
            {
                //Logger.Debug($"{modifier} < 0");
                hoverElement.BuffStrings.Add(string.Format("{0} {1:+0;-#}", description, modifier));
            }
            else if (modifier > 0)
            {
                //Logger.Debug($"{modifier} > 0");
                hoverElement.DebuffStrings.Add(string.Format("{0} {1:+0;-#}", description, modifier));
            }
            else if (modifier == 0)
            {
                //Logger.Debug($"{modifier} == 0");
            }
        }
    }
}
