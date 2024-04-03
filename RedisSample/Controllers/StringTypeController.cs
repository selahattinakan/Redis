using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RedisSample.Services;
using StackExchange.Redis;

namespace RedisSample.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly StackExchangeRedisService _redisService;
        private readonly IDatabase db;

        public StringTypeController(StackExchangeRedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }
        public IActionResult Index()
        {
            db.StringSet("name", "Selahattin");
            db.StringSet("surname", "Akan");
            db.StringSet("visitor", 100);

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/test_image.jpg");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            db.StringSet("imageByte", imageByte);

            return View();
        }

        public IActionResult Show()
        {
            var nameValue = db.StringGet("name");
            var surNameValue = db.StringGet("surname");
            if (!nameValue.HasValue && !surNameValue.HasValue) return Content("name cached not found");

            db.StringIncrement("visitor");
            var visitorValue = db.StringGet("visitor");

            string content = $"name:{nameValue} surname:{surNameValue}\n" +
                $"visitor:{visitorValue}";

            return Content(content);
        }

        public IActionResult ShowImageCached()
        {
            Byte[] imageByte = db.StringGet("imageByte");
            return File(imageByte, "image/jpg");
        }
    }
}
