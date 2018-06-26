using BattleTech;
using Harmony;

namespace ModifiersMod
{
    
    [HarmonyPatch(typeof(ToHit))]
    [HarmonyPatch("CalledShotBonusMultiplier", PropertyMethod.Getter)]
    class Patch_CalledShotBonusModifierProperty
    {
        static void Postfix(ref float __result)
        {
            // ChangeAmount here would be a percentage of increase or decrease to the game-calculated float
            //
            Logger.LogLine("In Postfix with " + __result);
            __result *= ModifiersMod.settings.ChangeAmount;
        }
    }
}