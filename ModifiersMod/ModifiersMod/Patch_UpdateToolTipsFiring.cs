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
        public static void Prefix(CombatHUDWeaponSlot __instance, ICombatant target, CombatHUD ___HUD, CombatGameState ___Combat)
        {
            Logger.Debug($"----- UpdateToolTipsFiring -----");

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
        }
    }
}