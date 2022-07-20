using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace API;

public static class GetUserRunningQueries
{
    [FunctionName("GetUserRunningQueries")]
    public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetUserRunningQueries/{login}")]
        HttpRequest req,
        string login,
        [Sql(@"
            SELECT r.[session_id] AS [id]
                 , r.[status]    
                 , DATEDIFF(SECOND, [start_time], GETUTCDATE()) / 60 AS [in_run]
                 , r.[command]
            FROM [sys].[dm_pdw_exec_requests] AS r
            JOIN [sys].[dm_pdw_exec_sessions] AS s ON r.[session_id] = s.[session_id]
            WHERE r.[status] NOT IN ('Completed', 'Failed', 'Cancelled')
              AND r.[session_id] <> session_id()
              AND s.[login_name] = @Login;",
            CommandType = System.Data.CommandType.Text,
            Parameters = "@Login={login}",
            ConnectionStringSetting = "SqlBindConnectionString")]
        IEnumerable<Queries> queries,
        ILogger log)
    {
        log.LogInformation($"Requested running queries for {login}");
        return new OkObjectResult(queries);
    }

    public class Queries
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public int? In_run { get; set; }
        public string Command { get; set; }
    }
}
