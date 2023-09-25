using Microsoft.AspNetCore.Mvc;

namespace publisher_api.Controllers;

[ApiController]
[Route("[controller]")]
public class PublisherController : ControllerBase
{
    private readonly ILogger<PublisherController> _logger;

    public PublisherController(ILogger<PublisherController> logger)
    {
        _logger = logger;
    }
}
