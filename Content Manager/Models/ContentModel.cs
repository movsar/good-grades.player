using Content_Manager.Services;
using Data;
using Microsoft.Extensions.Logging;
using Realms;
using System;
using System.Reflection;

namespace Content_Manager.Models
{
    public class ContentModel
    {
        public event Action DatabaseInitialized;
        public event Action DatabaseUpdated;

        private Storage _storage;
        private readonly FileService _fileService;
        private readonly ILogger<ContentModel> _logger;
        public Realm Database => Realm.GetInstance(_storage.DbConfig);
        public ContentModel(Storage storage, FileService fileService, ILogger<ContentModel> logger)
        {
            _storage = storage;
            _fileService = fileService;
            _logger = logger;
            _storage.DatabaseInitialized += OnDatabaseInitialized;
            _storage.DatabaseUpdated += OnDatabaseUpdated;
        }

        private void OnDatabaseUpdated()
        {
            DatabaseUpdated?.Invoke();
        }

        public void OnDatabaseInitialized(string databasePath)
        {
            _fileService.SetResourceString("lastOpenedDatabasePath", databasePath);

            DatabaseInitialized?.Invoke();
        }

        internal void CreateDatabase(string filePath)
        {
            // This must be in Content Manager project, otherwise the version will be 
            // taken from Data project, which is not what we need.
            string? appVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();

            try
            {
                _storage.CreateDatabase(filePath, appVersion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace, ex.InnerException);
            }
        }

        internal void OpenDatabase(string filePath)
        {
            try
            {
                _storage.OpenDatabase(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace, ex.InnerException);
            }
        }

        internal void ImportDatabase(string filePath)
        {
            _storage.ImportDatabase(filePath);
        }
    }
}
