using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPIPractica.Entity;
using RestAPIPractica.Interface;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IjwtService _jwtService;
    private readonly IAuthService _authService;

    public AuthController(IjwtService jwtService, IAuthService authService)
    {
        _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    [HttpPost("gettoken")]
    public IActionResult getToken(string correo)
    {
        var token = _jwtService.generateToken(correo);
        return Ok(token);
    }

    [HttpPost("auth")]
    public async Task<Auth> Auth(string correo, string password)
    {
        return await _authService.Auth(correo, password);
    }
}

