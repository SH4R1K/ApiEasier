using ApiEasier.Server.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ApiEasier.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamicCollectionController : ControllerBase
    {
        private readonly MongoDBContext _dbContext;

        public DynamicCollectionController(MongoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("{collectionName}")]
        public async Task<IActionResult> AddToCollecntion(string collectionName, [FromBody] object jsonData)
        {
            try
            {
                var collecntion = _dbContext.GetCollection<BsonDocument>(collectionName);
                var bsonDocument = BsonDocument.Parse(jsonData.ToString());
                await collecntion.InsertOneAsync(bsonDocument);
                return Ok(new { message = "Document added succsessfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
