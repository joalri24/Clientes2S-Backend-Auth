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

namespace Clientes2S_Backend_Auth.Controllers
{
    [RoutePrefix("api/clients")]
    public class ClientsController : ApiController
    {
        private Clientes2S_Backend_Auth_DbContext db = new Clientes2S_Backend_Auth_DbContext();

        // GET: api/Clients
        public IQueryable<Client> GetClients()
        {
            return db.Clients;
        }

        // GET: api/Clients/5
        [ResponseType(typeof(Client))]
        public async Task<IHttpActionResult> GetClient(int id)
        {
            Client client = await db.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
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
            return db.Contacts.Where(b => b.ClientId == id);
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
            return db.Jobs.Where(b => b.ClientId == id);
        }

        // PUT: api/Clients/5
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
        [ResponseType(typeof(Client))]
        public async Task<IHttpActionResult> PostClient(Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Clients.Add(client);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = client.Id }, client);
        }

        // DELETE: api/Clients/5
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
        }

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