using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Common.Extensions;
using SchoolHub.Common.Models;
using SchoolHub.Common.Models.Interfaces;
using SchoolHub.Common.Models.Usuarios;
using System.Data;

namespace SchoolHub.Common.Data
{
    public class AppDbContext : IdentityDbContext<Usuario, Funcao, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tennant> Tennants { get; set; }
        public DbSet<Turma> Turmas { get; set; }
        public DbSet<Presenca> Presencas { get; set; }
        public DbSet<Documento> Documentos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entities<IStatusModificado>(this, nameof(this.ModelStatusModificado));

            builder.Entity<Turma>()
                .HasOne(t => t.Tennant)
                .WithMany(tn => tn.Turmas)
                .HasForeignKey(t => t.TennantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Usuario>()
                .HasOne(u => u.Turma)
                .WithMany(t => t.Usuarios)
                .HasForeignKey(u => u.TurmaId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Presenca>()
                .HasOne(a => a.Turma)
                .WithMany(cg => cg.Presencas)
                .HasForeignKey(a => a.TurmaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Presenca>()
                .HasOne(a => a.Usuario)
                .WithMany(u => u.Presencas)
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ModelStatusModificado<TEntity>(EntityTypeBuilder<TEntity> entity) where TEntity : class, IStatusModificado
        {
            entity.Property(x => x.DataCadastro).HasColumnType("datetime2(2)");
            entity.Property(x => x.DataModificado).HasColumnType("datetime2(2)");
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            PreencheIStatusModificado();
            return base.SaveChanges();
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            PreencheIStatusModificado();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void PreencheIStatusModificado()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.Entity != null
                    && typeof(IStatusModificado).IsAssignableFrom(e.Entity.GetType())))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataModificado").CurrentValue = DateTime.Now;
                    entry.Property("DataCadastro").IsModified = false;
                }
            }
        }
    }
}
