﻿using MediaBrowser.Common.Extensions;
using MediaBrowser.Controller.Dto;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Net;
using MediaBrowser.Controller.Persistence;
using MediaBrowser.Controller.TV;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Querying;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MediaBrowser.Api
{
    /// <summary>
    /// Class GetNextUpEpisodes
    /// </summary>
    [Route("/Shows/NextUp", "GET", Summary = "Gets a list of next up episodes")]
    public class GetNextUpEpisodes : IReturn<ItemsResult>, IHasDtoOptions
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        [ApiMember(Name = "UserId", Description = "User Id", IsRequired = true, DataType = "string", ParameterType = "query", Verb = "GET")]
        public string UserId { get; set; }

        /// <summary>
        /// Skips over a given number of items within the results. Use for paging.
        /// </summary>
        /// <value>The start index.</value>
        [ApiMember(Name = "StartIndex", Description = "Optional. The record index to start at. All items with a lower index will be dropped from the results.", IsRequired = false, DataType = "int", ParameterType = "query", Verb = "GET")]
        public int? StartIndex { get; set; }

        /// <summary>
        /// The maximum number of items to return
        /// </summary>
        /// <value>The limit.</value>
        [ApiMember(Name = "Limit", Description = "Optional. The maximum number of records to return", IsRequired = false, DataType = "int", ParameterType = "query", Verb = "GET")]
        public int? Limit { get; set; }

        /// <summary>
        /// Fields to return within the items, in addition to basic information
        /// </summary>
        /// <value>The fields.</value>
        [ApiMember(Name = "Fields", Description = "Optional. Specify additional fields of information to return in the output. This allows multiple, comma delimeted. Options: Budget, Chapters, CriticRatingSummary, DateCreated, Genres, HomePageUrl, IndexOptions, MediaStreams, Overview, ParentId, Path, People, ProviderIds, PrimaryImageAspectRatio, Revenue, SortName, Studios, Taglines, TrailerUrls", IsRequired = false, DataType = "string", ParameterType = "query", Verb = "GET", AllowMultiple = true)]
        public string Fields { get; set; }

        [ApiMember(Name = "SeriesId", Description = "Optional. Filter by series id", IsRequired = false, DataType = "string", ParameterType = "query", Verb = "GET")]
        public string SeriesId { get; set; }

        /// <summary>
        /// Specify this to localize the search to a specific item or folder. Omit to use the root.
        /// </summary>
        /// <value>The parent id.</value>
        [ApiMember(Name = "ParentId", Description = "Specify this to localize the search to a specific item or folder. Omit to use the root", IsRequired = false, DataType = "string", ParameterType = "query", Verb = "GET")]
        public string ParentId { get; set; }

        [ApiMember(Name = "EnableImages", Description = "Optional, include image information in output", IsRequired = false, DataType = "boolean", ParameterType = "query", Verb = "GET")]
        public bool? EnableImages { get; set; }

        [ApiMember(Name = "ImageTypeLimit", Description = "Optional, the max number of images to return, per image type", IsRequired = false, DataType = "int", ParameterType = "query", Verb = "GET")]
        public int? ImageTypeLimit { get; set; }

        [ApiMember(Name = "EnableImageTypes", Description = "Optional. The image types to include in the output.", IsRequired = false, DataType = "string", ParameterType = "query", Verb = "GET")]
        public string EnableImageTypes { get; set; }
    }

    [Route("/Shows/Upcoming", "GET", Summary = "Gets a list of upcoming episodes")]
    public class GetUpcomingEpisodes : IReturn<ItemsResult>, IHasDtoOptions
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        [ApiMember(Name = "UserId", Description = "User Id", IsRequired = true, DataType = "string", ParameterType = "query", Verb = "GET")]
        public string UserId { get; set; }

        /// <summary>
        /// Skips over a given number of items within the results. Use for paging.
        /// </summary>
        /// <value>The start index.</value>
        [ApiMember(Name = "StartIndex", Description = "Optional. The record index to start at. All items with a lower index will be dropped from the results.", IsRequired = false, DataType = "int", ParameterType = "query", Verb = "GET")]
        public int? StartIndex { get; set; }

        /// <summary>
        /// The maximum number of items to return
        /// </summary>
        /// <value>The limit.</value>
        [ApiMember(Name = "Limit", Description = "Optional. The maximum number of records to return", IsRequired = false, DataType = "int", ParameterType = "query", Verb = "GET")]
        public int? Limit { get; set; }

        /// <summary>
        /// Fields to return within the items, in addition to basic information
        /// </summary>
        /// <value>The fields.</value>
        [ApiMember(Name = "Fields", Description = "Optional. Specify additional fields of information to return in the output. This allows multiple, comma delimeted. Options: Budget, Chapters, CriticRatingSummary, DateCreated, Genres, HomePageUrl, IndexOptions, MediaStreams, Overview, ParentId, Path, People, ProviderIds, PrimaryImageAspectRatio, Revenue, SortName, Studios, Taglines, TrailerUrls", IsRequired = false, DataType = "string", ParameterType = "query", Verb = "GET", AllowMultiple = true)]
        public string Fields { get; set; }

        /// <summary>
        /// Specify this to localize the search to a specific item or folder. Omit to use the root.
        /// </summary>
        /// <value>The parent id.</value>
        [ApiMember(Name = "ParentId", Description = "Specify this to localize the search to a specific item or folder. Omit to use the root", IsRequired = false, DataType = "string", ParameterType = "query", Verb = "GET")]
        public string ParentId { get; set; }

        [ApiMember(Name = "EnableImages", Description = "Optional, include image information in output", IsRequired = false, DataType = "boolean", ParameterType = "query", Verb = "GET")]
        public bool? EnableImages { get; set; }

        [ApiMember(Name = "ImageTypeLimit", Description = "Optional, the max number of images to return, per image type", IsRequired = false, DataType = "int", ParameterType = "query", Verb = "GET")]
        public int? ImageTypeLimit { get; set; }

        [ApiMember(Name = "EnableImageTypes", Description = "Optional. The image types to include in the output.", IsRequired = false, DataType = "string", ParameterType = "query", Verb = "GET")]
        public string EnableImageTypes { get; set; }
    }

    [Route("/Shows/{Id}/Similar", "GET", Summary = "Finds tv shows similar to a given one.")]
    public class GetSimilarShows : BaseGetSimilarItemsFromItem
    {
    }

    [Route("/Shows/{Id}/Episodes", "GET", Summary = "Gets episodes for a tv season")]
    public class GetEpisodes : IReturn<ItemsResult>, IHasItemFields
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        [ApiMember(Name = "UserId", Description = "User Id", IsRequired = true, DataType = "string", ParameterType = "query", Verb = "GET")]
        public string UserId { get; set; }

        /// <summary>
        /// Fields to return within the items, in addition to basic information
        /// </summary>
        /// <value>The fields.</value>
        [ApiMember(Name = "Fields", Description = "Optional. Specify additional fields of information to return in the output. This allows multiple, comma delimeted. Options: Budget, Chapters, CriticRatingSummary, DateCreated, Genres, HomePageUrl, IndexOptions, MediaStreams, Overview, ParentId, Path, People, ProviderIds, PrimaryImageAspectRatio, Revenue, SortName, Studios, Taglines, TrailerUrls", IsRequired = false, DataType = "string", ParameterType = "query", Verb = "GET", AllowMultiple = true)]
        public string Fields { get; set; }

        [ApiMember(Name = "Id", Description = "The series id", IsRequired = true, DataType = "string", ParameterType = "query", Verb = "GET")]
        public string Id { get; set; }

        [ApiMember(Name = "Season", Description = "Optional filter by season number.", IsRequired = false, DataType = "string", ParameterType = "query", Verb = "GET")]
        public int? Season { get; set; }

        [ApiMember(Name = "SeasonId", Description = "Optional. Filter by season id", IsRequired = false, DataType = "string", ParameterType = "query", Verb = "GET")]
        public string SeasonId { get; set; }

        [ApiMember(Name = "IsMissing", Description = "Optional filter by items that are missing episodes or not.", IsRequired = false, DataType = "bool", ParameterType = "query", Verb = "GET")]
        public bool? IsMissing { get; set; }

        [ApiMember(Name = "IsVirtualUnaired", Description = "Optional filter by items that are virtual unaired episodes or not.", IsRequired = false, DataType = "bool", ParameterType = "query", Verb = "GET")]
        public bool? IsVirtualUnaired { get; set; }

        [ApiMember(Name = "AdjacentTo", Description = "Optional. Return items that are siblings of a supplied item.", IsRequired = false, DataType = "string", ParameterType = "query", Verb = "GET")]
        public string AdjacentTo { get; set; }

        [ApiMember(Name = "StartItemId", Description = "Optional. Skip through the list until a given item is found.", IsRequired = false, DataType = "string", ParameterType = "query", Verb = "GET")]
        public string StartItemId { get; set; }

        /// <summary>
        /// Skips over a given number of items within the results. Use for paging.
        /// </summary>
        /// <value>The start index.</value>
        [ApiMember(Name = "StartIndex", Description = "Optional. The record index to start at. All items with a lower index will be dropped from the results.", IsRequired = false, DataType = "int", ParameterType = "query", Verb = "GET")]
        public int? StartIndex { get; set; }

        /// <summary>
        /// The maximum number of items to return
        /// </summary>
        /// <value>The limit.</value>
        [ApiMember(Name = "Limit", Description = "Optional. The maximum number of records to return", IsRequired = false, DataType = "int", ParameterType = "query", Verb = "GET")]
        public int? Limit { get; set; }
    }

    [Route("/Shows/{Id}/Seasons", "GET", Summary = "Gets seasons for a tv series")]
    public class GetSeasons : IReturn<ItemsResult>, IHasItemFields
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        [ApiMember(Name = "UserId", Description = "User Id", IsRequired = true, DataType = "string", ParameterType = "query", Verb = "GET")]
        public string UserId { get; set; }

        /// <summary>
        /// Fields to return within the items, in addition to basic information
        /// </summary>
        /// <value>The fields.</value>
        [ApiMember(Name = "Fields", Description = "Optional. Specify additional fields of information to return in the output. This allows multiple, comma delimeted. Options: Budget, Chapters, CriticRatingSummary, DateCreated, Genres, HomePageUrl, IndexOptions, MediaStreams, Overview, ParentId, Path, People, ProviderIds, PrimaryImageAspectRatio, Revenue, SortName, Studios, Taglines, TrailerUrls", IsRequired = false, DataType = "string", ParameterType = "query", Verb = "GET", AllowMultiple = true)]
        public string Fields { get; set; }

        [ApiMember(Name = "Id", Description = "The series id", IsRequired = true, DataType = "string", ParameterType = "query", Verb = "GET")]
        public string Id { get; set; }

        [ApiMember(Name = "IsSpecialSeason", Description = "Optional. Filter by special season.", IsRequired = false, DataType = "bool", ParameterType = "query", Verb = "GET")]
        public bool? IsSpecialSeason { get; set; }

        [ApiMember(Name = "IsMissing", Description = "Optional filter by items that are missing episodes or not.", IsRequired = false, DataType = "bool", ParameterType = "query", Verb = "GET")]
        public bool? IsMissing { get; set; }

        [ApiMember(Name = "IsVirtualUnaired", Description = "Optional filter by items that are virtual unaired episodes or not.", IsRequired = false, DataType = "bool", ParameterType = "query", Verb = "GET")]
        public bool? IsVirtualUnaired { get; set; }

        [ApiMember(Name = "AdjacentTo", Description = "Optional. Return items that are siblings of a supplied item.", IsRequired = false, DataType = "string", ParameterType = "query", Verb = "GET")]
        public string AdjacentTo { get; set; }
    }

    /// <summary>
    /// Class TvShowsService
    /// </summary>
    [Authenticated]
    public class TvShowsService : BaseApiService
    {
        /// <summary>
        /// The _user manager
        /// </summary>
        private readonly IUserManager _userManager;

        /// <summary>
        /// The _user data repository
        /// </summary>
        private readonly IUserDataManager _userDataManager;
        /// <summary>
        /// The _library manager
        /// </summary>
        private readonly ILibraryManager _libraryManager;

        private readonly IItemRepository _itemRepo;
        private readonly IDtoService _dtoService;
        private readonly ITVSeriesManager _tvSeriesManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsService" /> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="userDataManager">The user data repository.</param>
        /// <param name="libraryManager">The library manager.</param>
        public TvShowsService(IUserManager userManager, IUserDataManager userDataManager, ILibraryManager libraryManager, IItemRepository itemRepo, IDtoService dtoService, ITVSeriesManager tvSeriesManager)
        {
            _userManager = userManager;
            _userDataManager = userDataManager;
            _libraryManager = libraryManager;
            _itemRepo = itemRepo;
            _dtoService = dtoService;
            _tvSeriesManager = tvSeriesManager;
        }

        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>System.Object.</returns>
        public object Get(GetSimilarShows request)
        {
            var dtoOptions = GetDtoOptions(request);

            var result = SimilarItemsHelper.GetSimilarItemsResult(dtoOptions, _userManager,
                _itemRepo,
                _libraryManager,
                _userDataManager,
                _dtoService,
                Logger,
                request, item => item is Series,
                SimilarItemsHelper.GetSimiliarityScore);

            return ToOptimizedSerializedResultUsingCache(result);
        }

        public object Get(GetUpcomingEpisodes request)
        {
            var user = _userManager.GetUserById(request.UserId);

            var minPremiereDate = DateTime.Now.Date.AddDays(-1).ToUniversalTime();

            var parentIds = string.IsNullOrWhiteSpace(request.ParentId) ? new string[] { } : new[] { request.ParentId };

            var itemsResult = _libraryManager.GetItemsResult(new InternalItemsQuery(user)
            {
                IncludeItemTypes = new[] { typeof(Episode).Name },
                SortBy = new[] { "PremiereDate", "AirTime", "SortName" },
                SortOrder = SortOrder.Ascending,
                MinPremiereDate = minPremiereDate,
                StartIndex = request.StartIndex,
                Limit = request.Limit

            }, parentIds);

            var options = GetDtoOptions(request);

            var returnItems = _dtoService.GetBaseItemDtos(itemsResult.Items, options, user).ToArray();

            var result = new ItemsResult
            {
                TotalRecordCount = itemsResult.TotalRecordCount,
                Items = returnItems
            };

            return ToOptimizedSerializedResultUsingCache(result);
        }

        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>System.Object.</returns>
        public object Get(GetNextUpEpisodes request)
        {
            var result = _tvSeriesManager.GetNextUp(new NextUpQuery
            {
                Limit = request.Limit,
                ParentId = request.ParentId,
                SeriesId = request.SeriesId,
                StartIndex = request.StartIndex,
                UserId = request.UserId
            });

            var user = _userManager.GetUserById(request.UserId);

            var options = GetDtoOptions(request);

            var returnItems = _dtoService.GetBaseItemDtos(result.Items, options, user).ToArray();

            return ToOptimizedSerializedResultUsingCache(new ItemsResult
            {
                TotalRecordCount = result.TotalRecordCount,
                Items = returnItems
            });
        }

        /// <summary>
        /// Applies the paging.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="limit">The limit.</param>
        /// <returns>IEnumerable{BaseItem}.</returns>
        private IEnumerable<BaseItem> ApplyPaging(IEnumerable<BaseItem> items, int? startIndex, int? limit)
        {
            // Start at
            if (startIndex.HasValue)
            {
                items = items.Skip(startIndex.Value);
            }

            // Return limit
            if (limit.HasValue)
            {
                items = items.Take(limit.Value);
            }

            return items;
        }

        public object Get(GetSeasons request)
        {
            var user = _userManager.GetUserById(request.UserId);

            var series = _libraryManager.GetItemById(request.Id) as Series;

            if (series == null)
            {
                throw new ResourceNotFoundException("No series exists with Id " + request.Id);
            }

            var seasons = series.GetSeasons(user);

            if (request.IsSpecialSeason.HasValue)
            {
                var val = request.IsSpecialSeason.Value;

                seasons = seasons.Where(i => i.IsSpecialSeason == val);
            }

            seasons = FilterVirtualSeasons(request, seasons);

            // This must be the last filter
            if (!string.IsNullOrEmpty(request.AdjacentTo))
            {
                seasons = UserViewBuilder.FilterForAdjacency(seasons, request.AdjacentTo)
                    .Cast<Season>();
            }

            var dtoOptions = GetDtoOptions(request);

            var returnItems = _dtoService.GetBaseItemDtos(seasons, dtoOptions, user)
                .ToArray();

            return new ItemsResult
            {
                TotalRecordCount = returnItems.Length,
                Items = returnItems
            };
        }

        private IEnumerable<Season> FilterVirtualSeasons(GetSeasons request, IEnumerable<Season> items)
        {
            if (request.IsMissing.HasValue && request.IsVirtualUnaired.HasValue)
            {
                var isMissing = request.IsMissing.Value;
                var isVirtualUnaired = request.IsVirtualUnaired.Value;

                if (!isMissing && !isVirtualUnaired)
                {
                    return items.Where(i => !i.IsMissingOrVirtualUnaired);
                }
            }

            if (request.IsMissing.HasValue)
            {
                var val = request.IsMissing.Value;
                items = items.Where(i => i.IsMissingSeason == val);
            }

            if (request.IsVirtualUnaired.HasValue)
            {
                var val = request.IsVirtualUnaired.Value;
                items = items.Where(i => i.IsVirtualUnaired == val);
            }

            return items;
        }

        public object Get(GetEpisodes request)
        {
            var user = _userManager.GetUserById(request.UserId);

            IEnumerable<Episode> episodes;

            if (!string.IsNullOrWhiteSpace(request.SeasonId))
            {
                var season = _libraryManager.GetItemById(new Guid(request.SeasonId)) as Season;

                if (season == null)
                {
                    throw new ResourceNotFoundException("No season exists with Id " + request.SeasonId);
                }

                episodes = season.GetEpisodes(user);
            }
            else if (request.Season.HasValue)
            {
                var series = _libraryManager.GetItemById(request.Id) as Series;

                if (series == null)
                {
                    throw new ResourceNotFoundException("No series exists with Id " + request.Id);
                }

                episodes = series.GetEpisodes(user, request.Season.Value);
            }
            else
            {
                var series = _libraryManager.GetItemById(request.Id) as Series;

                if (series == null)
                {
                    throw new ResourceNotFoundException("No series exists with Id " + request.Id);
                }

                episodes = series.GetEpisodes(user);
            }

            // Filter after the fact in case the ui doesn't want them
            if (request.IsMissing.HasValue)
            {
                var val = request.IsMissing.Value;
                episodes = episodes.Where(i => i.IsMissingEpisode == val);
            }

            // Filter after the fact in case the ui doesn't want them
            if (request.IsVirtualUnaired.HasValue)
            {
                var val = request.IsVirtualUnaired.Value;
                episodes = episodes.Where(i => i.IsVirtualUnaired == val);
            }

            if (!string.IsNullOrWhiteSpace(request.StartItemId))
            {
                episodes = episodes.SkipWhile(i => !string.Equals(i.Id.ToString("N"), request.StartItemId, StringComparison.OrdinalIgnoreCase));
            }

            IEnumerable<BaseItem> returnItems = episodes;

            // This must be the last filter
            if (!string.IsNullOrEmpty(request.AdjacentTo))
            {
                returnItems = UserViewBuilder.FilterForAdjacency(returnItems, request.AdjacentTo);
            }

            var returnList = _libraryManager.ReplaceVideosWithPrimaryVersions(returnItems)
                .ToList();

            var pagedItems = ApplyPaging(returnList, request.StartIndex, request.Limit);

            var dtoOptions = GetDtoOptions(request);

            var dtos = _dtoService.GetBaseItemDtos(pagedItems, dtoOptions, user)
                .ToArray();

            return new ItemsResult
            {
                TotalRecordCount = returnList.Count,
                Items = dtos
            };
        }
    }
}
