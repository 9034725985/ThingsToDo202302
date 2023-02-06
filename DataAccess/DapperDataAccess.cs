using Dapper;
using DataAccess.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Npgsql;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DapperDataAccess : IDapperDataAccess
    {
        //private readonly string _connectionString = "Host=hansken.db.elephantsql.com;Database=xrbmpoui;User Id=xrbmpoui;Password=i38x7v1O3aNteoNxteJNB5thtPfKqqxn;";
        private readonly string _connectionString;
        private readonly ILogger _logger;

        public DapperDataAccess(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<IEnumerable<MyTodoItem>> GetTodoItems(CancellationToken cancellationToken)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            Stopwatch stopwatch = Stopwatch.StartNew();
            IEnumerable<MyTodoItem> items = await connection.QueryAsync<MyTodoItem>(
                @"select
                item.*
            from mytodoitem item
            limit 200
            ;");
            stopwatch.Stop();
            foreach (MyTodoItem item in items)
            {
                item.Stopwatch = stopwatch;
            }
            _logger.LogDebug("{methodName} returned {result}", nameof(GetTodoItems), JsonConvert.SerializeObject(items));
            return items;
        }
    }
}