using BreadBox_API.Data;
using BreadBox_API.Entities;
using BreadBox_API.Models;
using BreadBox_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BreadBox_API.Services
{  
    public class ClientService : IClientService
    {
        private readonly BreadBoxDbContext _context;

        public ClientService(BreadBoxDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClientModel>> GetAllClientsAsync()
        {
            return await _context.Clients
                .Select(c => new ClientModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    EmailAddress = c.EmailAddress,
                    Phone = c.Phone,
                    Address = c.Address,
                    CreatedAt = c.CreatedAt,
                    UserId = c.UserId
                })
                .ToListAsync();
        }

        public async Task<ClientModel> GetClientByIdAsync(int id)
        {
            var client = await _context.Clients
                .Where(c => c.Id == id)
                .Select(c => new ClientModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    EmailAddress = c.EmailAddress,
                    Phone = c.Phone,
                    Address = c.Address,
                    CreatedAt = c.CreatedAt,
                    UserId = c.UserId
                })
                .FirstOrDefaultAsync();

            return client;
        }

        public async Task<ClientModel> CreateClientAsync(ClientCreateModel clientCreateModel)
        {
            // Validate UserId exists
            var userExists = await _context.Users.AnyAsync(u => u.Id == clientCreateModel.UserId);
            if (!userExists)
            {
                throw new ArgumentException("Invalid UserId: User does not exist.");
            }

            var client = new Client
            {
                Name = clientCreateModel.Name,
                EmailAddress = clientCreateModel.EmailAddress,
                Phone = clientCreateModel.Phone,
                Address = clientCreateModel.Address,
                CreatedAt = DateTime.UtcNow,
                UserId = clientCreateModel.UserId
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return new ClientModel
            {
                Id = client.Id,
                Name = client.Name,
                EmailAddress = client.EmailAddress,
                Phone = client.Phone,
                Address = client.Address,
                CreatedAt = client.CreatedAt,
                UserId = client.UserId
            };
        }

        public async Task<ClientModel> UpdateClientAsync(int id, ClientCreateModel clientCreateModel)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return null;
            }

            // Validate UserId exists
            var userExists = await _context.Users.AnyAsync(u => u.Id == clientCreateModel.UserId);
            if (!userExists)
            {
                throw new ArgumentException("Invalid UserId: User does not exist.");
            }

            client.Name = clientCreateModel.Name;
            client.EmailAddress = clientCreateModel.EmailAddress;
            client.Phone = clientCreateModel.Phone;
            client.Address = clientCreateModel.Address;
            client.UserId = clientCreateModel.UserId;

            await _context.SaveChangesAsync();

            return new ClientModel
            {
                Id = client.Id,
                Name = client.Name,
                EmailAddress = client.EmailAddress,
                Phone = client.Phone,
                Address = client.Address,
                CreatedAt = client.CreatedAt,
                UserId = client.UserId
            };
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return false;
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
