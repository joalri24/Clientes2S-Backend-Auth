using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Clientes2S_Backend_Auth.Models
{
    public class Contact
    {
        public int Id { get; set; }

        // Foreign Key
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
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}