﻿using Data;
using Data.Entities;
using Data.Interfaces;
using Realms;
using System;
using System.Linq;
using System.Reflection;

namespace Content_Manager.Stores
{
    /*
     * This is needed for cross-window UI statefulness and reactivity, 
     * to know which object is currently selected, to process events from other windows etc.
     * Otherwise Database access will be done in ViewModels directly
     */
    public class ContentStore
    {

        #region Events, Properties and Fields
        private readonly Storage _storage;

        private SegmentEntity? _selectedSegment;
        public SegmentEntity? SelectedSegment
        {
            get
            {
                return _selectedSegment;
            }
            internal set
            {
                _selectedSegment = value;
                SelectedSegmentChanged?.Invoke(value);
            }
        }

        public Realm Database => _storage.Database;

        public event Action<SegmentEntity>? SelectedSegmentChanged;
        public event Action<string, IEntityBase>? ItemAdded;
        public event Action<string, IEntityBase>? ItemUpdated;
        public event Action<string, IEntityBase>? ItemDeleted;
        #endregion

        public ContentStore(Storage storage)
        {
            _storage = storage;
            SelectedSegment = Database.All<SegmentEntity>().FirstOrDefault();
        }

        internal void OpenDatabase(string filePath)
        {
            _storage.SetDatabaseConfig(filePath);
        }
        internal void CreateDatabase(string filePath)
        {
            string? appVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
            _storage.CreateDatabase(filePath, appVersion);
        }

        internal void ImportDatabase(string filePath)
        {
            _storage.ImportDatabase(filePath);
        }
    }
}
