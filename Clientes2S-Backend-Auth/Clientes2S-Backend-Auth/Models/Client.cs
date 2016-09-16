﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Clientes2S_Backend_Auth.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Association { get; set; }

        public string Comments { get; set; }

        public string Pendings { get; set; }

        public DateTime LastContact { get; set; }

        public string State { get; set; }

        // "Foreign Key"
        public int MainContactId { get; set; }

        public bool Follow{ get; set; }

        // Foreing key
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }

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

        public string OwnerEmail { get; set; }
    }
}