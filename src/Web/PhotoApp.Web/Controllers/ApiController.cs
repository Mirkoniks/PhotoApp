using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Controllers
{
    public class ApiController : Controller
    { 
        public class Model
        {
            public string Name { get; set; }

            public int Count { get; set; }
        }

        [HttpGet]
        public IActionResult ResultTest()
        {
            List<Model> list = new List<Model>();

            for (int i = 0; i < 1000; i++)
            {
                Model model = new Model();
                model.Count = i;
                model.Name = "A" + i;
                list.Add(model);
            }

            return Json(list);
        }
    }
}
