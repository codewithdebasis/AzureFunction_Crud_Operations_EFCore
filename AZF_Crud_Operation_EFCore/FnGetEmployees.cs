using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Crud_Operation_EFCore
{
    public class FnGetEmployees
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public FnGetEmployees(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FnGetEmployees>();
        }

        [Function("GetEmployees")]
        public async Task<List<Employee>> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetEmployees")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var _employee = new List<Employee>();
            try
            {
                string defaultConnection = Environment.GetEnvironmentVariable("DBConnections");

                _logger.LogInformation(defaultConnection);
                var options = new DbContextOptionsBuilder<SQLDBContext>();
                options.UseSqlServer(defaultConnection);

                var _dbContext = new SQLDBContext(options.Options);

                _employee = await _dbContext.Employees.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }

            return _employee;
        }
    }
}
