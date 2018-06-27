using Harmony;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace ModifiersMod
{
    public class Settings
    {
        // ChangeAmount is a factor and negative values equate to better shot chance
        public float ChangeAmount;
    }

    public static class ModifiersMod
    {
        public static Settings settings;
        public static string ModDirectory;
        public static void Init(string modDirectory, string settingsJson)
        {
            var harmony = HarmonyInstance.Create("ca.gnivler.ModifiersMod");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            ModDirectory = modDirectory;
            try
            {
                settings = JsonConvert.DeserializeObject<Settings>(settingsJson);
                //Logger.LogLine($"Deserialized {settings}");
            }
            catch (Exception)
            {
                settings = new Settings();
                //Logger.LogError(e);
            }
        }
    }
} 
