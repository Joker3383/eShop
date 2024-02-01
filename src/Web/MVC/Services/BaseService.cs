using System.Net;
using System.Text;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using MVC.Models;
using MVC.Services.Interfaces;
using Newtonsoft.Json;

namespace MVC.Services;

public class BaseService : IBaseService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BaseService(
        IHttpClientFactory clientFactory,
        IHttpContextAccessor httpContextAccessor)
    {
        _clientFactory = clientFactory;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
    {
        try
        {
            var client = _clientFactory.CreateClient();
            /*var discoveryDocument = await client.GetDiscoveryDocumentAsync(SD.AuthAPIBase);
        
            var tokenResponse = await client.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "mvc-client",
                ClientSecret = "mvc-client-secret",
                //Code = "code",
                RedirectUri = "https://localhost:7070/signin-oidc",
                //CodeVerifier = "code_verifier_used_for_pkce"
            });

            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            if (!string.IsNullOrEmpty(token))
            {
                client.SetBearerToken(token);
            }*/

            var httpMessage = new HttpRequestMessage();

            httpMessage.RequestUri = new Uri(requestDto.Url);
            HttpResponseMessage? apiResponse = null;
            switch (requestDto.Sd)
            {
                case SD.ApiType.POST:
                    httpMessage.Method = HttpMethod.Post;
                    break;
                case SD.ApiType.DELETE:
                    httpMessage.Method = HttpMethod.Delete;
                    break;
                case SD.ApiType.PUT:
                    httpMessage.Method = HttpMethod.Put;
                    break;
                default:
                    httpMessage.Method = HttpMethod.Get;
                    break;
            }

            if (requestDto.Data != null)
            {
                httpMessage.Content =
                    new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
            }

            apiResponse = await client.SendAsync(httpMessage);
            
            switch (apiResponse.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return new() { IsSuccess = false, Message = "Not Found" };
                case HttpStatusCode.Forbidden:
                    return new() { IsSuccess = false, Message = "Access Denied" };
                case HttpStatusCode.Unauthorized:
                    return new() { IsSuccess = false, Message = "Unauthorized" };
                case HttpStatusCode.InternalServerError:
                    return new() { IsSuccess = false, Message = "Internal Server Error" };
                default:
                    var apiContent = await apiResponse.Content.ReadAsStringAsync();
                    var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                    return apiResponseDto;
            }
        }
        catch (Exception ex)
        {
            var dto = new ResponseDto
            {
                Message = ex.Message.ToString(),
                IsSuccess = false
            };
            return dto;
        }
        
    
    }
}