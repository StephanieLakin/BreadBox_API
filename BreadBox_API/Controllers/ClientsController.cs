using BreadBox_API.Models;
using BreadBox_API.Services;
using BreadBox_API.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BreadBox_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        // GET: api/clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientModel>>> GetClients()
        {
            var clients = await _clientService.GetAllClientsAsync();
            return Ok(clients);
        }

        // GET: api/clients/1       
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientModel>> GetClient(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        // POST: api/clients
        [HttpPost]
        public async Task<ActionResult<ClientModel>> CreateClient(ClientCreateModel clientCreateModel)
        {
            try
            {
                var createdClient = await _clientService.CreateClientAsync(clientCreateModel);
                return CreatedAtAction(nameof(GetClient), new { id = createdClient.Id }, createdClient);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Errors = ex.Errors.Select(e => e.ErrorMessage) });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // PUT: api/clients/1
        [HttpPut("{id}")]
        public async Task<ActionResult<ClientModel>> UpdateClient( int id, ClientCreateModel clientCreateModel)
        {
            try
            {
                var updateClient = await _clientService.UpdateClientAsync(id, clientCreateModel);
                if (updateClient == null)
                {
                    return NotFound();  
                }
                return Ok(updateClient);
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

        // Delete: api/clients/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var deleted = await _clientService.DeleteClientAsync(id);
            if (!deleted)
            {
                return NotFound(deleted);
            }
            return NoContent();
        }

    }
}
