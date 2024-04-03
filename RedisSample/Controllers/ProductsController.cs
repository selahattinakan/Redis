using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RedisSample.Models;
using System.Text;

namespace RedisSample.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public IActionResult Index()
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            _distributedCache.SetString("name", "Selahattin", options);

            Product product = new Product()
            {
                Id = 1,
                Name = "Test",
                Price = 100
            };
            string jsonProduct = JsonConvert.SerializeObject(product);
            _distributedCache.SetString("product:1", jsonProduct, options);

            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);
            _distributedCache.Set("productByte:1", byteProduct, options);

            return View();
        }

        public IActionResult Show()
        {
            string? name = _distributedCache.GetString("name");

            string? jsonProduct = _distributedCache.GetString("product:1");
            Product product = JsonConvert.DeserializeObject<Product>(jsonProduct);

            Byte[] byteProduct = _distributedCache.Get("productByte:1");
            string? jsonProductByte = Encoding.UTF8.GetString(byteProduct);
            Product productByte = JsonConvert.DeserializeObject<Product>(jsonProductByte);

            string content = $"name:{name} \n" +
                $"product=> id:{product.Id} name:{product.Name} price:{product.Price} \n" +
                $"productByte=>id:{productByte.Id} name:{productByte.Name} price:{productByte.Price} \n";
            return Content(content);
        }

        public IActionResult Delete()
        {
            _distributedCache.Remove("name");
            return Content("name deleted");
        }

        public IActionResult ImageCache()
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/test_image.jpg");

            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("image", imageByte, options);

            return Content("image cached");
        }

        public IActionResult ShowImageCached()
        {
            Byte[] imageByte = _distributedCache.Get("image");
            return File(imageByte, "image/jpg");
        }
    }
}
