﻿using iRLeagueManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AutoMapper;
using AutoMapper.Collection;
using AutoMapper.EquivalencyExpression;
using AutoMapper.Configuration;
using AutoMapper.EntityFramework;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Members;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Reviews;
using iRLeagueManager.Enums;
using iRLeagueManager.Models.Database;
using iRLeagueManager.LeagueDBServiceRef;
using iRLeagueManager.Locations;

namespace iRLeagueManager.Data
{
    public class LeagueContext  //: ILeagueContext
    {
        private ModelMapperProfile MapperProfile { get; }
        private LocationMapperProfile LocationMapperProfile { get; }
        private MapperConfiguration MapperConfiguration { get; }
        public ModelManager ModelManager { get; }

        public UserModel CurrentUser { get; internal set; }

        public DbLeagueServiceClient DbContext { get; }

        private ObservableCollection<LeagueMember> memberList;
        public ObservableCollection<LeagueMember> MemberList => memberList;

        private ObservableCollection<SeasonModel> seasons;
        public ObservableCollection<SeasonModel> Seasons => seasons;

        //public DatabaseStatusModel DbStatus => DbContext?.Status;

        public LocationCollection LocationCollection { get; } = new LocationCollection();

        private string DatabaseName { get; } = "TestDatabase";

        //private ObservableCollection<SessionBase> sessions;
        //public ReadOnlyObservableCollection<SessionBase> Sessions => new ReadOnlyObservableCollection<SessionBase>(sessions);

        //private List<Season> ActiveSeasons { get; } = new List<Season>();
        //private List<Schedule> ActiveSchedules { get; } = new List<Schedule>();
        //private List<SessionBase> ActiveSessions { get; } = new List<SessionBase>();

        public LeagueContext() : base()
        {
            MapperProfile = new ModelMapperProfile();
            DbContext = new DbLeagueServiceClient();
            DbContext.SetDatabaseName(DatabaseName);
            LocationMapperProfile = new LocationMapperProfile(LocationCollection);
            MapperProfile.LeagueContext = this;
            memberList = new ObservableCollection<LeagueMember>();
            MapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(MapperProfile);
                cfg.AddProfile(LocationMapperProfile);
                cfg.AddCollectionMappers();
            });
            seasons = new ObservableCollection<SeasonModel>();
            ModelManager = new ModelManager();
            //DbStatus.ConnectionStatus = ConnectionStatusEnum.Connected;
        }

        public async Task<IEnumerable<SeasonModel>> GetSeasonListAsync()
        {
            return await GetModelsAsync<SeasonModel>();
        }

        public async Task UpdateMemberList()
        {
            var mapper = MapperConfiguration.CreateMapper();
            var memberData = await DbContext.GetAsync<LeagueMemberDataDTO>();
            var updateList = mapper.Map<IEnumerable<LeagueMember>>(memberData).ToArray();
        }

        public LeagueContext(IDatabaseStatus status) : this()
        {
            DbContext = new DbLeagueServiceClient(status);
        }

        public void SetUser(UserModel user)
        {
            CurrentUser = user;
        }

        public void AddStatusItem(IDatabaseStatus statusItem)
        {
            DbContext.AddStatusItem(statusItem);
        }

        public void RemoveStatusItem(IDatabaseStatus statusItem)
        {
            DbContext.RemoveStatusItem(statusItem);
        }

        public async Task<T> GetModelAsync<T>(long[] modelId) where T : ModelBase
        {
            var mapper = MapperConfiguration.CreateMapper();
            object data = null;
            var requestId = modelId.ToArray();
            T model = ModelManager.GetModel<T>(modelId);

            if (model != null)
                return model;

            if (typeof(T).Equals(typeof(SeasonModel)))
            {
                data = await DbContext.GetAsync<SeasonDataDTO>(requestId);
            }
            else if (typeof(T).Equals(typeof(ScheduleModel)))
            {
                //data = await DbContext.GetScheduleAsync(modelId);
                data = await DbContext.GetAsync<ScheduleDataDTO>(requestId);
            }
            else if (typeof(T).Equals(typeof(SessionModel)))
            {
                data = await DbContext.GetAsync<SessionDataDTO>(requestId);
            }
            else if (typeof(T).Equals(typeof(RaceSessionModel)))
            {
                data = await DbContext.GetAsync<RaceSessionDataDTO>(requestId);
                if (((SessionModel)data).SessionType != SessionType.Race)
                {
                    throw new Exception("Could not load race session #" + modelId + ". Session has not the type \"Race\"");
                }
            }
            else if (typeof(T).Equals(typeof(IncidentReviewModel)))
            {
                data = await DbContext.GetAsync<IncidentReviewDataDTO>(requestId);
            }
            else if (typeof(T).Equals(typeof(CommentBase)))
            {
                data = await DbContext.GetAsync<CommentDataDTO>(requestId);
            }
            else if (typeof(T).Equals(typeof(ReviewCommentModel)))
            {
                data = await DbContext.GetAsync<ReviewCommentDataDTO>(requestId);
            }
            else if (typeof(T).Equals(typeof(ResultModel)))
            {
                data = await DbContext.GetAsync<ResultDataDTO>(requestId);
            }
            else if (typeof(T).Equals(typeof(LeagueMember)))
            {
                data = await DbContext.GetAsync<LeagueMemberDataDTO>(requestId);
            }
            else if (typeof(T).Equals(typeof(ScoringModel)))
            {
                data = await DbContext.GetAsync<ScoringDataDTO>(requestId);
            }
            else if (typeof(T).Equals(typeof(ScoringRuleBase)))
            {
                throw new NotImplementedException("Loading of model from type " + typeof(T).ToString() + " not yet supported.");
            }
            else if (typeof(T).Equals(typeof(ScoredResultModel)))
            {
                data = await DbContext.GetAsync<ScoredResultDataDTO>(requestId);
            }
            else
            {
                throw new UnknownModelTypeException("Could not load Model of type " + typeof(T).ToString() + ". Model type not known.");
            }


            if (data != null)
            {
                model = mapper.Map<T>(data);
                model.InitializeModel();
                return model;
            }
            else
            {
                throw new Exception("Could not load model of type " + typeof(T).ToString() + " with ID: " + modelId.Select(x => x.ToString()).Aggregate((x,y) => x + " | " + y) + ". Database response was NULL. (No matching element found)");
            }
        }

        public async Task<T> GetModelAsync<T>(long modelId, long? modelId2nd = null, long? modelId3rd = null) where T : ModelBase
        {
            List<long> requestId = new List<long>();
            requestId.Add(modelId);
            if (modelId2nd != null)
                requestId.Add(modelId2nd.Value);
            if (modelId3rd != null)
                requestId.Add(modelId3rd.Value);

            return await GetModelAsync<T>(requestId.ToArray());
        }

        public async Task<IEnumerable<T>> GetModelsAsync<T>(IEnumerable<long> modelIds) where T : ModelBase
        {
            return await GetModelsAsync<T>(modelIds.Select(x => new long[] { x }));
        }

        public async Task<IEnumerable<T>> GetModelsAsync<T>(IEnumerable<long[]> modelIds = null) where T : ModelBase
        {
            object[] data = null;
            List<T> modelList = new List<T>();
            List<long[]> getModelIds = new List<long[]>();

            if (modelIds != null)
            {
                foreach (var modelId in modelIds)
                {
                    var loadedModel = ModelManager.GetModel<T>(modelId);
                    if (loadedModel != null)
                        modelList.Add(loadedModel);
                    else
                        getModelIds.Add(modelId);
                }
            }
            else
            {
                getModelIds = null;
            }

            if (getModelIds == null || getModelIds.Count > 0)
            {
                if (typeof(T).Equals(typeof(SeasonModel)))
                {
                    data = await DbContext.GetAsync<SeasonDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(LeagueMember)))
                {
                    data = await DbContext.GetAsync<LeagueMemberDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(ScheduleModel)))
                {
                    data = await DbContext.GetAsync<ScheduleDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(SessionModel)))
                {
                    data = await DbContext.GetAsync<SessionDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(RaceSessionModel)))
                {
                    data = await DbContext.GetAsync<RaceSessionDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(IncidentReviewModel)))
                {
                    data = await DbContext.GetAsync<IncidentReviewDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(CommentBase)))
                {
                    data = await DbContext.GetAsync<CommentDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(ReviewCommentModel)))
                {
                    data = await DbContext.GetAsync<ReviewCommentDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(ResultModel)))
                {
                    data = await DbContext.GetAsync<ResultDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(LeagueMember)))
                {
                    data = await DbContext.GetAsync<LeagueMemberDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(ScoringModel)))
                {
                    data = await DbContext.GetAsync<ScoringDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(ScoringRuleBase)))
                {
                    throw new NotImplementedException("Loading of model from type " + typeof(T).ToString() + " not yet supported.");
                }
                else
                {
                    throw new UnknownModelTypeException("Could not load Model of type " + typeof(T).ToString() + ". Model type not known.");
                }

                if (data == null)
                {
                    foreach (var modelId in getModelIds)
                    {
                        var add = await GetModelAsync<T>(modelId.ToArray());
                        if (add == null)
                            return new T[0];
                        modelList.Add(add);
                    }
                }
                else
                {
                    var mapper = MapperConfiguration.CreateMapper();
                    var addList = new List<T>();
                    mapper.Map(data, addList);
                    modelList.AddRange(addList);
                }
            }

            foreach(var model in modelList)
            {
                model.InitializeModel();
            }

            return modelList;
        } 

        public async Task<T> UpdateModelAsync<T>(T model) where T : ModelBase
        {
            var mapper = MapperConfiguration.CreateMapper();
            object data;
            
            if (model is SeasonModel)
            {
                data = mapper.Map<SeasonDataDTO>(model);
                data = await DbContext.PutAsync(data as SeasonDataDTO);
            }
            else if (model is ScheduleModel)
            {
                data = mapper.Map<ScheduleDataDTO>(model);
                data = await DbContext.PutAsync(data as ScheduleDataDTO);
            }
            else if (model is RaceSessionModel)
            {
                data = mapper.Map<RaceSessionDataDTO>(model);
                data = await DbContext.PutAsync(data as RaceSessionDataDTO);
                if (((SessionDataDTO)data).SessionType != SessionType.Race)
                {
                    throw new Exception("Could not load race session #" + (model as SessionModel).SessionId + ". Session has not the type \"Race\"");
                }
            }
            else if (model is SessionModel)
            {
                data = mapper.Map<SessionDataDTO>(model);
                data = await DbContext.PutAsync(data as SessionDataDTO);
            }
            else if (model is IncidentReviewModel)
            {
                data = mapper.Map<IncidentReviewDataDTO>(model);
                data = await DbContext.PutAsync(data as IncidentReviewDataDTO);
            }
            else if (model is CommentBase)
            {
                //data = mapper.Map<CommentDataDTO>(model);
                //data = await DbContext.PutComment(model as CommentDataDTO);
                throw new NotImplementedException("Loading of model from type " + typeof(T).ToString() + " not yet supported.");
            }
            else if (model is ReviewCommentModel)
            {
                data = mapper.Map<ReviewCommentDataDTO>(model);
                data = await DbContext.PutAsync(data as ReviewCommentDataDTO);
            }
            else if (model is ResultModel)
            {
                data = mapper.Map<ResultDataDTO>(model);
                data = await DbContext.PutAsync(data as ResultDataDTO);
            }
            else if (model is LeagueMember)
            {
                data = mapper.Map<LeagueMemberDataDTO>(model);
                data = await DbContext.PutAsync(data as LeagueMemberDataDTO);
            }
            else if (model is ScoringModel)
            {
                throw new NotImplementedException("Loading of model from type " + typeof(T).ToString() + " not yet supported.");
            }
            else if (model is ScoringRuleBase)
            {
                throw new NotImplementedException("Loading of model from type " + typeof(T).ToString() + " not yet supported.");
            }
            else
            {
                throw new UnknownModelTypeException("Could not load Model of type " + typeof(T).ToString() + ". Model type not known.");
            }

            mapper.Map(data, model);
            model.InitializeModel();
            return model;
        }

        public async Task<IEnumerable<T>> UpdateModelsAsync<T>(IEnumerable<T> models) where T : ModelBase
        {
            List<T> modelList = models.ToList();

            var mapper = MapperConfiguration.CreateMapper();
            object[] data = null;

            if (typeof(T).Equals(typeof(SeasonModel)))
            {
                data = mapper.Map<IEnumerable<SeasonDataDTO>>(models).ToArray();
                data = await DbContext.PutAsync(data.Cast<SeasonDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(SessionModel)))
            {
                data = mapper.Map<IEnumerable<SessionDataDTO>>(models).ToArray();
                data = await DbContext.PutAsync(data.Cast<SessionDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(RaceSessionModel)))
            {
                data = mapper.Map<IEnumerable<RaceSessionDataDTO>>(models).ToArray();
                data = await DbContext.PutAsync(data.Cast<RaceSessionDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ScheduleModel)))
            {
                data = mapper.Map<IEnumerable<ScheduleDataDTO>>(models).ToArray();
                data = await DbContext.PutAsync(data.Cast<ScheduleDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(IncidentReviewModel)))
            {
                data = mapper.Map<IEnumerable<IncidentReviewDataDTO>>(models).ToArray();
                data = await DbContext.PutAsync(data.Cast<IncidentReviewDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(CommentBase)))
            {
                data = mapper.Map<IEnumerable<CommentDataDTO>>(models).ToArray();
                data = await DbContext.PutAsync(data.Cast<CommentDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ReviewCommentModel)))
            {
                data = mapper.Map<IEnumerable<ReviewCommentDataDTO>>(models).ToArray();
                data = await DbContext.PutAsync(data.Cast<ReviewCommentDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ResultModel)))
            {
                data = mapper.Map<IEnumerable<ResultDataDTO>>(models).ToArray();
                data = await DbContext.PutAsync(data.Cast<ResultDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(LeagueMember)))
            {
                data = mapper.Map<IEnumerable<LeagueMemberDataDTO>>(models).ToArray();
                data = await DbContext.PutAsync(data.Cast<LeagueMemberDataDTO>().ToArray());
            }
            else if(typeof(T).Equals(typeof(ScoringModel)))
            {
                data = mapper.Map<IEnumerable<ScoringDataDTO>>(models).ToArray();
                data = await DbContext.PutAsync(data.Cast<ScoringDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ScoringRuleBase)))
            {
                throw new NotImplementedException("Loading of model from type " + typeof(T).ToString() + " not yet supported.");
            }
            else
            {
                throw new UnknownModelTypeException("Could not load Model of type " + typeof(T).ToString() + ". Model type not known.");
            }

            for (int i=0; i<modelList.Count; i++)
            {
                if (data[i] != null)
                {
                    modelList[i] = mapper.Map<T>(data[i]);
                    modelList[i].InitializeModel();
                }
                else
                {
                    modelList[i] = null;
                }
            }

            return modelList;
        }
    }
    
    public class UnknownModelTypeException : Exception
    {
        public UnknownModelTypeException() : base() { }
        
        public UnknownModelTypeException(string message) : base(message) { }
        
        public UnknownModelTypeException(string message, Exception innerException) : base(message, innerException) { }
    }
}
