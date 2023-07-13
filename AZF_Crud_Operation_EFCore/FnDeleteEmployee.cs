using System.Net;
using DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Crud_Operation_EFCore
{
    public class FnDeleteEmployee
    {
        private readonly ILogger _logger;

        public FnDeleteEmployee(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FnDeleteEmployee>();
        }

        [Function("DeleteEmployeeById")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "DeleteEmployee/{id}")] HttpRequestData req, long id)
        {
            string defaultConnection = Environment.GetEnvironmentVariable("DBConnections");
            var options = new DbContextOptionsBuilder<SQLDBContext>();
            options.UseSqlServer(defaultConnection);

            var _dbContext = new SQLDBContext(options.Options);

            var employee = await _dbContext.Employees.FindAsync(id);

            _dbContext.Employees.Remove(employee);

            return new OkObjectResult(HttpStatusCode.OK);
        }
    }
}
