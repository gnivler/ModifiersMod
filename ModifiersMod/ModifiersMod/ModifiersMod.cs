using Harmony;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace ModifiersMod
{
    public class Settings
    {
        // ChangeAmount is added or subtracted from the modifiers total, with negative being better shot chance
        public float ChangeAmount;
        public bool Debug = false;
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
                Logger.Debug($"Deserialized {settings}");
            }
            catch (Exception e)
            {
                settings = new Settings();
                Logger.LogError(e);
            }
        }
    }
} 
