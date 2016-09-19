using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Clientes2S_Backend_Auth.Models
{
    /// <summary>
    /// Represents a Task associated with a 2Secure client.
    /// </summary>
    public class Job
    {
        public int Id { get; set; }

        // Foreign Key
        /// <summary>
        /// The db Id of the Client associated with this Job.
        /// </summary>
        public int ClientId { get; set; }

        // Navigation properties
        public Client Client { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string State { get; set; }

        public DateTime Date { get; set; }

        // "Foreign Key"
        public int ContactId { get; set; }

        // Foreing key
        /// <summary>
        /// Foreign key of the User Table. Is the unique Id of the User owner of this job.
        /// </summary>
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}