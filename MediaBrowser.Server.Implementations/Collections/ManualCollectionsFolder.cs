﻿using MediaBrowser.Controller.Entities;
using System.Linq;

namespace MediaBrowser.Server.Implementations.Collections
{
    public class ManualCollectionsFolder : BasePluginFolder, IHiddenFromDisplay
    {
        public ManualCollectionsFolder()
        {
            Name = "Collections";
            DisplayMediaType = "CollectionFolder";
        }

        public override bool IsHidden
        {
            get
            {
                return true;
            }
        }

        public bool IsHiddenFromUser(User user)
        {
            return !user.Configuration.DisplayCollectionsView;
        }

        public override string CollectionType
        {
            get { return Model.Entities.CollectionType.BoxSets; }
        }

        public override string GetClientTypeName()
        {
            return typeof(CollectionFolder).Name;
        }
    }
}