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

            Logger.Debug($"----- AddToolTipDetail -----");
            //Logger.Debug($"description == {description}, modifier == {modifier}");

            try  // not catching exceptions, which are really occurring
            {
                var currentToolTipHoverElement = Traverse.Create(__instance).Field("ToolTipHoverElement")
                                                 .GetValue<CombatHUDTooltipHoverElement>();


                //var hoverElement = instance.Field("ToolTipHoverElement").GetValue<CombatHUDTooltipHoverElement>();


                if (modifier < 0)
                {
                    //Logger.Debug($"{modifier} < 0");
                    try
                    {
                        Logger.Debug($"BuffStrings.Add {description} {modifier}");
                        currentToolTipHoverElement.BuffStrings.Add(string.Format("{0} {1:+0;-#}", description, modifier));
                    }
                    catch (Exception e)
                    {
                        Logger.LogError(e);
                    }
                }
                else if (modifier > 0)
                {
                    //Logger.Debug($"{modifier} > 0");
                    try
                    {
                        Logger.Debug($"BuffStrings.Add {description} {modifier}");
                        currentToolTipHoverElement.DebuffStrings.Add(string.Format("{0} {1:+0;-#}", description, modifier));

                    }
                    catch (Exception e)
                    {
                        Logger.LogError(e);
                    }
                }
                else if (modifier == 0 && description == "OFFENSIVE PUSH")
                {
                    Logger.Debug($"{description} modifier {modifier} == 0");
                }

                Logger.Debug("----- Buff Strings   -----");
                currentToolTipHoverElement.BuffStrings.ForEach(x => Logger.Debug(x));

                Logger.Debug("----- Debuff Strings -----");
                currentToolTipHoverElement.DebuffStrings.ForEach(x => Logger.Debug(x));
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }
    }
}
