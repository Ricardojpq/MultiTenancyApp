using Microsoft.AspNetCore.Identity;
using MultiTenancyApp.Entities.Interfaces;


namespace MultiTenancyApp.Entities
{
    public class Company : MultiTenancyBaseEntity, ICommunEntity
    {
        public string? UserCreatedId { get; set; }
        public IdentityUser UserCreated { get; set; } = null!;
        public List<CompanyUserPermission> CompanyUserPermissions { get; set; } = null!;
    }
}
