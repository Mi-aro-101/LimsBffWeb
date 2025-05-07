using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using LimsBffWeb.Models;
using LimsUtils.Api;

namespace LimsBffWeb.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthBffController : Controller
{
    private readonly HttpClient _httpClient;

    private readonly string _authServiceUrl = "http://localhost:5042/api/auth"; // Replace with your ProductService URL

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
            // apiResponse.HandleResponse<String>();
            return Ok(apiResponse);
        }

        return Unauthorized(new { Message = "Invalid register credentials." });
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("admin-data")]
    public IActionResult GetAdminData()
    {
        return Ok(new ApiResponse
            {
                Data = null,
                ViewBag = null,
                IsSuccess = true,
                Message = "Admin only",
                StatusCode = 200
            });
    }

    [Authorize(Policy = "TesteurPolicy")]
    [HttpGet("testeur-data")]
    public IActionResult GetTesteurData()
    {
       return Ok(new ApiResponse
            {
                Data = null,
                ViewBag = null,
                IsSuccess = true,
                Message = "Testeur only",
                StatusCode = 200
            });
    }

    [Authorize(Policy = "AdminOrTesteurPolicy")]
    [HttpGet("admin-testeur")]
    public IActionResult GetAdminOrManagerData()
    {
       return Ok(new ApiResponse
            {
                Data = null,
                ViewBag = null,
                IsSuccess = true,
                Message = "Testeur and Admin",
                StatusCode = 200
            });
    }

}