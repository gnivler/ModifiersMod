using BattleTech.UI;
using Harmony;
using System;

namespace ModifiersMod
{
    [HarmonyPatch(typeof(CombatHUDWeaponSlot), "AddToolTipDetail")]
    public static class Patch_AddToolTipDetail
    {
        public static void Prefix(CombatHUDWeaponSlot __instance, string description, int modifier)
        {
            if (modifier != 0)
            {
                Logger.Debug($"----- AddToolTipDetail -----");
                if (__instance == null)
                {
                    Logger.Debug($"null, wtf");
                    return;
                }

                Logger.Debug($"description: {description}, modifier: {modifier}");
                var currentToolTipHoverElement = Traverse.Create(__instance).Field("ToolTipHoverElement")
                                                 .GetValue<CombatHUDTooltipHoverElement>();

                if (modifier < 0)
                {
                    Logger.Debug($"BuffStrings.Add {description} {modifier}");
                    currentToolTipHoverElement.BuffStrings.Add(string.Format("{0} {1:+0;-#}", description, modifier));
                }
                else if (modifier > 0)
                {
                    Logger.Debug($"DebuffStrings.Add {description} {modifier}");
                    currentToolTipHoverElement.DebuffStrings.Add(string.Format("{0} {1:+0;-#}", description, modifier));
                }

                if (__instance != null)
                {
                    Logger.Debug("----- Buff Strings     -----");
                    currentToolTipHoverElement.BuffStrings.ForEach(x => Logger.Debug(x));

                    Logger.Debug("----- Debuff Strings   -----");
                    currentToolTipHoverElement.DebuffStrings.ForEach(x => Logger.Debug(x));
                }
                else
                {
                    Logger.Debug("null again");
                }
            }
        }
    }
}
