﻿using System;
using System.Collections.Generic;
using BattleTech;
using BattleTech.UI;
using Harmony;

namespace ModifiersMod
{
    class Patch
    {
        // credit to FlukeyFiddler's original example
        [HarmonyPatch(typeof(ToHit), "GetMoraleAttackModifier")]
        public static class EnableOffensivePushModifier
        {
            public static bool Prefix(ToHit __instance, ICombatant target, bool isMoraleAttack,
                                      ref float __result, CombatGameState ___combat)
            {
                // if enemy return original?  and don't log (this fires during player's turn as well
                // and so doesn't run the method
                //if (___combat.LocalPlayerTeam.IsLocalPlayer)
                //{
                //    __result = 0f;
                //    Logger.Debug($"{(___combat.LocalPlayerTeam.IsLocalPlayer)}");
                //    return false;
                //}
                Logger.Debug($"----- Start GetMoraleAttackModifier ------");

                target.StatCollection.GetValue<float>("ToHitOffensivePushModifier");
                StatCollection collection = Traverse.Create(target).Property("StatCollection").GetValue<StatCollection>();
                float modifier = collection.GetValue<float>("ToHitOffensivePushModifier");
                //Statistic stat = collection.GetStatistic("ToHitOffensivePushModifier");
                //StatCollection.StatOperation operation = stat.


                //float modifier= 0;//= ToHitOffensivePushModifier.Value<float>();

                // need to get a list of effects then iterate through it to aggregate modifier
                // from multiple sources of the effect
                //
                //var effectlist = Traverse.Create(___combat).Field("effects").GetValue<List<Effect>>();            
                //foreach (var effect in effectlist)
                //{
                //    if (effect.EffectData.Description.Id == "StatusEffect - OP300") {
                //        num += effect.EffectData.Description
                //}
                //
                //var otherModifier = target.StatCollection.GetValue<float>("ToHitOffensivePushModifier");
                //
                //CombatGameState combat = Traverse.Create(__instance).Field("combat").GetValue<CombatGameState>();
                //num += combat.Constants.ToHit.ToHitOffensivePush;                                
                __result = (!isMoraleAttack) ? 0f : modifier;
                Logger.Debug($"Offensive Push modifier: {modifier}");
                Logger.Debug($"----- End GetMoraleAttackModifier --------");

                return false;
            }
        }

        public static class Patch_UpdateToolTipsFiring
        {
            // can't remember what the fuck I was trying to do here...
            public static bool Prefix(CombatHUDWeaponSlot __instance, ICombatant target, CombatHUD ___HUD)
            {
                Logger.Debug($"----- Start UpdateToolTipsFiring ---------");
                bool isMoraleAttack = ___HUD.SelectionHandler.ActiveState.SelectionType == SelectionType.FireMorale;
                Logger.Debug($"Called shot: {isMoraleAttack}");
                Logger.Debug($"Setting attackModifier");

                var combat = Traverse.Create(__instance).Field("Combat").GetValue<CombatGameState>();
                var attackModifier = combat.ToHit.GetMoraleAttackModifier(target, isMoraleAttack);

                Logger.Debug($"Trying to apply modifier");
                try
                {
                    Traverse.Create(__instance).Method("AddToolTipDetail",
                                               new Type[] { typeof(string), typeof(int) },
                                               new object[] { "OffPush MOD", attackModifier });

                    Logger.Debug($"Modifier applied: {attackModifier}");
                    Logger.Debug("Buffs:");
                    __instance.ToolTipHoverElement.BuffStrings.ForEach(x => Logger.Debug(x));
                    Logger.Debug("Debuffs:");
                    __instance.ToolTipHoverElement.DebuffStrings.ForEach(x => Logger.Debug(x));
                }
                catch (Exception e)
                {
                    Logger.LogError(e);
                }

                Logger.Debug($"----- End UpdateToolTipsFiring -----------{Environment.NewLine}");
                return false;
            }
        }

        [HarmonyPatch(typeof(CombatHUDWeaponSlot), "AddToolTipDetail")]
        public static class Patch_AddToolTipDetail
        {
            public static bool Prefix(CombatHUDWeaponSlot __instance, string description, int modifier)
            {
                if (modifier != 0)
                {
                    Logger.Debug($"----- Start AddToolTipDetail -------------");
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
                return false;
            }
        }
    }
}
