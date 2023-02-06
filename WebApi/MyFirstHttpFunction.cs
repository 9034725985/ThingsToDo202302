using System.Net;
using DataAccess.Model;
using DataAccess;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebApi
{
    public class MyFirstHttpFunction
    {
        private readonly ILogger _logger;
        IConfiguration Configuration { get; set; }

        public MyFirstHttpFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MyFirstHttpFunction>();
            var builder = new ConfigurationBuilder()
            .AddUserSecrets<MyFirstTimedFunction>();
            Configuration = builder.Build();
        }

        [Function("MyFirstHttpFunction")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req, CancellationToken cancellationToken)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string? _connectionString = Configuration["ConnectionStrings:Default"];
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                errorResponse.Headers.Add("Content-Type", "text/plain; charset=utf-8");

                errorResponse.WriteString("Something went wrong");

                return errorResponse;
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
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                response.WriteString(JsonConvert.SerializeObject(results));
                return response;
            }

            var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
            notFoundResponse.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            notFoundResponse.WriteString(JsonConvert.SerializeObject(results));
            return notFoundResponse;
        }
    }
}
