# DWH Killer

- Simple web interface for killing sessions in [Synapse Dedicated SQL Pool](https://docs.microsoft.com/en-us/azure/synapse-analytics/sql-data-warehouse/sql-data-warehouse-overview-what-is).
- Based on [Blazor Web assembly](https://docs.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-6.0#blazor-webassembly) and [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-overview).
- Uses [Azure AD](https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-overview) for authentication and authorization of API, application, and end users.

This application solves the limitation of Synapse Dedicated SQL Pool - you cannot grant permission to users for killing their queries only. The app provides a web interface where users can see their running queries and run the KILL command for selected sessions.

## Installation 
1. API 
  - Create Azure Function
  - Enable system assigned managed identity
  - Enable Microsoft authentication 
  - Grant permission to kill sessions in the database
2. App
  - Create Azure Web Static App
  - Create Enterprise Aplication in Azure AD for your app
  - Grant permission to App for calling API
3. Deploy
  - Deploy UI and API projects to Azure by Azure DevOps piplines or GitHub Actions
4. Assign users
  - Grant access to users for using App and API
  
<sub>Database setup:</sub>
```sql
CREATE USER [{FUNCTION_APP_NAME}] FROM EXTERNAL PROVIDER;
GRANT CONTROL ON DATABASE::[{DATABASE_NAME}] TO [{FUNCTION_APP_NAME}];
```
