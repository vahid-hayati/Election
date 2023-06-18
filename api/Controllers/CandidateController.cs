using api.Models;
using api.Settings;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CandidateController : ControllerBase
{
    private readonly IMongoCollection<President> _collection;

    public CandidateController(IMongoClient client, IMongoDbSettings dbSettings)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<President>("candidates");
    }

    [HttpPost("register")]
    public ActionResult<President> Create(President userInput)
    {
        President candidate = _collection.Find<President>(doc => doc.NationalCode == userInput.NationalCode.Trim()).FirstOrDefault();

        if (candidate == null)
        {
            candidate = new President(
                Id: null,
                Name: userInput.Name.Trim(),
                LastName: userInput.LastName.Trim(),
                NationalCode: userInput.NationalCode.Trim(),
                Education: userInput.Education.Trim(),
                DegreeOfEEducation: userInput.DegreeOfEEducation.Trim(),
                Age: userInput.Age,
                CompleteAddress: new CompleteAddress(
                    Street: userInput.CompleteAddress.Street.Trim(),
                    City: userInput.CompleteAddress.City.Trim(),
                    State: userInput.CompleteAddress.State.Trim(),
                    Country: userInput.CompleteAddress.Country.Trim()
                )
            );

            _collection.InsertOne(candidate);

            return candidate;
        }

        return BadRequest("This candidate is not qualified to be a candidate.");
    }

    [HttpGet("get-national-code/{nationalCode}")]
    public ActionResult<President> GetByNationalCode(string nationalCode)
    {
        President candidate = _collection.Find<President>(doc => doc.NationalCode == nationalCode.Trim()).FirstOrDefault();

         if (candidate == null)
        {
            return NotFound("The national code is not valid!");
        }

        return candidate;
    }

    [HttpGet]
    public ActionResult<List<President>> GetAll()
    {
        List<President> candidate = _collection.Find<President>(new BsonDocument()).ToList();

          if (!candidate.Any())
        {
            return Ok("Oh!! The list of candidate is empty");
        }

        return candidate;
    }

    [HttpPut("update/{PresidentId}")] 
    public ActionResult<UpdateResult> UpdatePresidentById(string PresidentId, President PresidentIn)
    {
        var updatedPresident = Builders<President>.Update
        .Set(doc => doc.Education, PresidentIn.Education.Trim())
        .Set(doc => doc.DegreeOfEEducation, PresidentIn.DegreeOfEEducation);

        return _collection.UpdateOne<President>(doc => doc.Id == PresidentId, updatedPresident);
    }

    [HttpDelete("delete/{PresidentId}")]
     public ActionResult<DeleteResult> Delete(string PresidentId)
    {
        return _collection.DeleteOne<President>(doc => doc.Id == PresidentId);
    }
}