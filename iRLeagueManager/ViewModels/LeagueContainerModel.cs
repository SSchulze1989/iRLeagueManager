using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using iRLeagueManager.Models;
using iRLeagueManager.Data;
using iRLeagueManager;

namespace iRLeagueManager.ViewModels
{
    public abstract class LeagueContainerModel<TSource> : ContainerModelBase<TSource> where TSource : MappableModel, INotifyPropertyChanged
    { 
        public virtual TSource Model { get => Source; set => SetSource(value); }

        public LeagueContainerModel()
        {
            Model = Template;
        }

        public LeagueContainerModel(TSource source) : base(source)
        {
        }

        protected abstract TSource Template { get; }

        public virtual async Task Load(params long[] modelId)
        { 
            if (Model == null || !Model.ModelId.SequenceEqual(modelId))
            {
                Model = Template;
            }

            try
            {
                IsLoading = true;
                Model = await LeagueContext.GetModelAsync<TSource>(modelId);
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public virtual async Task Update()
        {
            if (Model == null || Model == Template)
                return;

            try
            {
                IsLoading = true;
                Model = await LeagueContext.UpdateModelAsync(Model);
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public virtual async void SaveChanges()
        {
            IsLoading = true;
            try
            {
                await LeagueContext.UpdateModelAsync(Source);
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
