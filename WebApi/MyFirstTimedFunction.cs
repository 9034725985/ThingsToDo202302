using System;
using DataAccess;
using DataAccess.Model;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace WebApi
{
    public class MyFirstTimedFunction
    {
        private readonly ILogger _logger;
        IConfiguration Configuration { get; set; }

        public MyFirstTimedFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MyFirstTimedFunction>();
            var builder = new ConfigurationBuilder()
            .AddUserSecrets<MyFirstTimedFunction>();
            Configuration = builder.Build();
        }

        [Function("MyFirstTimedFunction")]
        public async Task Run([TimerTrigger("45 */5 * * * *")] MyInfo myTimer, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus?.Next}");
            //DapperDataAccess access = new(_connectionString, logger);
            string? _connectionString = Configuration["ConnectionStrings:Default"];
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                return;
            }
            DapperDataAccess access = new(_connectionString, _logger);
            IEnumerable<MyTodoItem> results = await access.GetTodoItems(cancellationToken);
            if (results != null && results.Any())
            {
                _logger.LogInformation($"We got at least one todo item.");
                foreach (var item in results)
                {
                    _logger.LogInformation($"The first one is {item.Title} with description {item.Description}. It took us {item.Stopwatch?.ElapsedMilliseconds} milliseconds to get this information. Its reported {nameof(MyTodoItem.IsDone)} status is {item.IsDone}.");
                }
            }
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus? ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
