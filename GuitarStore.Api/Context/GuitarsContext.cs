using GuitarStore.Api.Context.Entities;
using Microsoft.EntityFrameworkCore;

namespace GuitarStore.Api.Context
{
    public class GuitarsContext : DbContext
    {
        public DbSet<Guitar> Guitars { get; set; }

        public GuitarsContext(DbContextOptions<GuitarsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Guitar>().HasData(
                new Guitar() { Uid = Guid.Parse("3D423AAA-68E9-4A2F-A5BC-F20FDCC4FE3C"), Name = "Ibanez" },
                new Guitar() { Uid = Guid.Parse("3D423AAA-68E9-4A2F-A5BC-F21FDCC4FE3C"), Name = "Schecter" },
                new Guitar() { Uid = Guid.Parse("3D423AAA-68E9-4A2F-A5BC-F22FDCC4FE3C"), Name = "Fender" });
        }
    }
}
