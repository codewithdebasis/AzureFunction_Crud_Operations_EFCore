using System.Net;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Crud_Operation_EFCore
{
    public class FnCreateEmployee
    {
        private readonly ILogger _logger;

        public FnCreateEmployee(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FnCreateEmployee>();
        }

        [Function("CreateEmployee")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var _input_data = JsonConvert.DeserializeObject<Employee>(requestBody);

            try
            {

                string defaultConnection = Environment.GetEnvironmentVariable("DBConnections");
                var options = new DbContextOptionsBuilder<SQLDBContext>();
                options.UseSqlServer(defaultConnection);

                var _dbContext = new SQLDBContext(options.Options);

                _dbContext.Employees.Add(_input_data);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return new ObjectResult(e.ToString());
            }
            return new OkObjectResult(HttpStatusCode.OK);
        }
    }
}
