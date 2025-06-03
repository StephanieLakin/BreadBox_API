using BreadBox_API.Models;

namespace BreadBox_API.Services.Interfaces
{
    public interface IClientService
    {
        Task<List<ClientModel>> GetAllClientsAsync();
        Task<ClientModel> GetClientByIdAsync(int id);
        Task<ClientModel> CreateClientAsync(ClientCreateModel clientCreateModel);
        Task<ClientModel> UpdateClientAsync(int id, ClientCreateModel clientCreateModel);
        Task<bool> DeleteClientAsync(int id);
    }
}
