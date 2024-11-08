using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Common.Extensions;
using SchoolHub.Common.Models.Entities;
using SchoolHub.Common.Models.Entities.Interfaces;
using SchoolHub.Common.Models.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Tennant> Tennants { get; set; }
        public DbSet<ClassGroup> ClassGroups { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entities<IStatusModified>(this, nameof(this.ModelStatusModified));

            builder.Entity<ClassGroup>()
                .HasOne(t => t.Tennant)
                .WithMany(tn => tn.ClassGroups)
                .HasForeignKey(t => t.TennantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasOne(u => u.ClassGroup)
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.ClassGroupId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Attendance>()
                .HasOne(a => a.ClassGroup)
                .WithMany(cg => cg.Attendances)
                .HasForeignKey(a => a.ClassGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Attendance>()
                .HasOne(a => a.User)
                .WithMany(u => u.Attendances)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ModelStatusModified<TEntity>(EntityTypeBuilder<TEntity> entity) where TEntity : class, IStatusModified
        {
            entity.HasQueryFilter(x => !x.Deleted);
            entity.Property(x => x.DateDeleted).HasColumnType("datetime2(2)");
            entity.Property(x => x.DateRegistration).HasColumnType("datetime2(2)");
            entity.Property(x => x.LastModificationDate).HasColumnType("datetime2(2)");
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            FillModifiedStatus();
            return base.SaveChanges();
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            FillModifiedStatus();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void FillModifiedStatus()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.Entity != null
                        && typeof(IStatusModified).IsAssignableFrom(e.Entity.GetType())))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DateRegistration").CurrentValue = DateTime.Now;
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Property("LastModificationDate").CurrentValue = DateTime.Now;
                    entry.Property("DateRegistration").IsModified = false;
                }
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Property("DateDeleted").CurrentValue = DateTime.Now;
                    entry.Property("Deleted").CurrentValue = true;
                    entry.Property("DateRegistration").IsModified = false;
                }
            }
        }

    }
}
