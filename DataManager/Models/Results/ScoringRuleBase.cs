﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace iRLeagueManager.Models.Results
{
    [Serializable]
    [XmlInclude(typeof(StandardScoringRule))]
    public abstract class ScoringRuleBase
    {
        public ScoringRuleBase() { }

        /// <summary>
        /// Calculate Championship points for a single position
        /// </summary>
        /// <param name="place">Finish position</param>
        /// <returns>Championship points</returns>
        public abstract int GetSingleChampPoint(int place);

        /// <summary>
        /// Calculate Championship points.
        /// </summary>
        /// <param name="result">Result sheet for calculation - only "FinalResults" are taken into account</param>
        /// <returns>Dictionaray with |MemberId, ChampPoints|</returns>
        public abstract Dictionary<uint, int> GetChampPoints(ResultModel result);
    }
}
