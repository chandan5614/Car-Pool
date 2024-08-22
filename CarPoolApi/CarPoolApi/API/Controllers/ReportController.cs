using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportById(string id)
        {
            var report = await _reportService.GetReportByIdAsync(id);
            if (report == null) return NotFound();
            return Ok(report);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReports()
        {
            var reports = await _reportService.GetAllReportsAsync();
            return Ok(reports);
        }

        [HttpPost]
        public async Task<IActionResult> AddReport([FromBody] ReportDto reportDto)
        {
            await _reportService.AddReportAsync(reportDto);
            return CreatedAtAction(nameof(GetReportById), new { id = reportDto.ReportId }, reportDto);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateReportStatus(string id, [FromBody] ReportStatus status)
        {
            await _reportService.UpdateReportStatusAsync(id, status);
            return NoContent();
        }
    }
}
