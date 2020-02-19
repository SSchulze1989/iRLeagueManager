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

namespace iRLeagueManager.Data
{
    public class LeagueContext  //: ILeagueContext
    {
        private ModelMapperProfile MapperProfile { get; }
        private LocationMapperProfile LocationMapperProfile { get; }
        private MapperConfiguration MapperConfiguration { get; }

        public UserModel CurrentUser { get; internal set; }

        private DbLeagueServiceClient DbContext { get; }

        private ObservableCollection<LeagueMember> memberList;
        public ObservableCollection<LeagueMember> MemberList => memberList;

        private ObservableCollection<SeasonModel> seasons;
        public ObservableCollection<SeasonModel> Seasons => seasons;

        //public DatabaseStatusModel DbStatus => DbContext?.Status;

        public LocationCollection LocationCollection { get; } = new LocationCollection();

        //private ObservableCollection<SessionBase> sessions;
        //public ReadOnlyObservableCollection<SessionBase> Sessions => new ReadOnlyObservableCollection<SessionBase>(sessions);

        //private List<Season> ActiveSeasons { get; } = new List<Season>();
        //private List<Schedule> ActiveSchedules { get; } = new List<Schedule>();
        //private List<SessionBase> ActiveSessions { get; } = new List<SessionBase>();

        public LeagueContext() : base()
        {
            MapperProfile = new ModelMapperProfile();
            DbContext = new DbLeagueServiceClient();
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
            //DbStatus.ConnectionStatus = ConnectionStatusEnum.Connected;
        }

        public async Task<IEnumerable<SeasonInfo>> GetSeasonListAsync()
        {
            return await GetModelsAsync<SeasonModel>(null);
        }

        public async Task UpdateMemberList()
        {
            var mapper = MapperConfiguration.CreateMapper();
            var memberData = await DbContext.GetMembersAsync();
            mapper.Map(memberData, MemberList);
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

        public async Task<T> GetModelAsync<T>(long modelId) where T : ModelBase
        {
            var mapper = MapperConfiguration.CreateMapper();
            object data = null;

            if (typeof(T).Equals(typeof(SeasonModel)))
            {
                data = await DbContext.GetSeasonAsync(modelId);
            }
            else if (typeof(T).Equals(typeof(ScheduleModel)))
            {
                data = await DbContext.GetScheduleAsync(modelId);
            }
            else if (typeof(T).Equals(typeof(SessionModel)))
            {
                data = await DbContext.GetSessionAsync(modelId);
            }
            else if (typeof(T).Equals(typeof(RaceSessionModel)))
            {
                data = await DbContext.GetSessionAsync(modelId);
                if (((SessionModel)data).SessionType != SessionType.Race)
                {
                    throw new Exception("Could not load race session #" + modelId + ". Session has not the type \"Race\"");
                }
            }
            else if (typeof(T).Equals(typeof(IncidentReviewModel)))
            {
                data = await DbContext.GetReviewAsync(modelId);
            }
            else if (typeof(T).Equals(typeof(CommentBase)))
            {
                data = await DbContext.GetCommentAsync(modelId);
            }
            else if (typeof(T).Equals(typeof(ReviewCommentModel)))
            {
                data = await DbContext.GetCommentAsync(modelId);
            }
            else if (typeof(T).Equals(typeof(ResultModel)))
            {
                data = await DbContext.GetResultAsync(modelId);
            }
            else if (typeof(T).Equals(typeof(LeagueMember)))
            {
                data = await DbContext.GetMemberAsync(modelId);
            }
            else if (typeof(T).Equals(typeof(ScoringModel)))
            {
                throw new NotImplementedException("Loading of model from type " + typeof(T).ToString() + " not yet supported.");
            }
            else if (typeof(T).Equals(typeof(ScoringRuleBase)))
            {
                throw new NotImplementedException("Loading of model from type " + typeof(T).ToString() + " not yet supported.");
            }
            else
            {
                throw new UnknownModelTypeException("Could not load Model of type " + typeof(T).ToString() + ". Model type not known.");
            }

            if (data != null)
            {
                T model = mapper.Map<T>(data);
                model.InitializeModel();
                return model;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<T>> GetModelsAsync<T>(IEnumerable<long> modelIds = null) where T : ModelBase
        {
            object[] data = null;
            List<T> modelList = new List<T>();

            if (typeof(T).Equals(typeof(SeasonModel)))
            {
                data = await DbContext.GetSeasonsAsync(modelIds?.ToArray());
            }
            else if (typeof(T).Equals(typeof(LeagueMember)))
            {
                data = await DbContext.GetMembersAsync(modelIds?.ToArray());
            }
            else if (typeof(T).Equals(typeof(ScheduleModel)))
            {
                //data = await DbContext.GetScheduleAsync(modelId);
            }
            else if (typeof(T).Equals(typeof(SessionModel)))
            {
                //data = await DbContext.GetSessionAsync(modelId);
            }
            else if (typeof(T).Equals(typeof(RaceSessionModel)))
            {
                //data = await DbContext.GetSessionsAsync(modelIds);
                //if (((SessionModel)data).SessionType != SessionType.Race)
                //{
                //    throw new Exception("Could not load race session #" + modelId + ". Session has not the type \"Race\"");
                //}
            }
            else if (typeof(T).Equals(typeof(IncidentReviewModel)))
            {
                //data = await DbContext.GetReviewAsync(modelId);
            }
            else if (typeof(T).Equals(typeof(CommentBase)))
            {
                //data = await DbContext.GetCommentAsync(modelId);
            }
            else if (typeof(T).Equals(typeof(ReviewCommentModel)))
            {
                //data = await DbContext.GetCommentAsync(modelId);
            }
            else if (typeof(T).Equals(typeof(ResultModel)))
            {
                //data = await DbContext.GetResultAsync(modelId);
            }
            else if (typeof(T).Equals(typeof(LeagueMember)))
            {
                //data = await DbContext.GetMemberAsync(modelId);
            }
            else if (typeof(T).Equals(typeof(ScoringModel)))
            {
                //throw new NotImplementedException("Loading of model from type " + typeof(T).ToString() + " not yet supported.");
            }
            else if (typeof(T).Equals(typeof(ScoringRuleBase)))
            {
                //throw new NotImplementedException("Loading of model from type " + typeof(T).ToString() + " not yet supported.");
            }
            else
            {
                //throw new UnknownModelTypeException("Could not load Model of type " + typeof(T).ToString() + ". Model type not known.");
            }

            if (data == null)
            {
                foreach (var modelId in modelIds)
                {
                    var add = await GetModelAsync<T>(modelId);
                    if (add == null)
                        return new T[0];
                    modelList.Add(add);
                }
            }
            else
            {
                var mapper = MapperConfiguration.CreateMapper();
                mapper.Map(data, modelList);
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
            object data = null;
            
            if (model is SeasonModel)
            {
                data = mapper.Map<SeasonDataDTO>(model);
                data = await DbContext.PutSeasonAsync(data as SeasonDataDTO);
            }
            else if (model is ScheduleModel)
            {
                data = mapper.Map<ScheduleDataDTO>(model);
                data = await DbContext.PutScheduleAsync(data as ScheduleDataDTO);
            }
            else if (model is RaceSessionModel)
            {
                data = mapper.Map<RaceSessionDataDTO>(model);
                data = await DbContext.PutSessionAsync(data as RaceSessionDataDTO);
                if (((SessionDataDTO)data).SessionType != SessionType.Race)
                {
                    throw new Exception("Could not load race session #" + (model as SessionModel).SessionId + ". Session has not the type \"Race\"");
                }
            }
            else if (model is SessionModel)
            {
                data = mapper.Map<SessionDataDTO>(model);
                data = await DbContext.PutSessionAsync(data as SessionDataDTO);
            }
            else if (model is IncidentReviewModel)
            {
                data = mapper.Map<IncidentReviewDataDTO>(model);
                data = await DbContext.PutReviewAsync(data as IncidentReviewDataDTO);
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
                data = await DbContext.PutCommentAsync(data as ReviewCommentDataDTO);
            }
            else if (model is ResultModel)
            {
                data = mapper.Map<ResultDataDTO>(model);
                data = await DbContext.PutResultAsync(data as ResultDataDTO);
            }
            else if (model is LeagueMember)
            {
                data = mapper.Map<LeagueMemberDataDTO>(model);
                data = await DbContext.PutMemberAsync(data as LeagueMemberDataDTO);
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
            List<T> modelList = new List<T>();

            foreach (var model in models)
            {
                modelList.Add(await UpdateModelAsync<T>(model));
                model.InitializeModel();
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
