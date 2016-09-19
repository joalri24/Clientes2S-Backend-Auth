using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Clientes2S_Backend_Auth.Models;
using Microsoft.AspNet.Identity;

namespace Clientes2S_Backend_Auth.Controllers
{
    [Authorize(Roles = "Comercial,Admin")]
    [RoutePrefix("api/clients")]   
    public class ClientsController : ApiController
    {
        private Clientes2S_Backend_Auth_DbContext db = new Clientes2S_Backend_Auth_DbContext();

        // GET: api/Clients
        /// <summary>
        /// Returns all the clients associated with the authenticated user.
        /// </summary>
        /// <returns></returns>        
        public IQueryable<ClientDTO> GetClients()
        {

            var userId = User.Identity.GetUserId();

            // returns a DTO (data transfer object) that includes the owner's email.
            var clientes = from c in db.Clients.Where(c => c.ApplicationUserId == userId)
                           select new ClientDTO()
                           {
                               Id = c.Id,
                               Pendings = c.Pendings,
                               ApplicationUserId = c.ApplicationUserId,
                               Association = c.Association,
                               Comments = c.Comments,
                               Follow = c.Follow,
                               LastContact = c.LastContact,
                               MainContactId = c.MainContactId,
                               Name = c.Name,
                               OwnerEmail = c.ApplicationUser.Email,
                               State = c.State
                           };

            return clientes;
        }

        // GET: api/Clients/all
        /// <summary>
        /// Returns all the clients in the database. Restricted to Admins.
        /// </summary>
        /// <returns></returns>
        [Route("all")]
        [Authorize(Roles = "Admin")]
        public IQueryable<ClientDTO> GetAllClients()
        {
            //return db.Clients;
            var clientes = from c in db.Clients
                           select new ClientDTO()
                           {
                               Id = c.Id,
                               Pendings = c.Pendings,
                               ApplicationUserId = c.ApplicationUserId,
                               Association = c.Association,
                               Comments = c.Comments,
                               Follow = c.Follow,
                               LastContact = c.LastContact,
                               MainContactId = c.MainContactId,
                               Name = c.Name,
                               OwnerEmail = c.ApplicationUser.Email,
                               State = c.State
                        };

            return clientes;
        }

        // GET: api/Clients/5
        /// <summary>
        /// Returns the data of the specified client. Only the client's owner and 
        /// users with the Admin role can acces the data.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Client))]
        public async Task<IHttpActionResult> GetClient(int id)
        {
            Client client = await db.Clients.FindAsync(id);          

            if (client == null)    
            {
                return NotFound();
            }

            string userId = User.Identity.GetUserId();
            if ( !User.IsInRole("Admin") && client.ApplicationUserId != userId) // Si el usuario no es admin y no es el dueño del cliente.
            {
                return Unauthorized();
            }

            return Ok(client);
        }

        // GET: api/clients/1/contacts
        /// <summary>
        /// Gets all the contacts associated with the client.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id:int}/contacts")]
        [ResponseType(typeof(Contact))]
        public IQueryable<Contact> GetClientContacts(int id)
        {
            Client client =  db.Clients.Find(id);
            string userId = User.Identity.GetUserId();

            if (client != null && (client.ApplicationUserId == userId || User.IsInRole("Admin")) )  // Devuelve los datos únicamente si el usuario es admin o es dueño del cliente.
                return db.Contacts.Where(b => b.ClientId == id);
            else
                return new List<Contact>().AsQueryable();  // Lista vacía        
        }

        // GET: api/clients/1/contacts
        /// <summary>
        /// Gets all the Tasks(Jobs) associated with the client.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id:int}/jobs")]
        [ResponseType(typeof(Job))]
        public IQueryable<Job> GetClientjobs(int id)
        {

            Client client = db.Clients.Find(id);
            string userId = User.Identity.GetUserId();

            if (client != null && (client.ApplicationUserId == userId || User.IsInRole("Admin")))  // Devuelve los datos únicamente si el usuario es admin o es dueño del cliente.
                return db.Jobs.Where(b => b.ClientId == id);
            else
                return new List<Job>().AsQueryable();  // Lista vacía 

        }

        // PUT: api/Clients/5
        /// <summary>
        /// Modifies the data of the specified client. Only the client's owner and admins are authorized to do it.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutClient(int id, Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != client.Id)
            {
                return BadRequest();
            }

            string userId = User.Identity.GetUserId();
            if (!User.IsInRole("Admin") && client.ApplicationUserId != userId) // Si la tarea no pertenece al usuario y este no es admin.
            {
                return Unauthorized();
            }

            db.Entry(client).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Clients
        /// <summary>
        /// Creates a new Client using the info in the http body.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        [ResponseType(typeof(Client))]
        public async Task<IHttpActionResult> PostClient(Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            client.ApplicationUserId = User.Identity.GetUserId();  
            db.Clients.Add(client);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = client.Id }, client);
        }
     
        /** DELETE: api/Clients/5
        [ResponseType(typeof(Client))]
        public async Task<IHttpActionResult> DeleteClient(int id)
        {
            Client client = await db.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            db.Clients.Remove(client);
            await db.SaveChangesAsync();

            return Ok(client);
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClientExists(int id)
        {
            return db.Clients.Count(e => e.Id == id) > 0;
        }
    }
}