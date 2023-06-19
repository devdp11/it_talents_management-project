using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Entities;
using BusinessLogic.databaseContext;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly databaseContext _dbContext;

        public ClientController(databaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ClientModel>> GetClients()
        {
            var clients = _dbContext.Clients
                .Select(c => new ClientModel 
                { 
                    ClientID = c.Clientid,
                    UserID = c.Userid ?? default, // Se Userid for null, então use o valor padrão para int.
                    Name = c.Name
                })
                .ToList();

            return Ok(clients);
        }

        [HttpPost]
        public ActionResult<Client> CreateClient(ClientModel clientModel)
        {
            var client = new Client
            {
                Name = clientModel.Name,
                Userid = clientModel.UserID
            };

            _dbContext.Clients.Add(client);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetClientById), new { id = client.Clientid }, client);
        }

        [HttpGet("{id}")]
        public ActionResult<Client> GetClientById(int id)
        {
            var client = _dbContext.Clients.FirstOrDefault(c => c.Clientid == id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateClient(int id, ClientModel updatedClient)
        {
            var client = _dbContext.Clients.FirstOrDefault(c => c.Clientid == id);
            if (client == null)
            {
                return NotFound();
            }

            // Armazena o valor do UserID original
            var originalUserID = client.Userid;

            // Atualize apenas as propriedades que podem ser modificadas
            client.Name = updatedClient.Name;
            // Atualize outras propriedades, se necessário

            // Restaura o valor do UserID original no cliente atualizado
            updatedClient.UserID = originalUserID ?? default(int); // ou updatedClient.UserID = originalUserID.GetValueOrDefault();

            _dbContext.SaveChanges();
            return NoContent();
        }





        [HttpDelete("{id}")]
        public IActionResult DeleteClient(int id)
        {
            var client = _dbContext.Clients.FirstOrDefault(c => c.Clientid == id);
            if (client == null)
            {
                return NotFound();
            }
            _dbContext.Clients.Remove(client);
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpPost("{clientId}/jobproposals/{jobProposalId}")]
        public IActionResult AssignJobProposalToClient(int clientId, int jobProposalId)
        {
            var client = _dbContext.Clients.FirstOrDefault(c => c.Clientid == clientId);
            var jobProposal = _dbContext.Jobproposals.FirstOrDefault(j => j.Jobproposalid == jobProposalId);

            if (client == null || jobProposal == null)
            {
                return NotFound();
            }

            client.Jobproposals.Add(jobProposal);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
