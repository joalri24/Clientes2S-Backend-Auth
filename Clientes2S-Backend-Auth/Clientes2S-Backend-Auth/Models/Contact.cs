using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Clientes2S_Backend_Auth.Models
{
    /// <summary>
    /// Represents a Contact of a 2Secure client.
    /// </summary>
    public class Contact
    {
        public int Id { get; set; }

        // Foreign Key
        /// <summary>
        /// The db Id of the Client associated with this contact.
        /// </summary>
        public int ClientId { get; set; }

        // Navigation property
        public Client Client { get; set; }

        [Required]
        public string Name { get; set; }

        public string JobTitle { get; set; }

        public string Telephone { get; set; }

        public string Mail { get; set; }

        public DateTime LastContact { get; set; }

        public string Notes { get; set; }

        // Foreing key
        /// <summary>
        /// Foreign key of the User Table. Is the unique Id of the User owner of this contact.
        /// </summary>
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}