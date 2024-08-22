using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScheduleById(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out var scheduleGuid))
                {
                    return BadRequest(new { error = "The provided Schedule ID is not in a valid GUID format." });
                }

                var schedule = await _scheduleService.GetScheduleByIdAsync(id);
                if (schedule == null)
                {
                    return NotFound();
                }
                return Ok(schedule);
            }
            catch (FormatException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSchedules()
        {
            var schedules = await _scheduleService.GetAllSchedulesAsync();
            return Ok(schedules);
        }

        [HttpPost]
        public async Task<IActionResult> AddSchedule([FromBody] ScheduleDto scheduleDto)
        {
            await _scheduleService.AddScheduleAsync(scheduleDto);
            return CreatedAtAction(nameof(GetScheduleById), new { id = scheduleDto.ScheduleId }, scheduleDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSchedule([FromBody] ScheduleDto scheduleDto)
        {
            await _scheduleService.UpdateScheduleAsync(scheduleDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(string id)
        {
            await _scheduleService.DeleteScheduleAsync(id);
            return NoContent();
        }
    }
}
