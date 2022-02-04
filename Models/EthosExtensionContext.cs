using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace EthosExtensionApi.Models
{
    public class EthosExtensionContext : DbContext
    {
        public EthosExtensionContext(DbContextOptions<EthosExtensionContext> options)
        : base(options)
        {
        }

        public DbSet<StudentConfirmation> StudentConfirmations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentConfirmation>(entity =>
            {
                entity.HasNoKey();
            });
        }
    }
}