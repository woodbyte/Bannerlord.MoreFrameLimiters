using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace Bannerlord.MoreFrameLimiters
{
    internal sealed class Settings : AttributeGlobalSettings<Settings>
    {
        public override string Id => "MoreFrameLimiters";
        public override string FolderName => "MoreFrameLimiters";
        public override string DisplayName => $"More Frame Limiters {typeof(Settings).Assembly.GetName().Version.ToString(3)}";
        public override string FormatType => "json";

        [SettingPropertyInteger("Default Frame Limiter", 30, 360, RequireRestart = false, Order = 1)]
        [SettingPropertyGroup("General", GroupOrder = 1)]
        public int DefaultFrameLimiter { get; set; } = 60;

        [SettingPropertyInteger("Background Frame Limiter", 30, 360, RequireRestart = false, Order = 2)]
        [SettingPropertyGroup("General", GroupOrder = 1)]
        public int BackgroundFrameLimiter { get; set; } = 30;

        [SettingPropertyBool("Map Toggle", RequireRestart = false, Order = 1, IsToggle = true)]
        [SettingPropertyGroup("Campaign Map", GroupOrder = 2)]
        public bool MapToggle { get; set; } = true;

        [SettingPropertyInteger("Map Frame Limiter", 30, 360, RequireRestart = false, Order = 2)]
        [SettingPropertyGroup("Campaign Map", GroupOrder = 2)]
        public int MapFrameLimiter { get; set; } = 90;

        [SettingPropertyBool("Siege Toggle", RequireRestart = false, Order = 1, IsToggle = true)]
        [SettingPropertyGroup("Siege Battles", GroupOrder = 3)]
        public bool SiegeToggle { get; set; } = false;

        [SettingPropertyInteger("Siege Frame Limiter", 30, 360, RequireRestart = false, Order = 2)]
        [SettingPropertyGroup("Siege Battles", GroupOrder = 3)]
        public int SiegeLimiter { get; set; } = 60;

        [SettingPropertyBool("Arena Toggle", RequireRestart = false, Order = 1, IsToggle = true)]
        [SettingPropertyGroup("Arena Fights", GroupOrder = 4)]
        public bool ArenaToggle { get; set; } = true;

        [SettingPropertyInteger("Arena Frame Limiter", 30, 360, RequireRestart = false, Order = 2)]
        [SettingPropertyGroup("Arena Fights", GroupOrder = 4)]
        public int ArenaLimiter { get; set; } = 120;

        [SettingPropertyBool("Visit Toggle", RequireRestart = false, Order = 1, IsToggle = true)]
        [SettingPropertyGroup("Towns, Castles and Villages", GroupOrder = 5)]
        public bool VisitToggle { get; set; } = false;

        [SettingPropertyInteger("Visit Frame Limiter", 30, 360, RequireRestart = false, Order = 2)]
        [SettingPropertyGroup("Towns, Castles and Villages", GroupOrder = 5)]
        public int VisitLimiter { get; set; } = 60;

        [SettingPropertyBool("Indoor Toggle", RequireRestart = false, Order = 1, IsToggle = true)]
        [SettingPropertyGroup("Indoor Locations", GroupOrder = 6)]
        public bool IndoorToggle { get; set; } = true;

        [SettingPropertyInteger("Indoor Frame Limiter", 30, 360, RequireRestart = false, Order = 2)]
        [SettingPropertyGroup("Indoor Locations", GroupOrder = 6)]
        public int IndoorLimiter { get; set; } = 120;
    }
}
