using iRLeagueManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.ViewModels
{
    public interface ISeasonPageViewModel
    {
        Task Load(SeasonModel season);
    }
}
