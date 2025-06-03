using BreadBox_API.Models;

namespace BreadBox_API.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<List<InvoiceModel>> GetAllInvoicesAsync();
        Task<InvoiceModel> GetInvoiceByIdAsync(int id);
        Task<InvoiceModel> CreateInvoiceAsync(InvoiceCreateModel invoiceCreateModel);
        Task<InvoiceModel> UpdateInvoiceAsync(int id, InvoiceCreateModel invoiceCreateModel);
        Task<bool> DeleteInvoiceAsync(int id);
    }
}
