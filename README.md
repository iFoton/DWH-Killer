# DWH Killer

- Simple web interface for killing sessions in [Synapse Dedicated SQL Pool](https://docs.microsoft.com/en-us/azure/synapse-analytics/sql-data-warehouse/sql-data-warehouse-overview-what-is).
- Based on [Blazor Web assembly](https://docs.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-6.0#blazor-webassembly) and [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-overview).
- Uses [Azure AD](https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-overview) for authentication and authorization of API, application, and end users.

This application solves the limitation of Synapse Dedicated SQL Pool - you cannot grant permission to users for killing their queries only. The app provides a web interface where users can see their running queries and run the KILL command for selected sessions.

Take a look at the [article](https://www.linkedin.com/pulse/synapse-dwh-session-killer-app-ivan-shlemov/) for a detailed explanation. 
