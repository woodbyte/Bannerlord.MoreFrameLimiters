using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Engine.Options;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.MoreFrameLimiters
{
    public class SubModule : MBSubModuleBase
    {
        GameState? _activeState = null;

        int GetFrameLimiter(GameState activeState)
        {
            if (Settings.Instance == null) return 0;

            int limiter = 0;

            if (activeState is InitialState)
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

        void ApplyFrameLimiter()
        {
            if (_activeState == null) return;

            int limiter = GetFrameLimiter(_activeState);
            if (limiter == 0) return;

            NativeOptions.SetConfig(NativeOptions.NativeOptionsType.FrameLimiter, limiter);
            NativeOptions.ApplyConfigChanges(false);
        }

        protected override void OnSubModuleLoad()
        {
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
}