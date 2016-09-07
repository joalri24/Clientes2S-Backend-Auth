namespace Clientes2S_Backend_Auth.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Clientes2S_Backend_Auth.Models.Clientes2S_Backend_Auth_DbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Clientes2S_Backend_Auth.Models.Clientes2S_Backend_Auth_DbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Clients.AddOrUpdate(c => c.Id,
                new Client { Id = 1, Association = "Directo", Name = "2Secure", State = "Normal", LastContact = DateTime.Now, Comments = "", Pendings = "", Follow = false });
        }
    }
}
