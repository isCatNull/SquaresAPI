using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
[ApiController]
public class BaseController : Controller
{
}
