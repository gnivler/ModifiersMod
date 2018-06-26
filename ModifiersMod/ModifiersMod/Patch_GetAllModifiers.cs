using BattleTech;
using Harmony;
using UnityEngine;
using Newtonsoft.Json;
using System;

namespace ModifiersMod
{
    
    [HarmonyPatch(typeof(ToHit), "GetAllModifiers")]
    public static class Patch_GetAllModifiers
    {
        
        static bool Prefix()
        {
            return false;
        }

        static void Postfix(ToHit __instance, AbstractActor attacker, Weapon weapon, ICombatant target, Vector3 attackPosition, Vector3 targetPosition, LineOfFireLevel lofLevel, bool isCalledShot)
        {
            bool flag = lofLevel < LineOfFireLevel.LOFObstructed && weapon.IndirectFireCapable;
            float rangeModifier = __instance.GetRangeModifier(weapon, attackPosition, targetPosition);
            float coverModifier = __instance.GetCoverModifier(attacker, target, lofLevel);
            float selfSpeedModifier = __instance.GetSelfSpeedModifier(attacker);
            float selfSprintedModifier = __instance.GetSelfSprintedModifier(attacker);
            float selfArmMountedModifier = __instance.GetSelfArmMountedModifier(weapon);
            float stoodUpModifier = __instance.GetStoodUpModifier(attacker);
            float heightModifier = __instance.GetHeightModifier(attackPosition.y, targetPosition.y);
            float heatModifier = __instance.GetHeatModifier(attacker);
            float targetTerrainModifier = __instance.GetTargetTerrainModifier(target, targetPosition, false);
            float selfTerrainModifier = __instance.GetSelfTerrainModifier(attackPosition, false);
            float targetSpeedModifier = __instance.GetTargetSpeedModifier(target, weapon);
            float selfDamageModifier = __instance.GetSelfDamageModifier(attacker, weapon);
            float targetSizeModifier = __instance.GetTargetSizeModifier(target);
            float targetShutdownModifier = __instance.GetTargetShutdownModifier(target, false);
            float targetProneModifier = __instance.GetTargetProneModifier(target, false);
            float weaponAccuracyModifier = __instance.GetWeaponAccuracyModifier(attacker, weapon);
            float attackerAccuracyModifier = __instance.GetAttackerAccuracyModifier(attacker);
            float enemyEffectModifier = __instance.GetEnemyEffectModifier(target);
            float refireModifier = __instance.GetRefireModifier(weapon);
            float targetDirectFireModifier = __instance.GetTargetDirectFireModifier(target, flag);
            float indirectModifier = __instance.GetIndirectModifier(attacker, flag);
            float moraleAttackModifier = __instance.GetMoraleAttackModifier(target, isCalledShot);
            moraleAttackModifier += ModifiersMod.settings.ChangeAmount;
            float num = rangeModifier + coverModifier + selfSpeedModifier + selfSprintedModifier + selfArmMountedModifier + stoodUpModifier + heightModifier + heatModifier + targetTerrainModifier + selfTerrainModifier + targetSpeedModifier + selfDamageModifier + targetSizeModifier + targetShutdownModifier + targetProneModifier + weaponAccuracyModifier + attackerAccuracyModifier + enemyEffectModifier + refireModifier + targetDirectFireModifier + indirectModifier + moraleAttackModifier;
          
            CombatGameState combat = Traverse.Create(__instance).Property("combat").GetValue<CombatGameState>();
            if (num < 0f && !combat.Constants.ResolutionConstants.AllowTotalNegativeModifier)
            {
                num = 0f;
            }
        }
    }
}