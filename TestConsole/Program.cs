using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using iRLeagueDatabase;
//using iRLeagueDatabase.Entities;
using iRLeagueManager;
using iRLeagueManager.Data;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Sessions;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //var dbContext = new TestDbContext();

            //var Session = new Session()
            //{
            //    Result = new Result()
            //};
            ////dbContext.Sessions.Add(Session);
            ////dbContext.SaveChanges();
            //Session = dbContext.Sessions.Find(2);

            var client = new LeagueContext();

            var session = client.GetModelAsync<SessionModel>(1).Result;

            Console.ReadKey();
        }

        static void NotifyDirect(object sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName + " changed. (direct)");
        }

        static void NotifyContainer(object sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName + " changed. (container)");
        }
    }
}
