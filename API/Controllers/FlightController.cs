using Microsoft.AspNetCore.Mvc;
using Models.FlightModels;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class FlightController : ControllerBase
{
    private readonly ILogger<FlightController> _logger;

    public FlightController(ILogger<FlightController> logger)
    {
        _logger = logger;
    }
}
