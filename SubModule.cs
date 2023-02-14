using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Engine.Options;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.MoreFrameLimiters
{
    public class SubModule : MBSubModuleBase
    {
        GameState? _activeGameState = null;
        int? _newFrameLimiter = null;

        void ConfigureFrameLimiter(GameState activeState)
        {
            if (Settings.Instance == null) return;

            if (activeState is InitialState)
                _newFrameLimiter = Settings.Instance.DefaultFrameLimiter;
            else if (activeState is MapState)
                _newFrameLimiter = Settings.Instance.MapToggle ? Settings.Instance.MapFrameLimiter : Settings.Instance.DefaultFrameLimiter;
            else if (activeState is MissionState missionState)
            {
                if (missionState.CurrentMission == null) return;

                //"TournamentFight" "ArenaPracticeFight" "ArenaDuelMission"
                //"SiegeMissionWithDeployment"
                //"TownCenter"
                //"Village"
                //"Indoor"

                if (missionState.MissionName.Contains("Tournament") || missionState.MissionName.Contains("Arena"))
                    _newFrameLimiter = Settings.Instance.ArenaToggle ? Settings.Instance.ArenaLimiter : Settings.Instance.DefaultFrameLimiter;
                else if (missionState.MissionName.Contains("Siege"))
                    _newFrameLimiter = Settings.Instance.SiegeToggle ? Settings.Instance.SiegeLimiter : Settings.Instance.DefaultFrameLimiter;
                else if (missionState.MissionName == "TownCenter" || missionState.MissionName == "Village")
                    _newFrameLimiter = Settings.Instance.VisitToggle ? Settings.Instance.VisitLimiter : Settings.Instance.DefaultFrameLimiter;
                else if (missionState.MissionName == "Indoor")
                    _newFrameLimiter = Settings.Instance.IndoorToggle ? Settings.Instance.IndoorLimiter : Settings.Instance.DefaultFrameLimiter;
                else
                    _newFrameLimiter = Settings.Instance.DefaultFrameLimiter;
            }
        }

        protected override void OnApplicationTick(float dt)
        {
            if (_newFrameLimiter != null)
            {
                if (_newFrameLimiter.Value != NativeOptions.GetConfig(NativeOptions.NativeOptionsType.FrameLimiter))
                    NativeOptions.SetConfig(NativeOptions.NativeOptionsType.FrameLimiter, _newFrameLimiter.Value);

                _newFrameLimiter = null;
            }

            if (GameStateManager.Current == null) return;
            if (GameStateManager.Current.ActiveState != _activeGameState)
            {
                _activeGameState = GameStateManager.Current.ActiveState;

                ConfigureFrameLimiter(_activeGameState);
            }
        }

        public override void OnInitialState()
        {
            if (Settings.Instance == null) return;

            Settings.Instance.PropertyChanged += Instance_PropertyChanged;
        }

        private void Instance_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_activeGameState == null) return;

            ConfigureFrameLimiter(_activeGameState);
        }
    }
}