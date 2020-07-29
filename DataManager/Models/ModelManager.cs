﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

using iRLeagueManager.Models;
using iRLeagueManager.Models.Members;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Reviews;
using iRLeagueManager.Data;
using iRLeagueManager.LeagueDBServiceRef;

namespace iRLeagueManager.Models
{
    public class ModelManager : IModelManager
    {
        public ModelManager(IModelCache modelCache, IModelDataProvider modelDataProvider, IConfigurationProvider mapperConfiguration)
        {
            ModelCache = modelCache;
            ModelDataProvider = modelDataProvider;
            MapperConfiguration = mapperConfiguration as MapperConfiguration;
        }
        public IModelCache ModelCache { get; }

        private IModelDataProvider ModelDataProvider { get; }

        private MapperConfiguration MapperConfiguration { get; }

        public async Task<T> GetModelAsync<T>(params long[] modelId) where T : MappableModel
        {
            return await GetModelAsync<T>(modelId, true);
        }

        public async Task<T> GetModelAsync<T>(long[] modelId, bool update = true, bool reload = false) where T : MappableModel
        {
            return (await GetModelsAsync<T>(new long[][] { modelId }, update, reload)).FirstOrDefault();
        }

        public async Task<IEnumerable<T>> GetModelsAsync<T>(IEnumerable<long> modelIds) where T : MappableModel
        {
            return await GetModelsAsync<T>(modelIds.Select(x => new long[] { x }).ToArray());
        }

        public async Task<IEnumerable<T>> GetModelsAsync<T>(IEnumerable<long[]> modelIds = null, bool update = true, bool reload = false) where T : MappableModel
        {
            object[] data = null;
            List<T> modelList = new List<T>();
            List<T> updateList = new List<T>();
            List<long[]> getModelIds = new List<long[]>();

            if (modelIds != null)
            {
                foreach (var modelId in modelIds)
                {
                    var loadedModel = ModelCache.GetModel<T>(modelId);
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

            if (update)
            {
                _ = GetModelsAsync<T>(updateList.Where(x => x.ContainsChanges == false).Select(x => x.ModelId).ToArray(), update: false, reload: true);
                _ = UpdateModelsAsync(updateList.ToArray());
            }

            if (getModelIds == null || getModelIds.Count > 0)
            {
                if (typeof(T).Equals(typeof(SeasonModel)))
                {
                    data = await ModelDataProvider.GetAsync<SeasonDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(LeagueMember)))
                {
                    data = await ModelDataProvider.GetAsync<LeagueMemberDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(ScheduleModel)))
                {
                    data = await ModelDataProvider.GetAsync<ScheduleDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(SessionModel)))
                {
                    data = await ModelDataProvider.GetAsync<SessionDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(RaceSessionModel)))
                {
                    data = await ModelDataProvider.GetAsync<RaceSessionDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(IncidentReviewModel)))
                {
                    data = await ModelDataProvider.GetAsync<IncidentReviewDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(CommentModel)))
                {
                    data = await ModelDataProvider.GetAsync<CommentDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(ReviewCommentModel)))
                {
                    data = await ModelDataProvider.GetAsync<ReviewCommentDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(ResultModel)))
                {
                    data = await ModelDataProvider.GetAsync<ResultDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(LeagueMember)))
                {
                    data = await ModelDataProvider.GetAsync<LeagueMemberDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(ScoringModel)))
                {
                    data = await ModelDataProvider.GetAsync<ScoringDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(ScoredResultModel)))
                {
                    data = await ModelDataProvider.GetAsync<ScoredResultDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(ScoredResultRowModel)))
                {
                    data = await ModelDataProvider.GetAsync<ScoredResultRowDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(ResultRowModel)))
                {
                    data = await ModelDataProvider.GetAsync<ResultRowDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(StandingsModel)))
                {
                    data = await ModelDataProvider.GetAsync<StandingsDataDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
                }
                else if (typeof(T).Equals(typeof(AddPenaltyModel)))
                {
                    data = await ModelDataProvider.GetAsync<AddPenaltyDTO>(getModelIds?.Select(x => x.ToArray()).ToArray());
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
                    var validData = data.Where(x => x != null);
                    mapper.Map(validData, addList);
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

            foreach (var model in modelList)
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

        public async Task<T> UpdateModelAsync<T>(T model) where T : MappableModel
        {
            return (await UpdateModelsAsync<T>(new T[] { model })).FirstOrDefault();
        }

        public async Task<IEnumerable<T>> UpdateModelsAsync<T>(IEnumerable<T> models) where T : MappableModel
        {
            List<T> modelList = models.ToList();

            var mapper = MapperConfiguration.CreateMapper();
            object[] data = new object[0];

            if (models == null)
                return null;
            if (models.Count() == 0)
                return modelList;

            if (typeof(T).Equals(typeof(SeasonModel)))
            {
                data = mapper.Map<IEnumerable<SeasonDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PutAsync(data.Cast<SeasonDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(SessionModel)))
            {
                data = mapper.Map<IEnumerable<SessionDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PutAsync(data.Cast<SessionDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(RaceSessionModel)))
            {
                data = mapper.Map<IEnumerable<RaceSessionDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PutAsync(data.Cast<RaceSessionDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ScheduleModel)))
            {
                data = mapper.Map<IEnumerable<ScheduleDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PutAsync(data.Cast<ScheduleDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(IncidentReviewModel)))
            {
                data = mapper.Map<IEnumerable<IncidentReviewDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PutAsync(data.Cast<IncidentReviewDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(CommentModel)))
            {
                data = mapper.Map<IEnumerable<CommentDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PutAsync(data.Cast<CommentDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ReviewCommentModel)))
            {
                data = mapper.Map<IEnumerable<ReviewCommentDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PutAsync(data.Cast<ReviewCommentDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ResultModel)))
            {
                data = mapper.Map<IEnumerable<ResultDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PutAsync(data.Cast<ResultDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(LeagueMember)))
            {
                data = mapper.Map<IEnumerable<LeagueMemberDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PutAsync(data.Cast<LeagueMemberDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ScoringModel)))
            {
                data = mapper.Map<IEnumerable<ScoringDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PutAsync(data.Cast<ScoringDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ResultRowModel)))
            {
                data = mapper.Map<IEnumerable<ResultRowDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PutAsync(data.Cast<ResultRowDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ScoredResultModel)))
            {
                data = await ModelDataProvider.GetAsync<ScoredResultDataDTO>(models.Select(x => x.ModelId.ToArray()).ToArray());
            }
            else if (typeof(T).Equals(typeof(StandingsModel)))
            {
                data = await ModelDataProvider.GetAsync<StandingsDataDTO>(models.Select(x => x.ModelId.ToArray()).ToArray());
            }
            else if (typeof(T).Equals(typeof(ScoringModel)))
            {
                data = mapper.Map<IEnumerable<ScoringDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PutAsync(data.Cast<ScoringDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(AddPenaltyModel)))
            {
                data = mapper.Map<IEnumerable<AddPenaltyDTO>>(models).ToArray();
                data = await ModelDataProvider.PutAsync(data.Cast<AddPenaltyDTO>().ToArray());
            }
            else
            {
                throw new UnknownModelTypeException("Could not put Model of type " + typeof(T).ToString() + ". Model type not known.");
            }

            if (data == null)
                return null;

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

        public async Task<bool> DeleteModelAsync<T>(params long[] modelId) where T : MappableModel
        {
            return await DeleteModelsAsync<T>(new long[][] { modelId });
        }

        public async Task<bool> DeleteModelsAsync<T>(params T[] models) where T : MappableModel
        {
            if (models.Count() == 0)
                return true;

            return await DeleteModelsAsync<T>(models.Select(x => x.ModelId.ToArray()).ToArray());
        }

        public async Task<bool> DeleteModelsAsync<T>(long[] modelIds) where T : MappableModel
        {
            return await DeleteModelsAsync<T>(modelIds.Select(x => new long[] { x }).ToArray());
        }

        public async Task<bool> DeleteModelsAsync<T>(long[][] modelIds) where T : MappableModel
        {
            if (typeof(T).Equals(typeof(SeasonModel)))
            {
                return await ModelDataProvider.DelAsync<SeasonDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(SessionModel)))
            {
                return await ModelDataProvider.DelAsync<SessionDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(RaceSessionModel)))
            {
                return await ModelDataProvider.DelAsync<RaceSessionDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(ScheduleModel)))
            {
                return await ModelDataProvider.DelAsync<ScheduleDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(IncidentReviewModel)))
            {
                return await ModelDataProvider.DelAsync<IncidentReviewDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(CommentModel)))
            {
                return await ModelDataProvider.DelAsync<CommentDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(ReviewCommentModel)))
            {
                return await ModelDataProvider.DelAsync<ReviewCommentDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(ResultModel)))
            {
                return await ModelDataProvider.DelAsync<ResultDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(LeagueMember)))
            {
                return await ModelDataProvider.DelAsync<LeagueMemberDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(ScoringModel)))
            {
                return await ModelDataProvider.DelAsync<ScoringDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(ResultRowModel)))
            {
                return await ModelDataProvider.DelAsync<ResultRowDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(AddPenaltyModel)))
            {
                return await ModelDataProvider.DelAsync<AddPenaltyDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(ScoredResultModel)))
            {
                return await ModelDataProvider.DelAsync<ScoredResultDataDTO>(modelIds);
            }
            else if (typeof(T).Equals(typeof(ScoredResultRowModel)))
            {
                return await ModelDataProvider.DelAsync<ScoredResultRowDataDTO>(modelIds);
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

        public async Task<T> AddModelAsync<T>(T model) where T : MappableModel
            => (await AddModelsAsync(model)).FirstOrDefault();

        public async Task<IEnumerable<T>> AddModelsAsync<T>(params T[] models) where T : MappableModel
        {
            List<T> modelList = models.ToList();

            var mapper = MapperConfiguration.CreateMapper();
            object[] data = null;

            if (typeof(T).Equals(typeof(SeasonModel)))
            {
                data = mapper.Map<IEnumerable<SeasonDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PostAsync(data.Cast<SeasonDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(SessionModel)))
            {
                data = mapper.Map<IEnumerable<SessionDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PostAsync(data.Cast<SessionDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(RaceSessionModel)))
            {
                data = mapper.Map<IEnumerable<RaceSessionDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PostAsync(data.Cast<RaceSessionDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ScheduleModel)))
            {
                data = mapper.Map<IEnumerable<ScheduleDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PostAsync(data.Cast<ScheduleDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(IncidentReviewModel)))
            {
                data = mapper.Map<IEnumerable<IncidentReviewDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PostAsync(data.Cast<IncidentReviewDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(CommentModel)))
            {
                data = mapper.Map<IEnumerable<CommentDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PostAsync(data.Cast<CommentDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ReviewCommentModel)))
            {
                data = mapper.Map<IEnumerable<ReviewCommentDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PostAsync(data.Cast<ReviewCommentDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ResultModel)))
            {
                data = mapper.Map<IEnumerable<ResultDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PostAsync(data.Cast<ResultDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(LeagueMember)))
            {
                data = mapper.Map<IEnumerable<LeagueMemberDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PostAsync(data.Cast<LeagueMemberDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ScoringModel)))
            {
                data = mapper.Map<IEnumerable<ScoringDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PostAsync(data.Cast<ScoringDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ResultRowModel)))
            {
                data = mapper.Map<IEnumerable<ResultRowDataDTO>>(models).ToArray();
                data = await ModelDataProvider.PostAsync(data.Cast<ResultRowDataDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(AddPenaltyModel)))
            {
                data = mapper.Map<IEnumerable<AddPenaltyDTO>>(models).ToArray();
                data = await ModelDataProvider.PostAsync(data.Cast<AddPenaltyDTO>().ToArray());
            }
            else if (typeof(T).Equals(typeof(ScoringRuleBase)))
            {
                throw new NotImplementedException("Loading of model from type " + typeof(T).ToString() + " not yet supported.");
            }
            else
            {
                throw new UnknownModelTypeException("Could not load Model of type " + typeof(T).ToString() + ". Model type not known.");
            }

            for (int i = 0; i < data.Count(); i++)
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
}