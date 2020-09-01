using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using iRLeagueManager;
using iRLeagueManager.Data;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Results;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new LeagueContext();
            context.UserLoginAsync("Administrator", "administrator").Wait();

            var season = context.GetModelAsync<SeasonModel>(1).Result;
            var table = context.GetModelAsync<ScoringTableModel>(1).Result;
            var standings = context.GetModelAsync<StandingsModel>(table.ScoringTableId, table.Sessions.ElementAt(10).SessionId.GetValueOrDefault()).Result;

            Console.ReadKey();
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
