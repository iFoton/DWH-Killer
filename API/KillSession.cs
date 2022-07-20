using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Azure.Services.AppAuthentication;
using System.Collections.Generic;

namespace API;

public static class KillSession
{   
    [FunctionName("KillSession")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "KillSession/{session}")] HttpRequest req,
        string session,
        ILogger log)
    {
        try
        {
            int sid = int.Parse(session.Substring(3, 8));
            var claims = new List<string> {
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn",
                "preferred_username" };
            var user = req.HttpContext.User.Claims.First(c => claims.Contains(c.Type))?.Value;

            var query = $@"
                DECLARE @login NVARCHAR(100) = (SELECT TOP 1 [login_name] 
                                                FROM [sys].[dm_pdw_exec_sessions] 
                                                WHERE [session_id] = 'SID{sid}');
                IF @login IS NULL 
                    SELECT 'NO SESSION' AS [error];
                ELSE IF @login != N'{user}'
                    SELECT 'NOT AUTHORIZED' AS [error];
                ELSE 
                    KILL 'SID{sid}';";

            log.LogInformation($"Kill session {session} for user {user}");
                        
            using SqlConnection conn = new(Environment.GetEnvironmentVariable("SqlClientConnectionString"));
            conn.AccessToken = await new AzureServiceTokenProvider().GetAccessTokenAsync("https://database.windows.net/");
            using SqlCommand cmd = new(query, conn);
            cmd.CommandTimeout = 300;
            using SqlDataAdapter da = new(cmd);   
            await conn.OpenAsync();

            DataTable result = new();
            da.Fill(result);
            if (result.Rows.Count == 0)
            {
                return new OkObjectResult("OK");
            }
            else
            {
                return new BadRequestObjectResult(result.Rows[0]["error"]);
            }
        }
        catch (Exception ex)
        {
            log.LogWarning(ex.Message);
            return new BadRequestObjectResult(ex.Message);
        }        
    }
}


