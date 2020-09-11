﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueDatabase.DataTransfer;

namespace iRLeagueManager.Data
{
    public interface IActionProvider<TModelData, TKey> : IDisposable
    {
        Task CalculateScoredResultsAsync(TKey sessionId);
    }

    public interface IActionProvider : IActionProvider<MappableDTO, long> { }
}
