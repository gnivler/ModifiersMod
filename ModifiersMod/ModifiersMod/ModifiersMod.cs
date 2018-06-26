using Harmony;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace ModifiersMod
{

    public class Settings
    {
        public float ChangeAmount = 0f;
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
                // deserialize settings json onto our settings object
                settings = JsonConvert.DeserializeObject<Settings>(settingsJson);
            }
            catch (Exception)
            {
                // use default settings
                settings = new Settings();
            }
        }
    }
} 
