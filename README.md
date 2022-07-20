# DWH Killer

- Simple web interface for killing sessions in Synapse Dedicated SQL Pool.
- Based on Blazor Web assembly and Azure Functions.
- Uses Azure AD for authentication and authorization of API, application, and end users.

This application solves the limitation of Synapse Dedicated SQL Pool - you cannot grant permission to users for killing their queries only. The app provides a web interface where users can see their running queries and run the KILL command for selected sessions.

<sub>Database setup:</sub>
```sql
CREATE USER [{FUNCTION_APP_NAME}] FROM EXTERNAL PROVIDER;
GRANT CONTROL ON DATABASE::[{DATABASE_NAME}] TO [{FUNCTION_APP_NAME}];
```
