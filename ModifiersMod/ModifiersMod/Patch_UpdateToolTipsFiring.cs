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
        public static void Prefix(CombatHUDWeaponSlot __instance, ICombatant target, CombatHUD ___HUD, CombatGameState ___Combat,
                                  List<string> ___BuffStrings, List<string> ___DebuffStrings)
        {
            Logger.Debug($"----- UpdateToolTipsFiring -----");

            #region list_members
            //var fields = instance.Fields();
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
            #endregion  

            Logger.Debug($"Setting flag");
            bool flag = ___HUD.SelectionHandler.ActiveState.SelectionType == SelectionType.FireMorale;
            Logger.Debug($"Setting attackModifier");
            var attackModifier = ___Combat.ToHit.GetMoraleAttackModifier(target, flag);

            Logger.Debug($"Trying to Traverse AddToolTipDetail");
            try
            {
                Traverse.Create(__instance).Method("AddToolTipDetail", new Type[] { typeof(string), typeof(int) },
                                                    new object[2] { "Offensive Push MOD", attackModifier });
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
            Logger.Debug($"have tried tooltip detail with {attackModifier}");

            ___BuffStrings.ForEach(x => Logger.Debug(x));
            ___DebuffStrings.ForEach(x => Logger.Debug(x));
        }
    }
}