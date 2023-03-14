using System.ComponentModel.DataAnnotations;

namespace MultiTenancyApp.Entities
{
    public class MultiTenancyBaseEntity : BaseEntity
    {
        [Required]
        [MaxLength(80)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        public new bool IsDeleted { get; set; }

        public MultiTenancyBaseEntity()
        {
            CreatedDate = DateTime.Now;
            MaintenanceUser = "";
        }

        public override string ToString()
        {
            return $"Entity Info: Id: {Id}, Name: {Name}, Description: {Description}";
        }
    }
}
