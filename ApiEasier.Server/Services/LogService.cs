using ApiEasier.Server.DB;
using ApiEasier.Server.Interfaces;
using ApiEasier.Server.Models;
using MongoDB.Bson;

namespace ApiEasier.Server.Services
{
    public class LogService
    {
        private readonly IDynamicCollectionService _collectionService;
        public LogService(IDynamicCollectionService collectionService) 
        {
            _collectionService = collectionService;
        }
        public bool SaveLog(RequestLog log) 
        {
            _collectionService.AddDocToCollectionAsync("logs", log.ToJson());
            return true;
        }
    }
}
