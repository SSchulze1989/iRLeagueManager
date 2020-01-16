using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel;
using System.Runtime.CompilerServices;

//using iRLeagueDatabase;
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
            //var dbContext = new LeagueDbContext();

            //var season = new SeasonEntity()
            //{
            //    SeasonName = "TestSeason"
            //};

            //dbContext.Seasons.Add(season);
            //dbContext.SaveChanges();

            Derived derived = new Derived()
            {
                Property = "Property",
                DerivedProperty = "DerivedProperty"
            };
            derived.PropertyChanged += NotifyDirect;

            var container = new Container()
            {
                Derived = derived
            };
            container.PropertyChanged += NotifyContainer;

            derived.Property = "change";

            //var context = new LeagueContext();

            //var season = context.GetModelAsync<SeasonModel>(1).Result;
            //var schedule = new ScheduleModel();

            //season.Schedules.Add(schedule);
            //context.UpdateModelAsync(schedule).Wait();

            //context.UpdateModelAsync(season).Wait();

            //Console.WriteLine(season.SeasonName);
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
