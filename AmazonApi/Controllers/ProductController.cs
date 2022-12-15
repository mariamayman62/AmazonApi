using ITI.ElectroDev.Models;
using ITI.Library.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AmazonApi.Controllers
{
    [ApiController]
   // [Route("[controller]")]
    [Route("[controller]/[action]")]
    public class ProductController : Controller
    {
        private Context db;
        public ProductController(Context _db)
        {
            db = _db;
        }

        [HttpGet]
        public ResultViewModel Get()
        {
            var products = db.Product.ToList();
            return new ResultViewModel()
            {
                Success = true,
                Message = "Products List",
                Data = new { Products = products }
            };

        }
        [HttpGet("{id}")]
        public ResultViewModel Get(int id)
        {
            var products = db.Product.FirstOrDefault(i => i.Id == id);
            return new ResultViewModel()
            {
                Success = true,
                Message = "",
                Data = new { Product = products }
            };


        }
        [HttpGet]
        public string getBrandNameByItemName(string itemName)
        {
            string brandName = db.Product.Where(i => i.Name == itemName).Select(c => c.Brand.Name).FirstOrDefault();
            return brandName;
        }


    }
}
