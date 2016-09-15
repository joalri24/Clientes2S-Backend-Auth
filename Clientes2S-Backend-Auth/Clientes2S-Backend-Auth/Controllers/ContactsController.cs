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
    public class ContactsController : ApiController
    {
        private Clientes2S_Backend_Auth_DbContext db = new Clientes2S_Backend_Auth_DbContext();

        // GET: api/Contacts
        /// <summary>
        /// Returns all the Contacts associated with the authenticated user.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Contact> GetContacts()
        {
            //return db.Contacts;
            var userId = User.Identity.GetUserId();
            return db.Contacts.Where(c => c.ApplicationUserId == userId);
        }

        // GET: api/Contacts/5
        /// <summary>
        /// Returns the data of the specified Contact. Only the contact's owner and 
        /// users with the Admin role can have acces to the data.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Contact))]
        public async Task<IHttpActionResult> GetContact(int id)
        {
            Contact contact = await db.Contacts.FindAsync(id);
            string userId = User.Identity.GetUserId();

            if (contact == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin") &&  contact.ApplicationUserId != userId) // Si el contacto no pertenece al usuario que lo consulta y este no es admin.
            {
                return Unauthorized();
            }

            return Ok(contact);
        }

        // PUT: api/Contacts/5
        /// <summary>
        /// Modifies the data of the specified Contact. Only the contact's owner is authorized to do it.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutContact(int id, Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contact.Id)
            {
                return BadRequest();
            }

            string userId = User.Identity.GetUserId();
            if (!User.IsInRole("Admin") && contact.ApplicationUserId != userId) // Si el contacto no pertenece al usuario que lo consulta y este no es admin.
            {
                return Unauthorized();
            }

            db.Entry(contact).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
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

        // POST: api/Contacts
        /// <summary>
        /// Creates a new Contact using the info given in the HTTP body.
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        [ResponseType(typeof(Contact))]
        public async Task<IHttpActionResult> PostContact(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            contact.ApplicationUserId = User.Identity.GetUserId();
            db.Contacts.Add(contact);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = contact.Id }, contact);
        }

        /** DELETE: api/Contacts/5
        [ResponseType(typeof(Contact))]
        public async Task<IHttpActionResult> DeleteContact(int id)
        {
            Contact contact = await db.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            db.Contacts.Remove(contact);
            await db.SaveChangesAsync();

            return Ok(contact);
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContactExists(int id)
        {
            return db.Contacts.Count(e => e.Id == id) > 0;
        }
    }
}