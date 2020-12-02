// MIT License

// Copyright (c) 2020 Simon Schulze

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
    public class TeamsPageViewModel : ViewModelBase, IPageViewModel
    {
        private ObservableViewModelCollection<TeamViewModel, TeamModel> teams;
        public ObservableViewModelCollection<TeamViewModel, TeamModel> Teams { get => teams; private set => SetValue(ref teams, value); }

        private TeamViewModel selectedTeam;
        public TeamViewModel SelectedTeam { get => selectedTeam; set => SetValue(ref selectedTeam, value); }

        public ICommand AddTeamCmd { get; }
        public ICommand RemoveTeamCmd { get; }

        public TeamsPageViewModel()
        {
            Teams = new ObservableViewModelCollection<TeamViewModel, TeamModel>();
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

        public async Task<TeamModel> AddTeam(TeamModel team = null)
        {
            if (team == null)
                team = (new TeamViewModel()).GetModelTemplate();

            try
            {
                IsLoading = true;
                team = await LeagueContext.AddModelAsync(team);
                await Load();
                SelectedTeam = Teams.SingleOrDefault(x => x.TeamId == team.TeamId);
                return team;
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

        public override async Task Refresh()
        {
            LeagueContext.ModelManager.ForceExpireModels<TeamModel>();
            await Load();
            await base.Refresh();
        }
    }
}
