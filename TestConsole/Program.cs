using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using iRLeagueManager;
using iRLeagueManager.Data;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Results;
using iRLeagueManager.ResultsParser;
using Microsoft.Win32;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = @"C:\Users\simon\Documents\iracing-result-34832620.json";

            Stream stream = null;

            var parserService = ResultsParserFactory.GetResultsParser(ResultsFileTypeEnum.Json);
            IEnumerable<Dictionary<string, string>> lines = null;

            try
            {
                stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
                parserService.ReadStreamAsync(new StreamReader(stream, Encoding.Default)).Wait();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error parsing result File", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            finally
            {
                stream?.Dispose();
            }

            var newMembers = parserService.GetNewMemberList();
            var result = parserService.GetResultRows();

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
