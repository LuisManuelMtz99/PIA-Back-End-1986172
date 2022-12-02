using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PIA___Back___End___1986172.Entidades;

namespace PIA___Back___End___1986172
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Rifa> Rifas { get; set; }
        public DbSet<Boleto> Boletos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Premio> Premios { get; set; }
    }
}
