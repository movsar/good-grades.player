﻿using Data;
using Data.Entities;
using Data.Interfaces;
using Data.Services;
using Shared.Services;
using System;
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
        private readonly SettingsService _fileService;
        public DataContext DbContext => _storage.DbContext;

        private Segment? _selectedSegment;
        public Segment? SelectedSegment
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


        public event Action? CurrentDatabaseChanged;
        public event Action<Segment>? SelectedSegmentChanged;

        public event Action<IEntityBase>? ItemAdded;
        public event Action<IEntityBase>? ItemUpdated;
        public event Action<IEntityBase>? ItemDeleted;
        #endregion

        public ContentStore(Storage storage, SettingsService appSettings)
        {
            _storage = storage;
            _fileService = appSettings;
        }

        internal void OpenDatabase(string filePath)
        {
            try
            {
                _storage.SetDatabaseConfig(filePath);
                _fileService.SetValue("lastOpenedDatabasePath", filePath);
                CurrentDatabaseChanged?.Invoke();
            }
            catch (Exception ex)
            {
                ExceptionService.HandleError(ex, "Произошла ошибка с открытием Базы Данных предыдущей сессии");
            }
        }
        internal void CreateDatabase(string filePath)
        {
            string? appVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
            _storage.CreateDatabase(filePath, appVersion);
            OpenDatabase(filePath);
        }

        internal void ImportDatabase(string filePath)
        {
            _storage.ImportDatabase(filePath);
        }

        internal void RaiseItemDeletedEvent(IEntityBase entity)
        {
            ItemDeleted?.Invoke(entity);
        }
        internal void RaiseItemUpdatedEvent(IEntityBase entity)
        {
            ItemUpdated?.Invoke(entity);
        }
        internal void RaiseItemAddedEvent(IEntityBase entity)
        {
            ItemAdded?.Invoke(entity);
        }
    }
}
