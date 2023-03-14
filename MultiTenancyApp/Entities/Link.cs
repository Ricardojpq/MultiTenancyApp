using Microsoft.AspNetCore.Identity;
using MultiTenancyApp.Entities.Interfaces;

namespace MultiTenancyApp.Entities
{
    public class Link : ICommunEntity
    {
        public string UserId { get; set; }
        public Guid CompanyId { get; set; }
        public LinkStatus Status { get; set; }
        public Company Company { get; set; }
        public IdentityUser User { get; set; }
    }
}
