using BreadBox_API.Data;
using BreadBox_API.Entities;
using BreadBox_API.Models;
using BreadBox_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BreadBox_API.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly BreadBoxDbContext _context;

        public InvoiceService(BreadBoxDbContext context)
        {
            _context = context;
        }

        public async Task<List<InvoiceModel>> GetAllInvoicesAsync()
        {
            return await _context.Invoices
                .Select(i => new InvoiceModel
                {
                    Id = i.Id,
                    ClientId = i.ClientId,
                    Amount = i.Amount,
                    IssuedDate = i.IssuedDate,
                    DueDate = i.DueDate,
                    Status = i.Status,
                    UserId = i.UserId
                })
                .ToListAsync();
        }

        public async Task<InvoiceModel> GetInvoiceByIdAsync(int id)
        {
            return await _context.Invoices
                .Where(i => i.Id == id)
                .Select(i => new InvoiceModel
                {
                    Id = i.Id,
                    ClientId = i.ClientId,
                    Amount = i.Amount,
                    IssuedDate = i.IssuedDate,
                    DueDate = i.DueDate,
                    Status = i.Status,
                    UserId = i.UserId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<InvoiceModel> CreateInvoiceAsync(InvoiceCreateModel invoiceCreateModel)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == invoiceCreateModel.UserId))
                throw new ArgumentException("Invalid UserId: User does not exist.");
            if (!await _context.Clients.AnyAsync(c => c.Id == invoiceCreateModel.ClientId))
                throw new ArgumentException("Invalid ClientId: Client does not exist.");

            var invoice = new Invoice
            {
                ClientId = invoiceCreateModel.ClientId,
                Amount = invoiceCreateModel.Amount,
                IssuedDate = invoiceCreateModel.IssueDate,
                DueDate = invoiceCreateModel.DueDate,
                Status = invoiceCreateModel.Status,
                UserId = invoiceCreateModel.UserId
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return new InvoiceModel
            {
                Id = invoice.Id,
                Amount = invoice.Amount,
                IssuedDate = invoice.IssuedDate,
                DueDate = invoice.DueDate,
                Status = invoice.Status,
                UserId = invoice.UserId,
                ClientId = invoice.ClientId
            };
        }

        public async Task<InvoiceModel> UpdateInvoiceAsync(int id, InvoiceCreateModel invoiceCreateModel)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
                return null;

            // Validate UserId and ClientId
            if (!await _context.Users.AnyAsync(u => u.Id == invoiceCreateModel.UserId))
                throw new ArgumentException("Invalid UserId: User does not exist.");
            if (!await _context.Clients.AnyAsync(c => c.Id == invoiceCreateModel.ClientId))
                throw new ArgumentException("Invalid ClientId: Client does not exist.");

            invoice.ClientId = invoiceCreateModel.ClientId;
            invoice.Amount = invoiceCreateModel.Amount;
            invoice.IssuedDate = invoiceCreateModel.IssueDate;
            invoice.DueDate = invoiceCreateModel.DueDate;
            invoice.Status = invoiceCreateModel.Status;
            invoice.UserId = invoiceCreateModel.UserId;

            await _context.SaveChangesAsync();

            return new InvoiceModel
            {
                Id = invoice.Id,
                ClientId = invoice.ClientId,
                Amount = invoice.Amount,
                IssuedDate = invoice.IssuedDate,
                DueDate = invoice.DueDate,
                Status = invoice.Status,
                UserId = invoice.UserId
            };
        }

        public async Task<bool> DeleteInvoiceAsync(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
                return false;

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

