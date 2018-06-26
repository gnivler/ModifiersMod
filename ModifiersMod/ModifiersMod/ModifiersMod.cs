using Harmony;
using System.Reflection;

namespace ModifiersMod
{
    class ModifiersMod
    {
        public static void Init(string directory, string settingsJSON)
        {
            var harmony = HarmonyInstance.Create("ca.gnivler.ModifiersMod");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

        }
    }
}