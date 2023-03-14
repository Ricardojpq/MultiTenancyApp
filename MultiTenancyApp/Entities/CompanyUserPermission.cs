using Microsoft.AspNetCore.Identity;
using MultiTenancyApp.Entities.Interfaces;
using MultiTenancyApp.Utils;

namespace MultiTenancyApp.Entities
{
    public class CompanyUserPermission : ICommunEntity
    {
        public string UserId { get; set; }
        public Guid CompanyId { get; set; }
        public Permissions Permission { get; set; }
        public IdentityUser? User { get; set; }
        public Company? Company { get; set; }
    }
}
