using Core.Infrastructure.MongoDb.Implementation;
using Core.Infrastructure.MongoDb.Interface;
using Core.Infrastructure.MongoDb.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IMongoRepository<Users> _mongoRepository;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IMongoRepository<Users> mongoRepository)
    {
        _logger = logger;
        _mongoRepository = mongoRepository;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        //var a = Task.Run(async () => await _mongoRepository?.FindOneAsync(x => x.Role == "_mongoRepository")!);
        //var response = await a;
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost(Name = "GetWeatherForecast")]
    public async Task<bool> Post()
    {
        var a = await _mongoRepository?.FindOneAsync(x => x.Role == "Admin")!;
        return true;
    }
}

[BsonCollection("users")]
public class Users : Document
{
    [Required]
    [BsonElement("userId")]
    public string UserId { get; set; } = null!;

    [Required]
    [BsonElement("fullName")]
    public string FullName { get; set; } = null!;

    [Required]
    [BsonElement("userAlias")]
    public string UserAlias { get; set; } = null!;

    [Required]
    [BsonElement("emailAddress")]
    public string EmailAddress { get; set; } = null!;

    [Required]
    [BsonElement("role")]
    public string Role { get; set; } = null!;

    [Required]
    [BsonElement("globalEmployeeId")]
    public string GlobalEmployeeId { get; set; } = null!;

    [Required]
    [BsonElement("lastLoginDate")]
    public DateTime LastLoginDate { get; set; } = DateTime.MinValue;

    [Required]
    [BsonElement("lastLoginRole")]
    public string LastLoginRole { get; set; } = null!;
}