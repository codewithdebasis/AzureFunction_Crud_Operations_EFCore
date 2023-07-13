using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

[assembly: FunctionsStartup(typeof(Crud_Operation_EFCore.Startup))]

namespace Crud_Operation_EFCore
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {

            builder.Services.AddHttpClient();

            builder.Services.AddDbContext<SQLDBContext>(options =>
                {
                    options.UseSqlServer(Environment.GetEnvironmentVariable("DBConnections"));

                });
        }
    }
}
