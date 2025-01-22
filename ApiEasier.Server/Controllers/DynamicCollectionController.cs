using ApiEasier.Server.Db;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiEasier.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamicCollectionController : ControllerBase
    {
        private readonly MongoDbContext _dbContext;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DynamicCollectionController"/>.
        /// </summary>
        /// <param name="dbContext">Контекст MongoDB.</param>
        public DynamicCollectionController(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // POST api/DynamicCollection/AddToCollection/{collectionName}
        [HttpPost("{collectionName}")]
        public async Task<IActionResult> AddToCollection(string collectionName, [FromBody] object jsonData)
        {
            try
            {
                var collection = _dbContext.GetCollection<BsonDocument>(collectionName);
                var bsonDocument = BsonDocument.Parse(jsonData.ToString());
                await collection.InsertOneAsync(bsonDocument);
                return Ok(new { message = "Документ успешно добавлен!" });
            }
            catch (FormatException ex)
            {
                return BadRequest($"Неверный формат JSON: {ex.Message}");
            }
            catch (MongoCommandException ex)
            {
                return BadRequest($"Ошибка при выполнении команды MongoDB: {ex.Message}");
            }
            catch (MongoException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка MongoDB: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Произошла ошибка при добавлении документа: {ex.Message}");
            }
        }
    }
}
