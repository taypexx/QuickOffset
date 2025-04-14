using MelonLoader;
using Tomlet;
using Tomlet.Attributes;

namespace CompetitivePlay
{
    internal static class Save
    {
        internal static Config Config = new();

        public static void Load()
        {
            if (!File.Exists(Path.Combine("UserData", "QuickOffset.cfg")))
            {
                var defaultConfig = TomletMain.TomlStringFrom(Config);
                File.WriteAllText(Path.Combine("UserData", "QuickOffset.cfg"), defaultConfig);
            }

            var data = File.ReadAllText(Path.Combine("UserData", "QuickOffset.cfg"));
            Config = TomletMain.To<Config>(data);
        }

        public static void Save_()
        {
            File.WriteAllText(Path.Combine("UserData", "QuickOffset.cfg"), TomletMain.TomlStringFrom(Config));
        }
    }

    public class Config
    {
        [TomlPrecedingComment("Hotkey for opening the offset list")]
        internal string OpenHotkey { get; set; } = "P";

        [TomlPrecedingComment("The list of all offsets")]
        internal List<float> Offsets { get; set; } = new() { 0f };

    }
}