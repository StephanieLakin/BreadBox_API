using BreadBox_API.Models;
using BreadBox_API.Services;
using BreadBox_API.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BreadBox_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        // GET: api/invoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoiceModel>>> GetInvoices()
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync();
            return Ok(invoices);
        }

        // GET: api/invoices/1
        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceModel>> GetInvoice(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null)
                return NotFound();
            return Ok(invoice);
        }

        // POST: api/invoices
        [HttpPost]
        public async Task<ActionResult<InvoiceModel>> CreateInvoice(InvoiceCreateModel invoiceCreateModel)
        {
            try
            {
                var createdInvoice = await _invoiceService.CreateInvoiceAsync(invoiceCreateModel);
                return CreatedAtAction(nameof(GetInvoice), new { id = createdInvoice.Id }, createdInvoice);
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

        // PUT: api/invoices/1
        [HttpPut("{id}")]
        public async Task<ActionResult<InvoiceModel>> UpdateInvoice(int id, InvoiceCreateModel invoiceCreateModel)
        {
            try
            {
                var updatedInvoice = await _invoiceService.UpdateInvoiceAsync(id, invoiceCreateModel);
                if (updatedInvoice == null)
                    return NotFound();
                return Ok(updatedInvoice);
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

        // DELETE: api/invoices/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var deleted = await _invoiceService.DeleteInvoiceAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
