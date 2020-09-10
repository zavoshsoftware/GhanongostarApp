using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using Helper;
using Models;
using Models.Input;

namespace API.Controllers
{
    public class TempController : Infrastructure.BaseControllerWithUnitOfWork
    {
        StatusManagement status = new StatusManagement();

        [Route("version/getios")]
        public IosTempViewModel GetVersionForAppStore()
        {

            string[] versions = new string[]
            {
               "1.0.2"
            };

            IosTempViewModel ios = new IosTempViewModel()
            {
                Result = versions,
                Status = status.ReturnStatus(0, "vesions", true)
            };
            return ios;
        }
    }
}
