using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using System.Net.Http;

using LimsBffWeb.Models;
using LimsBffWeb.Utils;

namespace LimsBffWeb.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthBffController : Controller
{
    private readonly HttpClient _httpClient;

    private readonly string _authServiceUrl = "http://localhost:5042/api/Auth"; // Replace with your ProductService URL

    public AuthBffController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        string authServiceUrl = _authServiceUrl + "/login";
        var response = await _httpClient.PostAsJsonAsync(authServiceUrl, loginDto);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.Data != null && apiResponse?.IsSuccess == true)
        {
            apiResponse.HandleResponse<String>();
            return Ok(apiResponse);
        }

        return Unauthorized(new { Message = "Invalid login credentials." });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        string authServiceUrl = _authServiceUrl + "/register";
        var response = await _httpClient.PostAsJsonAsync(authServiceUrl, registerDto);
        using var responseStream = await response.Content.ReadAsStreamAsync();
        ApiResponse? apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(responseStream);
        if (apiResponse?.IsSuccess == true)
        {
            apiResponse.HandleResponse<String>();
            return Ok(apiResponse);
        }

        return Unauthorized(new { Message = "Invalid register credentials." });
    }

}