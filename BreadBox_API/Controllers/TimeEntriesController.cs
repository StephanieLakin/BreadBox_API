using BreadBox_API.Models;
using BreadBox_API.Services;
using BreadBox_API.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BreadBox_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeEntriesController : ControllerBase
    {
        private readonly ITimeEntryService _timeEntryService;

        public TimeEntriesController(ITimeEntryService timeEntryService)
        {
            _timeEntryService = timeEntryService;
        }

        // GET: api/timeentries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeEntryModel>>> GetTimeEntries()
        {
            var timeEntries = await _timeEntryService.GetAllTimeEntriesAsync();
            return Ok(timeEntries);
        }

        // GET: api/timeentries/1
        [HttpGet("{id}")]
        public async Task<ActionResult<TimeEntryModel>> GetTimeEntry(int id)
        {
            var timeEntry = await _timeEntryService.GetTimeEntryByIdAsync(id);
            if (timeEntry == null)
                return NotFound();
            return Ok(timeEntry);
        }

        // POST: api/timeentries
        [HttpPost]
        public async Task<ActionResult<TimeEntryModel>> CreateTimeEntry(TimeEntryCreateModel timeEntryCreateModel)
        {
            try
            {
                var createdTimeEntry = await _timeEntryService.CreateTimeEntryAsync(timeEntryCreateModel);
                return CreatedAtAction(nameof(GetTimeEntry), new { id = createdTimeEntry.Id }, createdTimeEntry);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Errors = ex.Errors.Select(e => e.ErrorMessage) });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/timeentries/1
        [HttpPut("{id}")]
        public async Task<ActionResult<TimeEntryModel>> UpdateTimeEntry(int id, TimeEntryCreateModel timeEntryCreateModel)
        {
            try
            {
                var updatedTimeEntry = await _timeEntryService.UpdateTimeEntryAsync(id, timeEntryCreateModel);
                if (updatedTimeEntry == null)
                    return NotFound();
                return Ok(updatedTimeEntry);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Errors = ex.Errors.Select(e => e.ErrorMessage) });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/timeentries/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimeEntry(int id)
        {
            var deleted = await _timeEntryService.DeleteTimeEntryAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
