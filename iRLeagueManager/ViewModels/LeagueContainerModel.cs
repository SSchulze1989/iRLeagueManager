// MIT License

// Copyright (c) 2020 Simon Schulze

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using iRLeagueManager.Models;
using iRLeagueManager.Data;
using iRLeagueManager;
using System.Windows;

namespace iRLeagueManager.ViewModels
{
    public abstract class LeagueContainerModel<TSource> : ContainerModelBase<TSource> where TSource : MappableModel, INotifyPropertyChanged, new()
    { 
        public virtual TSource Model { get => Source; set => SetSource(value); }

        public DateTime CreateDate => (Model?.CreatedOn).GetValueOrDefault();
        public readonly UserViewModel createUser = new UserViewModel();
        public UserViewModel CreateUser
        {
            get
            {
                if (createUser.UserId != Model?.CreatedByUserId)
                {
                    if (createUser.UpdateSource(LeagueContext.UserManager.GetUserModel(Model?.CreatedByUserId) ?? Models.User.UserModel.Template))
                    {
                        OnPropertyChanged();
                    }
                }
                return createUser;
            }
        }

        public DateTime LastEdit => (Model?.LastModifiedOn).GetValueOrDefault();
        private readonly UserViewModel lastEditUser = new UserViewModel();
        public UserViewModel LastEditUser
        {
            get
            {
                if (lastEditUser.UserId != Model?.LastModifiedByUserId)
                {
                    if (lastEditUser.UpdateSource(LeagueContext.UserManager.GetUserModel(Model?.LastModifiedByUserId) ?? Models.User.UserModel.Template))
                    {
                        OnPropertyChanged();
                    }
                }
                return lastEditUser;
            }
        }

        public bool HasEdits => LastEdit > CreateDate;

        public ICommand SaveChangesCmd { get; protected set; }

        public LeagueContainerModel() : this(new TSource())
        {
        }

        public LeagueContainerModel(TSource source) : base(source)
        {
            SaveChangesCmd = new RelayCommand(async o => await SaveChanges(), o => (Model?.ContainsChanges).GetValueOrDefault());
        }

        protected abstract TSource Template { get; }

        public TSource GetModelTemplate()
        {
            return Template;
        }

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

        public virtual async Task SaveChanges()
        {
            IsLoading = true;
            try
            {
                if (Model.ContainsChanges)
                {
                    IsLoading = true;
                    await LeagueContext.UpdateModelAsync(Source);
                    SaveChangesCmd.CanExecute(null);
                    OnPropertyChanged(nameof(SaveChangesCmd));
                }
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
