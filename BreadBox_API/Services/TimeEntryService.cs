using BreadBox_API.Data;
using BreadBox_API.Entities;
using BreadBox_API.Models;
using BreadBox_API.Services.Interfaces;
using BreadBox_API.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BreadBox_API.Services
{
    public class TimeEntryService : ITimeEntryService
    {
        private readonly BreadBoxDbContext _context;
        private readonly IValidator<TimeEntryCreateModel> _timeEntryCreateValidator;

        public TimeEntryService(BreadBoxDbContext context, IValidator<TimeEntryCreateModel> timeEntryValidator)
        {
            _context = context;
            _timeEntryCreateValidator = timeEntryValidator;
        }

        public async Task<List<TimeEntryModel>> GetAllTimeEntriesAsync()
        {
            return await _context.TimeEntries
                .Select(te => new TimeEntryModel
                {
                    Id = te.Id,
                    ClientId = te.ClientId,
                    StartTime = te.StartTime,
                    EndTime = te.EndTime,
                    HoursWorked = te.HoursWorked,
                    Description = te.Description,
                    Rate = te.Rate,
                    CreatedAt = te.CreatedAt,
                    UserId = te.UserId
                }
                ).ToListAsync();
        }
        public async Task<TimeEntryModel> GetTimeEntryByIdAsync(int id)
        {
            return await _context.TimeEntries
                .Where(te => te.Id == id)
                .Select(te => new TimeEntryModel
                {
                    Id = te.Id,
                    ClientId = te.ClientId,
                    StartTime = te.StartTime,
                    EndTime = te.EndTime,
                    HoursWorked = te.HoursWorked,
                    Description = te.Description,
                    Rate = te.Rate,
                    CreatedAt = te.CreatedAt,
                    UserId = te.UserId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<TimeEntryModel> CreateTimeEntryAsync(TimeEntryCreateModel timeEntryCreateModel)
        {
            var validationResult = await _timeEntryCreateValidator.ValidateAsync(timeEntryCreateModel);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            // Validate UserId and ClientId
            if (!await _context.Users.AnyAsync(u => u.Id == timeEntryCreateModel.UserId))
                throw new ArgumentException("Invalid UserId: User does not exist.");
            if (!await _context.Clients.AnyAsync(c => c.Id == timeEntryCreateModel.ClientId))
                throw new ArgumentException("Invalid ClientId: Client does not exist.");

            // Validate StartTime < EndTime
            if (timeEntryCreateModel.StartTime >= timeEntryCreateModel.EndTime)
                throw new ArgumentException("StartTime must be earlier than EndTime.");

            double hoursWorked = timeEntryCreateModel.EndTime.HasValue
                            ? (timeEntryCreateModel.EndTime.Value.ToUniversalTime() - timeEntryCreateModel.StartTime.ToUniversalTime()).TotalHours
                            : timeEntryCreateModel.HoursWorked;

            var timeEntry = new TimeEntry
            {
                ClientId = timeEntryCreateModel.ClientId,
                StartTime = timeEntryCreateModel.StartTime.ToUniversalTime(),
                EndTime = timeEntryCreateModel.EndTime?.ToUniversalTime(),
                HoursWorked = hoursWorked,
                Description = timeEntryCreateModel.Description,
                Rate = timeEntryCreateModel.Rate,
                CreatedAt = DateTime.UtcNow,
                UserId = timeEntryCreateModel.UserId
            };

            _context.TimeEntries.Add(timeEntry);
            await _context.SaveChangesAsync();

            return new TimeEntryModel
            {
                Id = timeEntry.Id,
                ClientId = timeEntry.ClientId,
                StartTime = timeEntry.StartTime,
                EndTime = timeEntry.EndTime,
                HoursWorked = timeEntry.HoursWorked,
                Description = timeEntry.Description,
                Rate = timeEntry.Rate,
                CreatedAt = timeEntry.CreatedAt,
                UserId = timeEntry.UserId
            };
        }

        public async Task<TimeEntryModel> UpdateTimeEntryAsync(int id, TimeEntryCreateModel timeEntryCreateModel)
        {

            var validationResult = await _timeEntryCreateValidator.ValidateAsync(timeEntryCreateModel);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var timeEntry = await _context.TimeEntries.FindAsync(id);
            if (timeEntry == null)
                return null;

            // Validate UserId and ClientId
            if (!await _context.Users.AnyAsync(u => u.Id == timeEntryCreateModel.UserId))
                throw new ArgumentException("Invalid UserId: User does not exist.");
            if (!await _context.Clients.AnyAsync(c => c.Id == timeEntryCreateModel.ClientId))
                throw new ArgumentException("Invalid ClientId: Client does not exist.");

            // Validate StartTime < EndTime (already handled by validator, but kept for clarity)
            if (timeEntryCreateModel.StartTime >= timeEntryCreateModel.EndTime)
                throw new ArgumentException("StartTime must be earlier than EndTime.");

            timeEntry.ClientId = timeEntryCreateModel.ClientId;
            timeEntry.StartTime = timeEntryCreateModel.StartTime;
            timeEntry.EndTime = timeEntryCreateModel.EndTime;
            timeEntry.Description = timeEntryCreateModel.Description;
            timeEntry.UserId = timeEntryCreateModel.UserId;

            await _context.SaveChangesAsync();

            return new TimeEntryModel
            {
                Id = timeEntry.Id,
                ClientId = timeEntry.ClientId,
                StartTime = timeEntry.StartTime,
                EndTime = timeEntry.EndTime,
                Description = timeEntry.Description,
                UserId = timeEntry.UserId
            };
        }

        public async Task<bool> DeleteTimeEntryAsync(int id)
        {
            var timeEntry = await _context.TimeEntries.FindAsync(id);
            if (timeEntry == null)
                return false;

            _context.TimeEntries.Remove(timeEntry);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

