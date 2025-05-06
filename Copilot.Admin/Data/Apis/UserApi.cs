using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Copilot.Admin.Data.Exceptions;

namespace Copilot.Admin.Data.Apis;

public class UserApi
{
    private readonly HttpClient _httpClient;

    public UserApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public record SignInResponse(string AccessToken);
    public async Task<SignInResponse?> SignIn(LoginParameter loginRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("/identity/login", loginRequest);

        return response.StatusCode switch
        {
            HttpStatusCode.OK =>
                await response.Content.ReadFromJsonAsync<SignInResponse>(),

            HttpStatusCode.NotFound =>
                throw new NotFoundHttpException(await response.Content.ReadAsStringAsync()),

            HttpStatusCode.BadRequest =>
                throw new BadRequestHttpException(await response.Content.ReadAsStringAsync()),

            _ => throw new UnsupportedHttpException($"Code: {(int)response.StatusCode}. " +
                                                    $"Message: {await response.Content.ReadAsStringAsync()}")
        };
    }

    public async Task SignUp(SignUpParameter parameter)
    {
        await _httpClient.PostAsJsonAsync("/identity/register", parameter);
    }

    public async Task<UserViewModel?> GetData(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync("/identity/manage/info");

        return response.StatusCode switch
        {
            HttpStatusCode.OK =>
                await response.Content.ReadFromJsonAsync<UserViewModel>(),

            HttpStatusCode.Unauthorized =>
                throw new UnauthorizedHttpException(await response.Content.ReadAsStringAsync()),

            _ => throw new UnsupportedHttpException(await response.Content.ReadAsStringAsync())
        };
    }
}

public record LoginParameter(string Email, string Password);

public record SignUpParameter(string Email, string Password);

public record UserViewModel(Guid Id, string Email);