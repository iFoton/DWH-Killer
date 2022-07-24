using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Http.Json;

namespace UI.Pages;

public partial class Killer
{
    private Query[]? queries;
    private string? user;
    private string? status;
    private bool disabled = false;

    public class Query
    {
        public string Id { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int? In_run { get; set; }
        public string Command { get; set; } = null!;
    }

    protected override async Task OnInitializedAsync()
    {
        status = "Loading...";
        var User = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
        user = User.FindFirst(c => c.Type == "preferred_username")?.Value;
        await GetQueries(user);
    }

    private async Task Refresh(MouseEventArgs e)
    {
        await GetQueries(user);
    }

    private async Task GetQueries(string? user)
    {
        status = "Loading...";
        disabled = true;

        try
        {
            queries = await Http.GetFromJsonAsync<Query[]>($"GetUserRunningQueries/{user}");
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }
        finally
        {
            disabled = false;
            status = null;
        }
    }

    private async Task KillSession(string sid)
    {
        status = $"Killing {sid}";
        disabled = true;

        try
        {
            var result = await Http.PostAsJsonAsync($"KillSession/{sid}", String.Empty);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                status = $"Killed {sid}";
            }
            else
            {
                status = $"Error: {result.Content}";
            }
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }
        finally
        {
            await GetQueries(user);
        }
    }
}
