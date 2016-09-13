﻿using System;
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
    [Authorize(Roles = "Employee,Admin")]
    [RoutePrefix("api/clients")]   
    public class ClientsController : ApiController
    {
        private Clientes2S_Backend_Auth_DbContext db = new Clientes2S_Backend_Auth_DbContext();

        // GET: api/Clients
        /// <summary>
        /// Returns all the clients associated with the authenticated user.
        /// </summary>
        /// <returns></returns>        
        public IQueryable<Client> GetClients()
        {
            //return db.Clients;
            var userId = User.Identity.GetUserId();
            return db.Clients.Where(c => c.ApplicationUserId == userId);
        }

        // GET: api/Clients
        /// <summary>
        /// Returns all the clients in the database. Restricted to Admins.
        /// </summary>
        /// <returns></returns>
        [Route("all")]
        [Authorize(Roles = "Admin")]
        public IQueryable<Client> GetAllClients()
        {
            return db.Clients;
        }

        // GET: api/Clients/5
        /// <summary>
        /// Returns the data of the specified client. Only the client's owner and 
        /// users with the Admin role can have acces to the data.
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
        /// Modifies the data of the specified client. Only the client's owner is authorized to do it.
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

            // Verifica que el usuario que hace a solicitud sea dueño del cliente.
            string userId = User.Identity.GetUserId();
            if (client.ApplicationUserId != userId)
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
        /// Creates a new Client using the info given in the http body.
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