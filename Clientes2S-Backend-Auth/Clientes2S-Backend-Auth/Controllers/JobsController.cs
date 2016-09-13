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
    [Authorize(Roles = "Employee,Admin")]
    public class JobsController : ApiController
    {
        private Clientes2S_Backend_Auth_DbContext db = new Clientes2S_Backend_Auth_DbContext();

        // GET: api/Jobs
        /// <summary>
        /// Returns all the Jobs associated with the authenticated user.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Job> GetJobs()
        {
            //return db.Jobs;
            var userId = User.Identity.GetUserId();
            return db.Jobs.Where(c => c.ApplicationUserId == userId);
        }

        // GET: api/Jobs/5
        /// <summary>
        /// Returns the data of the specified Job. Only the job's owner and 
        /// users with the Admin role can have acces to the data.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Job))]
        public async Task<IHttpActionResult> GetJob(int id)
        {
            Job job = await db.Jobs.FindAsync(id);
            var userId = User.Identity.GetUserId();

            if (job == null)   
            {
                return NotFound();
            }
            if (!User.IsInRole("Admin") && job.ApplicationUserId != userId) // Si la tarea no pertenece al usuario que lo consulta y este no es admin.
            {
                return Unauthorized();
            }

            return Ok(job);
        }

        // PUT: api/Jobs/5
        /// <summary>
        /// Modifies the data of the specified Job. Only the job's owner is authorized to do it.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="job"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutJob(int id, Job job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != job.Id)
            {
                return BadRequest();
            }

            // Verifica que el usuario que hace a solicitud sea dueño de la tarea.
            string userId = User.Identity.GetUserId();
            if (job.ApplicationUserId != userId)
            {
                return Unauthorized();
            }

            db.Entry(job).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(id))
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

        // POST: api/Jobs
        /// <summary>
        /// Creates a new Job using the info given in the HTTP body.
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        [ResponseType(typeof(Job))]
        public async Task<IHttpActionResult> PostJob(Job job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            job.ApplicationUserId = User.Identity.GetUserId();
            db.Jobs.Add(job);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = job.Id }, job);
        }

        /** DELETE: api/Jobs/5
        [ResponseType(typeof(Job))]
        public async Task<IHttpActionResult> DeleteJob(int id)
        {
            Job job = await db.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            db.Jobs.Remove(job);
            await db.SaveChangesAsync();

            return Ok(job);
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool JobExists(int id)
        {
            return db.Jobs.Count(e => e.Id == id) > 0;
        }
    }
}