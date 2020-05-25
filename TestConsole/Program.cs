using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager;
using iRLeagueManager.Data;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Results;
using iRLeagueManager.LeagueDBServiceRef;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //var dbClient = new DbLeagueServiceClient();

            //var msg = new GETItemsRequestMessage()
            //{
            //    databaseName = "TestDatabase",
            //    userName = "testuser",
            //    password = "1234",
            //    requestItemIds = new long[][] { new long[] { 1 } },
            //    requestItemType = typeof(ScoringDataDTO).Name,
            //    requestResponse = true
            //};

            //var response = dbClient.DatabaseGET(msg);
            //var scoringDto = response.items.First() as ScoringDataDTO;

            var pair = new MyKeyValuePair<int, int>(1, 2);
            Console.WriteLine(pair.Key + " - " + pair.Value);
            MyKeyValuePair pair2 = pair;
            Console.WriteLine((int)pair2.Key + " - " + (int)pair2.Value);

            Console.ReadKey();
        }
    }
}
