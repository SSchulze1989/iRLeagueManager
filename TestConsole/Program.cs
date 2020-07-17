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
            //var dbClient = new LeagueDBServiceClient();
            //dbClient.SetDatabaseName("TestDatabase");

            //byte[] pw = Encoding.UTF8.GetBytes("TestPasswort");

            ////var test = dbClient.AuthenticateUserAsync("Master", pw, "TestDatabase");
            ////test.Wait();

            ////var entry = test.Result;

            //Console.WriteLine(entry);

            ////var msg = new GETItemsRequestMessage()
            ////{
            ////    databaseName = "TestDatabase",
            ////    userName = "testuser",
            ////    password = "1234",
            ////    requestItemIds = new long[][] { new long[] { 1 } },
            ////    requestItemType = typeof(ScoringDataDTO).Name,
            ////    requestResponse = true
            ////};

            ////var response = dbClient.DatabaseGET(msg);
            ////var scoringDto = response.items.First() as ScoringDataDTO;


            //Console.ReadKey();
        }
    }
}
