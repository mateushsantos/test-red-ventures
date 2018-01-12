using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RV.Test.Web.Controllers
{
    [Route("[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public int Get()
        {
            return 1;
        }

        [HttpGet("{id}")]
        public int Get(int id)
        {
            return id;
        }
    }
}
