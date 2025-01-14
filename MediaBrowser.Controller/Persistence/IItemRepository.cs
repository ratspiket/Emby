﻿using MediaBrowser.Controller.Entities;
using MediaBrowser.Model.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Model.Querying;

namespace MediaBrowser.Controller.Persistence
{
    /// <summary>
    /// Provides an interface to implement an Item repository
    /// </summary>
    public interface IItemRepository : IRepository
    {
        /// <summary>
        /// Opens the connection to the repository
        /// </summary>
        /// <returns>Task.</returns>
        Task Initialize();

        /// <summary>
        /// Saves an item
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        Task SaveItem(BaseItem item, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        Task DeleteItem(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the critic reviews.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <returns>Task{IEnumerable{ItemReview}}.</returns>
        IEnumerable<ItemReview> GetCriticReviews(Guid itemId);

        /// <summary>
        /// Gets the children items.
        /// </summary>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>IEnumerable&lt;BaseItem&gt;.</returns>
        IEnumerable<BaseItem> GetChildrenItems(Guid parentId);

        /// <summary>
        /// Saves the critic reviews.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <param name="criticReviews">The critic reviews.</param>
        /// <returns>Task.</returns>
        Task SaveCriticReviews(Guid itemId, IEnumerable<ItemReview> criticReviews);

        /// <summary>
        /// Saves the items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        Task SaveItems(IEnumerable<BaseItem> items, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves the item.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>BaseItem.</returns>
        BaseItem RetrieveItem(Guid id);

        /// <summary>
        /// Gets chapters for an item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<ChapterInfo> GetChapters(Guid id);

        /// <summary>
        /// Gets a single chapter for an item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        ChapterInfo GetChapter(Guid id, int index);

        /// <summary>
        /// Saves the chapters.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="chapters">The chapters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        Task SaveChapters(Guid id, IEnumerable<ChapterInfo> chapters, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <param name="parentId">The parent id.</param>
        /// <returns>IEnumerable{ChildDefinition}.</returns>
        IEnumerable<Guid> GetChildren(Guid parentId);

        /// <summary>
        /// Saves the children.
        /// </summary>
        /// <param name="parentId">The parent id.</param>
        /// <param name="children">The children.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        Task SaveChildren(Guid parentId, IEnumerable<Guid> children, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the media streams.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>IEnumerable{MediaStream}.</returns>
        IEnumerable<MediaStream> GetMediaStreams(MediaStreamQuery query);

        /// <summary>
        /// Saves the media streams.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="streams">The streams.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        Task SaveMediaStreams(Guid id, IEnumerable<MediaStream> streams, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the item ids.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>IEnumerable&lt;Guid&gt;.</returns>
        QueryResult<Guid> GetItemIds(InternalItemsQuery query);
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>QueryResult&lt;BaseItem&gt;.</returns>
        QueryResult<BaseItem> GetItems(InternalItemsQuery query);

        /// <summary>
        /// Gets the item ids list.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>List&lt;Guid&gt;.</returns>
        List<Guid> GetItemIdsList(InternalItemsQuery query);

        /// <summary>
        /// Gets the people.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>List&lt;PersonInfo&gt;.</returns>
        List<PersonInfo> GetPeople(InternalPeopleQuery query);

        /// <summary>
        /// Updates the people.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="people">The people.</param>
        /// <returns>Task.</returns>
        Task UpdatePeople(Guid itemId, List<PersonInfo> people);

        /// <summary>
        /// Gets the people names.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        List<string> GetPeopleNames(InternalPeopleQuery query);

        /// <summary>
        /// Gets the item ids with path.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>QueryResult&lt;Tuple&lt;Guid, System.String&gt;&gt;.</returns>
        QueryResult<Tuple<Guid, string>> GetItemIdsWithPath(InternalItemsQuery query);

        /// <summary>
        /// Gets the item list.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>List&lt;BaseItem&gt;.</returns>
        IEnumerable<BaseItem> GetItemList(InternalItemsQuery query);

        /// <summary>
        /// Updates the inherited values.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        Task UpdateInheritedValues(CancellationToken cancellationToken);
    }
}

