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
        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);
        }

        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src;
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) return src;

            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderByDescending(x => prop.GetValue(x))
                : src.OrderBy(x => prop.GetValue(x));
        }


    }
}
