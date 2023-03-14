using System.ComponentModel.DataAnnotations;

namespace MultiTenancyApp.Entities
{
    public class BaseEntity
    {
        /// <summary>
        /// This class contains all the fields that every table in the database must have. Fields are:
        /// <para>Id</para>
        /// <para>IsActive</para>
        /// <para>CreateDate</para>
        /// <para>MaintenanceUser</para>
        /// </summary>

        [Key]
        public Guid Id { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        [MaxLength(80)]
        public string MaintenanceUser { get; set; }

        public bool IsDeleted { get; set; }
        public BaseEntity()
        {
            CreatedDate = DateTime.Now;
            MaintenanceUser = "";
        }

        public override string ToString()
        {
            return $"Entity Info: PK: {Id}, Active: {IsActive}, Created: {CreatedDate}";
        }
    }
}
