using Microsoft.AspNetCore.Mvc;
using kd_backend.Models;
using kd_backend.Services;

namespace kd_backend.Controllers;

[ApiController]
[Route("api/data-processing")]
public class DataProcessingController : ControllerBase
{
    private readonly DataProcessingService _dataProcessingService;

    public DataProcessingController(DataProcessingService dataProcessingService)
    {
        _dataProcessingService = dataProcessingService;
    }

    [HttpPost("submit")]
    public IActionResult ProcessData([FromBody] PayloadModel payload)
    {
        if (payload == null)
        {
            Console.Out.WriteLine(payload);
            return BadRequest("Payload is null.");
        }
        var filePath = _dataProcessingService.ProcessPayloadAndSaveCsv(payload);
        Console.Out.WriteLine(payload);
        return Ok(new { Message = "CSV file created.", FilePath = filePath });
    }
}