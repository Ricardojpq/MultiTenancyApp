using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MultiTenancyApp.Extensions;
using MultiTenancyApp.Services.Interfaces;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace MultiTenancyApp.Persistence
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private string tenantId;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IServiceTenant serviceTenant) 
                : base(options)
        {
            tenantId = serviceTenant.GetTenant();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries().Where(e=> e.State == EntityState.Added
                && e.Entity is ITenantEntity))
            {
                if (string.IsNullOrEmpty(tenantId))
                {
                    throw new Exception("TenantId not found at record creation time.");
                }

                var entity = item.Entity as ITenantEntity;
                entity!.TenantId = tenantId;
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                var type = entity.ClrType;

                if (typeof(ITenantEntity).IsAssignableFrom(type))
                {
                    var method = typeof(ApplicationDbContext).
                        GetMethod(nameof(SetGlobalTenantFilter), 
                        BindingFlags.NonPublic | BindingFlags.Static)?.MakeGenericMethod(type);

                    var filter = method?.Invoke(null, new object[] { this })!;
                    entity.SetQueryFilter((LambdaExpression)filter);
                    entity.AddIndex(entity.FindProperty(nameof(ITenantEntity.TenantId))!);
                }else if (type.SkipTenantValidation())
                {
                    continue;
                }
                else
                {
                    throw new Exception($"The {entity} entity has not been marked as a tenant or commun entity");
                }
            }
        }

        private static LambdaExpression SetGlobalTenantFilter<TEntity>(
            ApplicationDbContext context)
            where TEntity : class,ITenantEntity
        {
            Expression<Func<TEntity, bool>> filter = x => x.TenantId == context.tenantId;
            return filter;
        }
    }
}
