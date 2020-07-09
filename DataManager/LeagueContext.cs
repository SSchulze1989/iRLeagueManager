using iRLeagueManager.Interfaces;
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
using System.Data;
using System.Collections;

namespace iRLeagueManager.Data
{
    public class LeagueContext
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
            DbContext = new DbLeagueServiceClient(new WCFLeagueDbClientWrapper());
            DbContext.OpenConnection();
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
            CurrentUser = UserModel.GetAnonymous();
            _ = UpdateMemberList();
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
            foreach(var member in updateList)
            {
                ModelManager.PutOrGetModel(member);
            }
            memberList = new ObservableCollection<LeagueMember>(updateList);
        }

        public LeagueContext(IDatabaseStatus status) : this()
        {
            DbContext.AddStatusItem(status);
        }

        public async Task<bool> UserLoginAsync(string userName, byte[] password)
        {
            var authResult = await DbContext.AuthenticateUserAsync(userName, password);

            var mapper = MapperConfiguration.CreateMapper();
            if (authResult.IsAuthenticated)
            {
                CurrentUser = mapper.Map<UserModel>(authResult.AuthenticatedUser);
                return true;
            }
            else
            {
                CurrentUser = UserModel.GetAnonymous();
            }

            return false;
        }

        public void Reconnect()
        {
            if (DbContext != null)
            {
                DbContext.SetDbClient(new WCFLeagueDbClientWrapper());
                CurrentUser = UserModel.GetAnonymous();
            }
        }

        public void AddStatusItem(IDatabaseStatus statusItem)
        {
            DbContext.AddStatusItem(statusItem);
        }

        public void RemoveStatusItem(IDatabaseStatus statusItem)
        {
            DbContext.RemoveStatusItem(statusItem);
        }

        public async Task<T> GetModelAsync<T>(params long[] modelId) where T : ModelBase
        {
            return await GetModelAsync<T>(modelId, true);
        }

        public async Task<T> GetModelAsync<T>(long[] modelId, bool update = true, bool reload = false) where T : ModelBase
        {
            return (await GetModelsAsync<T>(new long[][] { modelId }, update, reload)).FirstOrDefault();
            //var mapper = MapperConfiguration.CreateMapper();
            //object data = null;
            //var requestId = modelId.ToArray();
            //T model = ModelManager.GetModel<T>(modelId);

            //if (model != null)
            //{
            //    if (update)
            //        _ = UpdateModelAsync(model);
            //    return model;
            //}

            ////if (typeof(T).Equals(typeof(SeasonModel)))
            ////{
            ////    data = await DbContext.GetAsync<SeasonDataDTO>(requestId);
            ////}
            ////else if (typeof(T).Equals(typeof(ScheduleModel)))
            ////{
            ////    //data = await DbContext.GetScheduleAsync(modelId);
            ////    data = await DbContext.GetAsync<ScheduleDataDTO>(requestId);
            ////}
            ////else if (typeof(T).Equals(typeof(SessionModel)))
            ////{
            ////    data = await DbContext.GetAsync<SessionDataDTO>(requestId);
            ////}
            ////else 
            //if (typeof(T).Equals(typeof(RaceSessionModel)))
            //{
            //    data = await DbContext.GetAsync<RaceSessionDataDTO>(requestId);
            //    if (((SessionModel)data).SessionType != SessionType.Race)
            //    {
            //        throw new Exception("Could not load race session #" + modelId + ". Session has not the type \"Race\"");
            //    }
            //}
            ////else if (typeof(T).Equals(typeof(IncidentReviewModel)))
            ////{
            ////    data = await DbContext.GetAsync<IncidentReviewDataDTO>(requestId);
            ////}
            ////else if (typeof(T).Equals(typeof(CommentBase)))
            ////{
            ////    data = await DbContext.GetAsync<CommentDataDTO>(requestId);
            ////}
            ////else if (typeof(T).Equals(typeof(ReviewCommentModel)))
            ////{
            ////    data = await DbContext.GetAsync<ReviewCommentDataDTO>(requestId);
            ////}
            ////else if (typeof(T).Equals(typeof(ResultModel)))
            ////{
            ////    data = await DbContext.GetAsync<ResultDataDTO>(requestId);
            ////}
            ////else if (typeof(T).Equals(typeof(LeagueMember)))
            ////{
            ////    data = await DbContext.GetAsync<LeagueMemberDataDTO>(requestId);
            ////}
            ////else if (typeof(T).Equals(typeof(ScoringModel)))
            ////{
            ////    data = await DbContext.GetAsync<ScoringDataDTO>(requestId);
            ////}
            //else if (typeof(T).Equals(typeof(ScoringRuleBase)))
            //{
            //    throw new NotImplementedException("Loading of model from type " + typeof(T).ToString() + " not yet supported.");
            //}
            ////else if (typeof(T).Equals(typeof(ScoredResultModel)))
            ////{
            ////    data = await DbContext.GetAsync<ScoredResultDataDTO>(requestId);
            ////}
            ////else if (typeof(T).Equals(typeof(ResultRowModel)))
            ////{
            ////    data = await DbContext.GetAsync<ResultRowDataDTO>(requestId);
            ////}
            ////else if (typeof(T).Equals(typeof(StandingsModel)))
            ////{
            ////    data = await DbContext.GetAsync<StandingsDataDTO>(requestId);
            ////}
            ////else if (typeof(T).Equals(typeof(AddPenaltyModel)))
            ////{
            ////    data = await DbContext.GetAsync<AddPenaltyDTO>(requestId);
            ////}
            ////else
            ////{
            ////    throw new UnknownModelTypeException("Could not load Model of type " + typeof(T).ToString() + ". Model type not known.");
            ////}
            //else
            //{
            //    data = DbContext.GetAsync(requestId, typeof(T)) as T;
            //}


            //if (data != null)
            //{
            //    model = mapper.Map<T>(data);
            //    model.InitializeModel();
            //    return model;
            //}
            //else
            //{
            //    throw new Exception("Could not load model of type " + typeof(T).ToString() + " with ID: " + modelId.Select(x => x.ToString()).Aggregate((x,y) => x + " | " + y) + ". Database response was NULL. (No matching element found)");
            //}
        }

        //public async Task<T> GetModelAsync<T>(long modelId, long? modelId2nd = null, long? modelId3rd = null, bool update = false) where T : ModelBase
        //{
        //    List<long> requestId = new List<long>();
        //    requestId.Add(modelId);
        //    if (modelId2nd != null)
        //        requestId.Add(modelId2nd.Value);
        //    if (modelId3rd != null)
        //        requestId.Add(modelId3rd.Value);

        //    return await GetModelAsync<T>(requestId.ToArray(), update);
        //}

        public async Task<IEnumerable<T>> GetModelsAsync<T>(IEnumerable<long> modelIds) where T : ModelBase
        {
            return await GetModelsAsync<T>(modelIds.Select(x => new long[] { x }).ToArray());
        }

        public async Task<IEnumerable<T>> GetModelsAsync<T>(IEnumerable<long[]> modelIds = null, bool update = true, bool reload = false) where T : ModelBase
        {
            object[] data = null;
            List<T> modelList = new List<T>();
            List<T> updateList = new List<T>();
            List<long[]> getModelIds = new List<long[]>();

            if (modelIds != null)
            {
                foreach (var modelId in modelIds)
                {
                    var loadedModel = ModelManager.GetModel<T>(modelId);
                    if (loadedModel != null)
                    {
                        modelList.Add(loadedModel);
                        if (reload)
                            getModelIds.Add(modelId);
                        else
                            updateList.Add(loadedModel);
                    }
                    else
                        getModelIds.Add(modelId);
                }
            }
            else
            {
                getModelIds = null;
            }

            //_ = Task.Run(async () =>
            //{
            //    await UpdateModelsAsync(updateList.ToArray());
            //});
            if (update)
                _ = UpdateModelsAsync(updateList.ToArray());

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
                else if (typeof(T).Equals(typeof(ScoredResultModel)))
                {
                    data = await DbContext.GetAsync<ScoredResultDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(ResultRowModel)))
                {
                    data = await DbContext.GetAsync<ResultRowDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(AddPenaltyModel)))
                {
                    data = await DbContext.GetAsync<AddPenaltyDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(ScoringRuleBase)))
                {
                    throw new NotImplementedException("Loading of model from type " + typeof(T).ToString() + " not yet supported.");
                }
                else
                {
                    throw new UnknownModelTypeException("Could not load Model of type " + typeof(T).ToString() + ". Model type not known.");
                }
                //data = await DbContext.GetAsync(getModelIds?.Select(x => x.ToArray()).ToArray(), typeof(T));

                if (data == null)
                {
                    if (getModelIds == null)
                        return null;

                    foreach (var modelId in getModelIds)
                    {
                        var add = await GetModelAsync<T>(modelId.ToArray());
                        if (add == null)
                            return new T[0];
                        if (modelList.Any(x => x.ModelId.SequenceEqual(add.ModelId)))
                            add.CopyTo(modelList.SingleOrDefault(x => x.ModelId == add.ModelId));
                        else
                            modelList.Add(add);
                    }
                }
                else
                {
                    var mapper = MapperConfiguration.CreateMapper();
                    var addList = new List<T>();
                    mapper.Map(data.Where(x => x != null), addList);
                    //modelList.AddRange(addList);
                    foreach (var add in addList)
                    {
                        if (modelList.Any(x => x != null && x.ModelId.SequenceEqual(add.ModelId)))
                            add.CopyTo(modelList.SingleOrDefault(x => x.ModelId == add.ModelId));
                        else
                            modelList.Add(add);
                    }
                }
            }

            foreach(var model in modelList)
            {
                if (model != null)
                    model.InitializeModel();
            }

            if (modelIds == null)
                return modelList;
            else
            {
                return modelIds.Select(x => modelList.SingleOrDefault(y => y != null && y.ModelId.SequenceEqual(x)));
            }
        } 

        public async Task<T> UpdateModelAsync<T>(T model) where T : ModelBase
        {
            return (await UpdateModelsAsync<T>(new T[] { model })).FirstOrDefault();
            //var mapper = MapperConfiguration.CreateMapper();
            //object data;
            
            //if (model is SeasonModel)
            //{
            //    data = mapper.Map<SeasonDataDTO>(model);
            //    data = await DbContext.PutAsync(data as SeasonDataDTO);
            //}
            //else if (model is ScheduleModel)
            //{
            //    data = mapper.Map<ScheduleDataDTO>(model);
            //    data = await DbContext.PutAsync(data as ScheduleDataDTO);
            //}
            //else if (model is RaceSessionModel)
            //{
            //    data = mapper.Map<RaceSessionDataDTO>(model);
            //    data = await DbContext.PutAsync(data as RaceSessionDataDTO);
            //    if (((SessionDataDTO)data).SessionType != SessionType.Race)
            //    {
            //        throw new Exception("Could not load race session #" + (model as SessionModel).SessionId + ". Session has not the type \"Race\"");
            //    }
            //}
            //else if (model is SessionModel)
            //{
            //    data = mapper.Map<SessionDataDTO>(model);
            //    data = await DbContext.PutAsync(data as SessionDataDTO);
            //}
            //else if (model is IncidentReviewModel)
            //{
            //    data = mapper.Map<IncidentReviewDataDTO>(model);
            //    data = await DbContext.PutAsync(data as IncidentReviewDataDTO);
            //}
            //else if (model is CommentBase)
            //{
            //    //data = mapper.Map<CommentDataDTO>(model);
            //    //data = await DbContext.PutComment(model as CommentDataDTO);
            //    throw new NotImplementedException("Loading of model from type " + typeof(T).ToString() + " not yet supported.");
            //}
            //else if (model is ReviewCommentModel)
            //{
            //    data = mapper.Map<ReviewCommentDataDTO>(model);
            //    data = await DbContext.PutAsync(data as ReviewCommentDataDTO);
            //}
            //else if (model is ResultModel)
            //{
            //    data = mapper.Map<ResultDataDTO>(model);
            //    data = await DbContext.PutAsync(data as ResultDataDTO);
            //}
            //else if (model is LeagueMember)
            //{
            //    data = mapper.Map<LeagueMemberDataDTO>(model);
            //    data = await DbContext.PutAsync(data as LeagueMemberDataDTO);
            //}
            //else if (model is ResultRowModel)
            //{
            //    data = mapper.Map<ResultRowDataDTO>(model);
            //    data = await DbContext.PutAsync(data as ResultRowDataDTO);
            //}
            //else if (model is ScoredResultModel)
            //{
            //    data = await DbContext.GetAsync<ScoredResultDataDTO>(model.ModelId);
            //}
            //else if (model is StandingsModel)
            //{
            //    data = await DbContext.GetAsync<StandingsDataDTO>(model.ModelId);
            //}
            //else if (model is ScoringModel)
            //{
            //    data = mapper.Map<ScoringDataDTO>(model);
            //    data = await DbContext.PutAsync(data as ScoringDataDTO);
            //}
            //else if (model is AddPenaltyModel)
            //{
            //    data = mapper.Map<AddPenaltyDTO>(model);
            //    data = await DbContext.PutAsync(data as AddPenaltyDTO);
            //}
            //else if (model is ScoringRuleBase)
            //{
            //    throw new NotImplementedException("Loading of model from type " + typeof(T).ToString() + " not yet supported.");
            //}
            //else
            //{
            //    throw new UnknownModelTypeException("Could not load Model of type " + typeof(T).ToString() + ". Model type not known.");
            //}

            //mapper.Map(data, model);
            //model.InitializeModel();
            //return model;
        }

        public async Task<IEnumerable<T>> UpdateModelsAsync<T>(IEnumerable<T> models) where T : ModelBase
        {
            List<T> modelList = models.ToList();

            var mapper = MapperConfiguration.CreateMapper();
            object[] data = new object[0];

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
            else if (typeof(T).Equals(typeof(ScoringModel)))
            {
                data = mapper.Map<IEnumerable<ScoringDataDTO>>(models).ToArray();
                data = await DbContext.PutAsync(data.Cast<ScoringDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ResultRowModel)))
            {
                data = mapper.Map<IEnumerable<ResultRowDataDTO>>(models).ToArray();
                data = await DbContext.PutAsync(data.Cast<ResultRowDataDTO>().ToArray());
            }
            else
            if (typeof(T).Equals(typeof(ScoredResultModel)))
            {
                data = await DbContext.GetAsync<ScoredResultDataDTO>(models.Select(x => x.ModelId.ToArray()).ToArray());
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
                return null;

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


        public async Task<bool> DeleteModelsAsync<T>(params T[] models) where T : ModelBase
        {
            if (models.Count() == 0)
                return true;

            return await DeleteModelsAsync<T>(models.Select(x => x.ModelId.ToArray()).ToArray());
        }

        public async Task<bool> DeleteModelsAsync<T>(long[] modelid) where T : ModelBase
        {
            return await DeleteModelsAsync<T>(new long[][] { modelid });
        }

        public async Task<bool> DeleteModelsAsync<T>(long[][] modelIds) where T : ModelBase
        { 
            if (typeof(T).Equals(typeof(SeasonModel)))
            {
                return await DbContext.DelAsync<SeasonDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(SessionModel)))
            {
                return await DbContext.DelAsync<SessionDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(RaceSessionModel)))
            {
                return await DbContext.DelAsync<RaceSessionDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(ScheduleModel)))
            {
                return await DbContext.DelAsync<ScheduleDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(IncidentReviewModel)))
            {
                return await DbContext.DelAsync<IncidentReviewDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(CommentBase)))
            {
                return await DbContext.DelAsync<CommentDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(ReviewCommentModel)))
            {
                return await DbContext.DelAsync<ReviewCommentDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(ResultModel)))
            {
                return await DbContext.DelAsync<ResultDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(LeagueMember)))
            {
                return await DbContext.DelAsync<LeagueMemberDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(ScoringModel)))
            {
                return await DbContext.DelAsync<ScoringDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(ResultRowModel)))
            {
                return await DbContext.DelAsync<ResultRowDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(AddPenaltyModel)))
            {
                return await DbContext.DelAsync<AddPenaltyDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(ScoredResultModel)))
            {
                return await DbContext.DelAsync<ScoredResultDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(ScoredResultRowModel)))
            {
                return await DbContext.DelAsync<ScoredResultRowDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(ScoringRuleBase)))
            {
                throw new NotImplementedException("Loading of model from type " + typeof(T).ToString() + " not yet supported.");
            }
            else
            {
                throw new UnknownModelTypeException("Could not load Model of type " + typeof(T).ToString() + ". Model type not known.");
            }
        }

        public async Task<T> AddModelAsync<T>(T model) where T : ModelBase
            => (await AddModelsAsync(model)).FirstOrDefault();

        public async Task<IEnumerable<T>> AddModelsAsync<T>(params T[] models) where T : ModelBase
        {
            List<T> modelList = models.ToList();

            var mapper = MapperConfiguration.CreateMapper();
            object[] data = null;

            if (typeof(T).Equals(typeof(SeasonModel)))
            {
                data = mapper.Map<IEnumerable<SeasonDataDTO>>(models).ToArray();
                data = await DbContext.PostAsync(data.Cast<SeasonDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(SessionModel)))
            {
                data = mapper.Map<IEnumerable<SessionDataDTO>>(models).ToArray();
                data = await DbContext.PostAsync(data.Cast<SessionDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(RaceSessionModel)))
            {
                data = mapper.Map<IEnumerable<RaceSessionDataDTO>>(models).ToArray();
                data = await DbContext.PostAsync(data.Cast<RaceSessionDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ScheduleModel)))
            {
                data = mapper.Map<IEnumerable<ScheduleDataDTO>>(models).ToArray();
                data = await DbContext.PostAsync(data.Cast<ScheduleDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(IncidentReviewModel)))
            {
                data = mapper.Map<IEnumerable<IncidentReviewDataDTO>>(models).ToArray();
                data = await DbContext.PostAsync(data.Cast<IncidentReviewDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(CommentBase)))
            {
                data = mapper.Map<IEnumerable<CommentDataDTO>>(models).ToArray();
                data = await DbContext.PostAsync(data.Cast<CommentDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ReviewCommentModel)))
            {
                data = mapper.Map<IEnumerable<ReviewCommentDataDTO>>(models).ToArray();
                data = await DbContext.PostAsync(data.Cast<ReviewCommentDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ResultModel)))
            {
                data = mapper.Map<IEnumerable<ResultDataDTO>>(models).ToArray();
                data = await DbContext.PostAsync(data.Cast<ResultDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(LeagueMember)))
            {
                data = mapper.Map<IEnumerable<LeagueMemberDataDTO>>(models).ToArray();
                data = await DbContext.PostAsync(data.Cast<LeagueMemberDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ScoringModel)))
            {
                data = mapper.Map<IEnumerable<ScoringDataDTO>>(models).ToArray();
                data = await DbContext.PostAsync(data.Cast<ScoringDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ResultRowModel)))
            {
                data = mapper.Map<IEnumerable<ResultRowDataDTO>>(models).ToArray();
                data = await DbContext.PostAsync(data.Cast<ResultRowDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(AddPenaltyModel)))
            {
                data = mapper.Map<IEnumerable<AddPenaltyDTO>>(models).ToArray();
                data = await DbContext.PostAsync(data.Cast<AddPenaltyDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ScoringRuleBase)))
            {
                throw new NotImplementedException("Loading of model from type " + typeof(T).ToString() + " not yet supported.");
            }
            else
            {
                throw new UnknownModelTypeException("Could not load Model of type " + typeof(T).ToString() + ". Model type not known.");
            }

            for (int i = 0; i < modelList.Count; i++)
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

        private Type GetModelDtoType(Type modelType)
        {
            if (modelType.Equals(typeof(SeasonModel)))
            {
                return typeof(SeasonDataDTO);
            }
            else if (modelType.Equals(typeof(SessionModel)))
            {
                return typeof(SessionDataDTO);
            }
            else if (modelType.Equals(typeof(RaceSessionModel)))
            {
                return typeof(RaceSessionDataDTO);
            }
            else if (modelType.Equals(typeof(ScheduleModel)))
            {
                return typeof(ScheduleDataDTO);
            }
            else if (modelType.Equals(typeof(IncidentReviewModel)))
            {
                return typeof(IncidentReviewDataDTO);
            }
            else if (modelType.Equals(typeof(CommentBase)))
            {
                return typeof(CommentDataDTO);
            }
            else if (modelType.Equals(typeof(ReviewCommentModel)))
            {
                return typeof(ReviewCommentDataDTO);
            }
            else if (modelType.Equals(typeof(ResultModel)))
            {
                return typeof(ResultDataDTO);
            }
            else if (modelType.Equals(typeof(LeagueMember)))
            {
                return typeof(LeagueMemberDataDTO);
            }
            else if (modelType.Equals(typeof(ScoringModel)))
            {
                return typeof(ScoringDataDTO);
            }
            else if (modelType.Equals(typeof(ResultRowModel)))
            {
                return typeof(ResultRowDataDTO);
            }
            else if (modelType.Equals(typeof(AddPenaltyModel)))
            {
                return typeof(AddPenaltyDTO);
            }
            else if (modelType.Equals(typeof(ScoredResultModel)))
            {
                return typeof(ScoredResultDataDTO);
            }
            else if (modelType.Equals(typeof(ScoredResultRowModel)))
            {
                return typeof(ScoredResultRowDataDTO);
            }
            else if (modelType.Equals(typeof(ScoringRuleBase)))
            {
                throw new NotImplementedException("Loading of model from type " + modelType.ToString() + " not yet supported.");
            }
            else
            {
                throw new UnknownModelTypeException("Could not load Model of type " + modelType.ToString() + ". Model type not known.");
            }
        }

        private Type GetModelDtoEnumerable(Type modelType)
        {
            if (modelType.Equals(typeof(SeasonModel)))
            {
                return typeof(IEnumerable<SeasonDataDTO>);
            }
            else if (modelType.Equals(typeof(SessionModel)))
            {
                return typeof(IEnumerable<SessionDataDTO>);
            }
            else if (modelType.Equals(typeof(RaceSessionModel)))
            {
                return typeof(IEnumerable<RaceSessionDataDTO>);
            }
            else if (modelType.Equals(typeof(ScheduleModel)))
            {
                return typeof(IEnumerable<ScheduleDataDTO>);
            }
            else if (modelType.Equals(typeof(IncidentReviewModel)))
            {
                return typeof(IEnumerable<IncidentReviewDataDTO>);
            }
            else if (modelType.Equals(typeof(CommentBase)))
            {
                return typeof(IEnumerable<CommentDataDTO>);
            }
            else if (modelType.Equals(typeof(ReviewCommentModel)))
            {
                return typeof(IEnumerable<ReviewCommentDataDTO>);
            }
            else if (modelType.Equals(typeof(ResultModel)))
            {
                return typeof(IEnumerable<ResultDataDTO>);
            }
            else if (modelType.Equals(typeof(LeagueMember)))
            {
                return typeof(IEnumerable<LeagueMemberDataDTO>);
            }
            else if (modelType.Equals(typeof(ScoringModel)))
            {
                return typeof(IEnumerable<ScoringDataDTO>);
            }
            else if (modelType.Equals(typeof(ResultRowModel)))
            {
                return typeof(IEnumerable<ResultRowDataDTO>);
            }
            else if (modelType.Equals(typeof(AddPenaltyModel)))
            {
                return typeof(IEnumerable<AddPenaltyDTO>);
            }
            else if (modelType.Equals(typeof(ScoredResultModel)))
            {
                return typeof(IEnumerable<ScoredResultDataDTO>);
            }
            else if (modelType.Equals(typeof(ScoredResultRowModel)))
            {
                return typeof(IEnumerable<ScoredResultRowDataDTO>);
            }
            else if (modelType.Equals(typeof(ScoringRuleBase)))
            {
                throw new NotImplementedException("Loading of model from type " + modelType.ToString() + " not yet supported.");
            }
            else
            {
                throw new UnknownModelTypeException("Could not load Model of type " + modelType.ToString() + ". Model type not known.");
            }
        }
    }
    
    public class UnknownModelTypeException : Exception
    {
        public UnknownModelTypeException() : base() { }
        
        public UnknownModelTypeException(string message) : base(message) { }
        
        public UnknownModelTypeException(string message, Exception innerException) : base(message, innerException) { }
    }
}
