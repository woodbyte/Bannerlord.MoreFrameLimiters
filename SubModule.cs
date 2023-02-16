using HarmonyLib;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Engine.Options;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ScreenSystem;

namespace Bannerlord.MoreFrameLimiters
{
    public class SubModule : MBSubModuleBase
    {
        static GameState? _activeState = null;
        static bool _gameWindowHasFocus = true;
        static int _lastForegroundLimiter;

        internal static void SetGameWindowHasFocus(bool hasFocus)
        {
            _gameWindowHasFocus = hasFocus;

            ApplyFrameLimiter();
        }

        static int GetFrameLimiter(GameState activeState)
        {
            if (Settings.Instance == null) return 0;

            int limiter = _lastForegroundLimiter;

            if (!_gameWindowHasFocus)
                limiter = Settings.Instance.BackgroundFrameLimiter;
            else if (activeState is InitialState)
                limiter = Settings.Instance.DefaultFrameLimiter;
            else if (activeState is MapState)
                limiter = Settings.Instance.MapToggle ? Settings.Instance.MapFrameLimiter : Settings.Instance.DefaultFrameLimiter;
            else if (activeState is MissionState missionState && missionState.CurrentMission != null)
            {
                if (missionState.MissionName.Contains("Tournament") || missionState.MissionName.Contains("Arena"))
                    limiter = Settings.Instance.ArenaToggle ? Settings.Instance.ArenaLimiter : Settings.Instance.DefaultFrameLimiter;
                else if (missionState.MissionName.Contains("Siege"))
                    limiter = Settings.Instance.SiegeToggle ? Settings.Instance.SiegeLimiter : Settings.Instance.DefaultFrameLimiter;
                else if (missionState.MissionName == "TownCenter" || missionState.MissionName == "Village")
                    limiter = Settings.Instance.VisitToggle ? Settings.Instance.VisitLimiter : Settings.Instance.DefaultFrameLimiter;
                else if (missionState.MissionName == "Indoor")
                    limiter = Settings.Instance.IndoorToggle ? Settings.Instance.IndoorLimiter : Settings.Instance.DefaultFrameLimiter;
                else
                    limiter = Settings.Instance.DefaultFrameLimiter;
            }

            return limiter;
        }

        static void ApplyFrameLimiter()
        {
            if (_activeState == null) return;

            int limiter = GetFrameLimiter(_activeState);
            if (limiter == 0) return;

            NativeOptions.SetConfig(NativeOptions.NativeOptionsType.FrameLimiter, limiter);
            NativeOptions.ApplyConfigChanges(false);

            if (_gameWindowHasFocus)
                _lastForegroundLimiter = limiter;
        }

        protected override void OnSubModuleLoad()
        {
            var harmony = new Harmony("bannerlord.moreframelimiters");
            harmony.PatchAll();

            NativeOptions.OnNativeOptionsApplied += ApplyFrameLimiter;
        }

        protected override void OnApplicationTick(float dt)
        {
            if (GameStateManager.Current != null)
            {
                var currentState = GameStateManager.Current.ActiveState;

                if (currentState != null && currentState != _activeState)
                {
                    _activeState = currentState;

                    ApplyFrameLimiter();
                }
            }
        }
    }

    [HarmonyPatch]
    internal class Patches
    {
        [HarmonyPatch(typeof(ScreenManager))]
        [HarmonyPatch(nameof(ScreenManager.OnGameWindowFocusChange))]
        class Patch01
        {
            internal static void Postfix(bool focusGained)
            {
                SubModule.SetGameWindowHasFocus(focusGained);
            }
        }
    }
}