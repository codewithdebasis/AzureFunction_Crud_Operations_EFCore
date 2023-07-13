using System.Net;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Crud_Operation_EFCore
{
    public class FnGetEmployeeCode
    {
        private readonly ILogger _logger;

        public FnGetEmployeeCode(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FnGetEmployeeCode>();
        }

        [Function("GetEmployeeByCode")]
        public async Task<Employee> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetEmployeeByCode/{code}")] HttpRequestData req, string code)
        {
            var _employee = new Employee();
            try
            {
                string defaultConnection = Environment.GetEnvironmentVariable("DBConnections");
                var options = new DbContextOptionsBuilder<SQLDBContext>();
                options.UseSqlServer(defaultConnection);

                var _dbContext = new SQLDBContext(options.Options);

                var _employees = await _dbContext.Employees.ToListAsync();
                _employee = _employees.Where(s => s.Code.Equals(code)).ToList().FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }

            return _employee;
        }
    }
}
