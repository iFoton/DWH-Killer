using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace UI;

public class APIAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public APIAuthorizationMessageHandler(IAccessTokenProvider provider,
        NavigationManager navigationManager, IConfiguration config)
        : base(provider, navigationManager)
    {
        ConfigureHandler(
            authorizedUrls: new[] { config.GetSection("API")["Base"] },
            scopes: new[] { config.GetSection("API")["Scope"] });
    }
}
