using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using iRLeagueDatabase.DataTransfer.Statistics;
using iRLeagueManager;
using iRLeagueManager.Data;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Results;
using iRLeagueManager.ResultsParser;
using iRLeagueDatabase.Extensions;
using Microsoft.Win32;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //var fileName = @"C:\Users\simon\Documents\iracing-result-34832620.json";

            //Stream stream = null;

            //var parserService = ResultsParserFactory.GetResultsParser(ResultsFileTypeEnum.Json);

            //try
            //{
            //    stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
            //    parserService.ReadStreamAsync(new StreamReader(stream, Encoding.Default)).Wait();
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.ToString(), "Error parsing result File", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}
            //finally
            //{
            //    stream?.Dispose();
            //}

            //var newMembers = parserService.GetNewMemberList();
            //var result = parserService.GetResultRows();

            // Test Statistics loading from API
            var context = new LeagueContext();
            context.SetLeagueName("SkippyCup");
            context.UserLoginAsync("simonschulze", "ollgass").Wait();

            var statsSets = context.ModelDatabase.GetAsync<SeasonStatisticSetDTO>(null).Result;
            var stats = context.ModelDatabase.GetAsync<DriverStatisticDTO>(new long[][] { new long[] { statsSets.First().Id } }).Result.FirstOrDefault();

            //var importStat = new ImportedStatisticSetDTO()
            //{
            //    Description = "Test import",
            //    ImportSource = "Season statistic set",
            //    FirstDate = DateTime.Now,
            //    LastDate = DateTime.Now
            //};
            //importStat = context.ModelDatabase.PostAsync(new ImportedStatisticSetDTO[] { importStat }).Result.FirstOrDefault();

            var importStat = context.ModelDatabase.GetAsync<ImportedStatisticSetDTO>(new long[][] { new long[] { 6 } });

            stats.StatisticSetId = 6;
            stats.DriverStatisticRows.ForEach(x => x.StatisticSetId = 0);
            stats = context.ModelDatabase.PostAsync(new DriverStatisticDTO[] { stats }).Result.FirstOrDefault();

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
