using Microsoft.AspNetCore.Mvc;
using System.Reflection;
namespace FirstExam.Controllers

{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PetsController : ControllerBase
    {
        private static readonly List<Pet> _pets = new()
{
    new Pet { Id = Guid.NewGuid(), OwnerId = Guid.NewGuid(), Name = "Preto", Species = "dog", Breed = "Sharpei", Birthdate = new DateTime(2020, 5, 12), Sex = "male", WeigthKg = 22.5m },
    new Pet { Id = Guid.NewGuid(), OwnerId = Guid.NewGuid(), Name = "Mimi", Species = "cat", Breed = "Siamese", Birthdate = new DateTime(2019, 8, 20), Sex = "female", WeigthKg = 4.28m },
    new Pet { Id = Guid.NewGuid(), OwnerId = Guid.NewGuid(), Name = "Kiko", Species = "bird", Breed = "Parrot", Birthdate = new DateTime(2021, 1, 15), Sex = "male", WeigthKg = 0.3m },
    new Pet { Id = Guid.NewGuid(), OwnerId = Guid.NewGuid(), Name = "Lola", Species = "dog", Breed = "Cooker", Birthdate = new DateTime(2018, 11, 5), Sex = "female", WeigthKg = 28.0m },
    new Pet { Id = Guid.NewGuid(), OwnerId = Guid.NewGuid(), Name = "Spike", Species = "reptile", Breed = "Iguana", Birthdate = new DateTime(2022, 3, 30), Sex = "male", WeigthKg = 5.1m }
};

    }
}
