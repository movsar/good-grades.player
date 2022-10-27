using Content_Manager.Services;
using Content_Manager.Stores;
using Data;
using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Content_Manager.Models
{
    public class ContentModel
    {
        public event Action ContentModelInitialized;
        private Storage _storage;
        private readonly FileService _fileService;
        private readonly ILogger<ContentModel> _logger;

        public ContentModel(Storage storage, FileService fileService, ILogger<ContentModel> logger)
        {
            _storage = storage;
            _fileService = fileService;
            _logger = logger;
            _storage.DatabaseInitialized += DatabaseInitialized;
        }

        public void DatabaseInitialized(string databasePath)
        {
            _fileService.SetResourceString("lastOpenedDatabasePath", databasePath);

            ContentModelInitialized?.Invoke();
        }

        private IGeneralRepository SelectRepository<TModel>()
        {
            var t = typeof(TModel);
            switch (t)
            {
                case var _ when t.IsAssignableTo(typeof(IDbMeta)):
                case var _ when t.IsAssignableFrom(typeof(IDbMeta)):
                    return _storage.DbMetaRepository;

                case var _ when t.IsAssignableTo(typeof(ISegment)):
                case var _ when t.IsAssignableFrom(typeof(ISegment)):
                    return _storage.SegmentsRepository;

                case var _ when t.IsAssignableTo(typeof(IReadingMaterial)):
                case var _ when t.IsAssignableFrom(typeof(IReadingMaterial)):
                    return _storage.ReadingMaterialsRepository;

                case var _ when t.IsAssignableTo(typeof(IListeningMaterial)):
                case var _ when t.IsAssignableFrom(typeof(IListeningMaterial)):
                    return _storage.ListeningMaterialsRepository;

                case var _ when t.IsAssignableTo(typeof(ITestingQuestion)):
                case var _ when t.IsAssignableFrom(typeof(ITestingQuestion)):
                    return _storage.QuestionsRepository;

                case var _ when t.IsAssignableTo(typeof(IQuizItem)):
                case var _ when t.IsAssignableFrom(typeof(IQuizItem)):
                    return _storage.QuizItemsRepository;

                case var _ when t.IsAssignableTo(typeof(ICelebrityWordsQuiz)):
                case var _ when t.IsAssignableFrom(typeof(ICelebrityWordsQuiz)):
                    return _storage.CwqRepository;

                case var _ when t.IsAssignableTo(typeof(IProverbSelectionQuiz)):
                case var _ when t.IsAssignableFrom(typeof(IProverbSelectionQuiz)):
                    return _storage.PsqRepository;

                case var _ when t.IsAssignableTo(typeof(IProverbBuilderQuiz)):
                case var _ when t.IsAssignableFrom(typeof(IProverbBuilderQuiz)):
                    return _storage.PbqRepository;

                case var _ when t.IsAssignableTo(typeof(IGapFillerQuiz)):
                case var _ when t.IsAssignableFrom(typeof(IGapFillerQuiz)):
                    return _storage.GfqRepository;

                case var _ when t.IsAssignableTo(typeof(ITestingQuiz)):
                case var _ when t.IsAssignableFrom(typeof(ITestingQuiz)):
                    return _storage.TsqRepository;

                default:
                    throw new Exception();
            }
        }
        public void DeleteItems<TModel>(IEnumerable<IModelBase> items) where TModel : IModelBase
        {
            var repository = SelectRepository<TModel>();
            foreach (var item in items)
            {
                repository.Delete(item);
            }
        }
        public void DeleteItem<TModel>(IModelBase item) where TModel : IModelBase
        {
            try
            {
                SelectRepository<TModel>().Delete(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace, ex.InnerException);
            }
        }

        public void UpdateItem<TModel>(IModelBase item) where TModel : IModelBase
        {
            try
            {
                SelectRepository<TModel>().Update(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace, ex.InnerException);
            }
        }

        public void AddItem<TModel>(ref IModelBase item) where TModel : IModelBase
        {
            try
            {


                SelectRepository<TModel>().Add(ref item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace, ex.InnerException);
            }
        }

        public TModel GetById<TModel>(string id) where TModel : IModelBase
        {
            try
            {
                return SelectRepository<TModel>().GetById<TModel>(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace, ex.InnerException);
                throw;
            }
        }

        public IEnumerable<TModel> GetAll<TModel>() where TModel : IModelBase
        {
            try
            {
                return SelectRepository<TModel>().GetAll<TModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace, ex.InnerException);
                return Enumerable.Empty<TModel>();
            }
        }

        internal void CreateDatabase(string filePath)
        {
            try
            {
                _storage.CreateDatabase(filePath);
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
    }
}
