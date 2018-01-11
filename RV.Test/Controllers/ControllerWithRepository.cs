using Microsoft.AspNetCore.Mvc;
using RV.Test.Infra;
using RV.Test.Infra.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RV.Test.Web.Controllers
{
    public class ControllerWithRepository<T> : Controller where T : class
    {
        protected IRepository<T> _repository;

        public ControllerWithRepository(IRepository<T> repository)
        {
            _repository = repository;
        }
    }
}
