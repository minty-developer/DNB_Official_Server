using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]/v1")]
public partial class ApiController : ControllerBase
{
    [HttpGet]
    public IActionResult Ping()
    {
        return StatusCode(200);
    }
}