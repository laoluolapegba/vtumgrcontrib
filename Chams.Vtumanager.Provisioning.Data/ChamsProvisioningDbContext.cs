using Chams.Vtumanager.Provisioning.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;


namespace Chams.Vtumanager.Provisioning.Data
{
    public class ChamsProvisioningDbContext :DbContext
    {
        private readonly string _connectionString;
        private readonly  IConfiguration _config;

        public ChamsProvisioningDbContext(DbContextOptions<ChamsProvisioningDbContext> options) : base(options)
        {
            //reducing database "chatter" in code first
            //step 1: turn off initialization -https://romiller.com/2014/06/10/reducing-code-first-database-chatter/
            //Database.SetInitializer<BodcContext>(null);
        }

        public ChamsProvisioningDbContext()
        {
           
        }
        private void OnEntityUpdating()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is ITrackable trackable)
                {
                    var now = DateTime.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.UpdatedAt = now;
                            break;
                        case EntityState.Added:
                            trackable.CreatedAt = now;
                            trackable.UpdatedAt = now;
                            break;
                    }
                }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseOracle(@"Data Source=10.161.10.195:1525/epin;User ID=epin;Password=emts818!", options => options
                //.UseOracleSQLCompatibility("11"));
                    

            }
            
        }
        public override int SaveChanges()
        {
            OnEntityUpdating();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnEntityUpdating();
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            

            //builder.Entity<HpinPinMailerJobs>().ToTable("HY_PIN_MAILER_JOBS");
            //builder.Entity<HpinPinMailerJobsDetail>().ToTable("HY_PIN_MAILER_JOBS_DETAIL");

            //one to many
            //builder.Entity<PinMailerJob>().HasMany(cg => cg.PinJobetails).WithOne(bb => bb.MailerJob);
            //builder.Entity<SalesOrder>().HasMany(cg => cg.OrderDetails).WithOne(bb => bb.SalesOrder);
            //builder.Entity<SalesOrder>().HasMany(cg => cg.PinMailerJobs).WithOne(bb => bb.SalesOrder);


            // builder.Entity<SalesOrderDetail>()
            //.HasOne(p => p.SalesOrder)
            //.WithMany(b => b.OrderDetails);
            // base.OnModelCreating(builder);

            // builder.Entity<PinMailerJobsDetail>()
            //.HasOne(p => p.MailerJob)
            //.WithMany(b => b.Jobetails);
            // base.OnModelCreating(builder);

            // builder.Entity<PinMailerJob>()
            //.HasOne(p => p.SalesOrder)
            //.WithMany(b => b.MailerJobs);
            // base.OnModelCreating(builder);

        }

        public void SeedData()
        {
            SeedRoles();
        }
        private void SeedRoles()
        {
            //var roles = new List<string>() { "User", "Admin" };

            //roles.ForEach((role) =>
            //{
            //    var exists = Roles.Any(x => x.Name == role);

            //    if (!exists)
            //    {
            //        Roles.Add(new Role
            //        {
            //            Id = Guid.NewGuid().ToString(),
            //            Name = role,
            //            NormalizedName = role.ToUpper(),
            //            ConcurrencyStamp = Guid.NewGuid().ToString()
            //        });
            //    }
            //});
        }
        public static class ModelBuilderExtensions
        {
            //public static void SetupEnumStringConverters(this ModelBuilder builder)
            //{
            //    foreach (var entityType in builder.Model.GetEntityTypes())
            //    {
            //        foreach (var property in entityType.GetProperties())
            //        {
            //            if (property.ClrType.IsEnum)
            //            {
            //                builder.Entity(entityType.Name)
            //                    .Property(property.Name)
            //                    .HasConversion<string>();
            //            }
            //        }
            //    }
            //}
        }
    }
}

