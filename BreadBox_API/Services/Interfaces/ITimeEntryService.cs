using BreadBox_API.Models;

namespace BreadBox_API.Services.Interfaces
{
    public interface ITimeEntryService
    {
        Task<List<TimeEntryModel>> GetAllTimeEntriesAsync();
        Task<TimeEntryModel> GetTimeEntryByIdAsync(int id);
        Task<TimeEntryModel> CreateTimeEntryAsync(TimeEntryCreateModel timeEntryCreateModel);
        Task<TimeEntryModel> UpdateTimeEntryAsync(int id, TimeEntryCreateModel timeEntryCreateModel);
        Task<bool> DeleteTimeEntryAsync(int id);
    }
}
