using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Clientes2S_Backend_Auth.Models
{
    public class Job
    {
        public int Id { get; set; }

        // Foreign Key
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
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}