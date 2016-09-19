using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Clientes2S_Backend_Auth.Models
{
    /// <summary>
    /// Represents a Client of 2Secure.
    /// </summary>
    public class Client
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The type of association with this cliente. Normally is Direct or By Intermediary.
        /// </summary>
        [Required]
        public string Association { get; set; }

        public string Comments { get; set; }

        public string Pendings { get; set; }

        public DateTime LastContact { get; set; }

        public string State { get; set; }

        /// <summary>
        /// The db Id of the main contact of this Client.
        /// </summary>
        public int MainContactId { get; set; }

        /// <summary>
        /// States if is worthy ot not to Follow this client.
        /// </summary>
        public bool Follow{ get; set; }

        // Foreing key
        /// <summary>
        /// Foreign key of the User Table. Is the unique Id of the User owner of this client.
        /// </summary>
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }

    /// <summary>
    /// The data representation of the Client that is sent in the responses.
    /// </summary>
    public class ClientDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Association { get; set; }

        public string Comments { get; set; }

        public string Pendings { get; set; }

        public DateTime LastContact { get; set; }

        public string State { get; set; }

        // "Foreign Key"
        public int MainContactId { get; set; }

        public bool Follow { get; set; }

        // Foreing key
        public string ApplicationUserId { get; set; }

        /// <summary>
        /// The email of the user owner of the Client.
        /// </summary>
        public string OwnerEmail { get; set; }
    }
}