using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using iRLeagueManager.ViewModels.Collections;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Members;

namespace iRLeagueManager.ViewModels
{
    public class TeamsPageViewModel : ViewModelBase
    {
        private ObservableModelCollection<TeamViewModel, TeamModel> teams;
        public ObservableModelCollection<TeamViewModel, TeamModel> Teams { get => teams; private set => SetValue(ref teams, value); }

        private TeamViewModel selectedTeam;
        public TeamViewModel SelectedTeam { get => selectedTeam; set => SetValue(ref selectedTeam, value); }

        public ICommand AddTeamCmd { get; }
        public ICommand RemoveTeamCmd { get; }

        public TeamsPageViewModel()
        {
            Teams = new ObservableModelCollection<TeamViewModel, TeamModel>();
            AddTeamCmd = new RelayCommand(async o => await AddTeam(), o => true);
            RemoveTeamCmd = new RelayCommand(async o => await RemoveTeam(o as TeamModel), o => o != null);
        }

        public async Task Load()
        {
            try
            {
                IsLoading = true;
                var teamModels = new ObservableCollection<TeamModel>(await LeagueContext.GetModelsAsync<TeamModel>());
                Teams.UpdateSource(teamModels);
                if (SelectedTeam == null)
                    SelectedTeam = Teams.FirstOrDefault();
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task<TeamModel> AddTeam()
        {
            try
            {
                IsLoading = true;
                var newTeam = new TeamModel()
                {
                    Name = "New Team"
                };
                newTeam = await LeagueContext.AddModelAsync(newTeam);
                await Load();
                SelectedTeam = Teams.SingleOrDefault(x => x.TeamId == newTeam.TeamId);
                return newTeam;
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
            return null;
        }

        public async Task RemoveTeam(TeamModel team)
        {
            try
            {
                IsLoading = true;
                await LeagueContext.DeleteModelAsync<TeamModel>(team.TeamId);
                await Load();
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
