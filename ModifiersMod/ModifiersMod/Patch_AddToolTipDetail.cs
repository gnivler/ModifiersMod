using BattleTech.UI;
using Harmony;
using System;

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
            if (modifier != 0)
            {
                Logger.Debug($"----- Start AddToolTipDetail -------------");
                if (__instance == null)
                {
                    Logger.Debug($"null, wtf");
                    return;
                }

                Logger.Debug($"Call: {description}, modifier: {modifier}");
                var currentToolTipHoverElement = Traverse.Create(__instance).Field("ToolTipHoverElement")
                                                 .GetValue<CombatHUDTooltipHoverElement>();

                var buffs = currentToolTipHoverElement.BuffStrings;
                var debuffs = currentToolTipHoverElement.DebuffStrings;
                var effectString = string.Format("{0} {1:+0;-#}", description, modifier);

                if (modifier < 0)
                {
                    Logger.Debug($">>Buffed: {effectString}");
                    buffs.Add(effectString);
                }
                else if (modifier > 0)
                {
                    Logger.Debug($">>Debuffed: {effectString}");
                    debuffs.Add(effectString);
                }

                if (buffs.Count > 0)
                {
                    Logger.Debug("----- Buff Strings -----------------------");
                    buffs.ForEach(x => Logger.Debug(x));
                }

                if (debuffs.Count > 0)
                {
                    Logger.Debug("----- Debuff Strings ---------------------");
                    debuffs.ForEach(x => Logger.Debug(x));
                }
                Logger.Debug($"----- End AddToolTipDetail ---------------{Environment.NewLine}");
            }
        }
    }
}
