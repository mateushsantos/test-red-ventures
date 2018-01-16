using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RV.Test.Web.Extensions.Controllers
{
    public static class ControllerExtensions
    {
        public static IEnumerable<string> GetModelStateErrors(this Controller controller)
        {
            var errors = new List<string>();

            foreach (var value in controller.ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }

            return errors;
        }
    }
}
