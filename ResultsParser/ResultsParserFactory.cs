using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.ResultsParser
{
    public static class ResultsParserFactory 
    {
        public static IResultsParser GetResultsParser(ResultsFileTypeEnum resultsFileType)
        {
            switch (resultsFileType)
            {
                case ResultsFileTypeEnum.CSV:
                    return new CSVParser();
                case ResultsFileTypeEnum.Json:
                    return new JsonParser();
                default:
                    return null;
            }
        }
    }
}
