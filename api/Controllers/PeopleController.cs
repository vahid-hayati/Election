using api.Models;
using api.Settings;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeopleController : ControllerBase
{
    private readonly IMongoCollection<PresidentialElectionForm> _collection;

    public PeopleController(IMongoClient client, IMongoDbSettings dbSettings)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<PresidentialElectionForm>("peoples");
    }

    [HttpPost("add")]
    public ActionResult<PresidentialElectionForm> Create(PresidentialElectionForm userInput)
    {
        PresidentialElectionForm user = _collection.Find<PresidentialElectionForm>(doc => doc.NationalCode == userInput.NationalCode.Trim()).FirstOrDefault();

        if (user == null)
        {
            user = new PresidentialElectionForm(
                Id: null,
                Name: userInput.Name.Trim(),
                LastName: userInput.LastName.Trim(),
                NationalCode: userInput.NationalCode.Trim(),
                PresidentId: userInput.PresidentId,
                NationalCodePresident: userInput.NationalCodePresident.Trim(),
                CompleteAddress: new CompleteAddress(
                    Street: userInput.CompleteAddress.Street.Trim(),
                    City: userInput.CompleteAddress.City.Trim(),
                    State: userInput.CompleteAddress.State.Trim(),
                    Country: userInput.CompleteAddress.Country.Trim()
                )
            );

            _collection.InsertOne(user);

            return user;
        }

        return BadRequest("Votes have already been registered with this national code.");
    }
    
    [HttpGet("get-national-code/{nationalCode}")]
    public ActionResult<PresidentialElectionForm> GetByNationalCode(string nationalCode)
    {
        PresidentialElectionForm user = _collection.Find<PresidentialElectionForm>(doc => doc.NationalCode == nationalCode.Trim()).FirstOrDefault();

        if (user == null)
        {
            return NotFound("The national code is not valid!");
        }

        return user;
    }

    [HttpGet]
    public ActionResult<List<PresidentialElectionForm>> GetAll()
    {
         List<PresidentialElectionForm> users = _collection.Find<PresidentialElectionForm>(new BsonDocument()).ToList();

          if (!users.Any())
        {
            return Ok("Oh!! The list of users is empty");
        }

        return users;
    }

    [HttpGet("get-by-president-id/{presidentId}")]
    public ActionResult<IEnumerable<PresidentialElectionForm>> GetByPresidentId(string presidentId)
    {
      List<PresidentialElectionForm> filteredVotes = _collection.Find<PresidentialElectionForm>(doc => doc.PresidentId == presidentId).ToList<PresidentialElectionForm>();  

      if (!filteredVotes.Any())
      {
        return NoContent();
      }

      return filteredVotes;
    } 
}