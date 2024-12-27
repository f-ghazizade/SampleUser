using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [StringLength(20)]
        public required string UserName { get; set; }

        [StringLength(72)]
        public required string Password { get; set; }
    }
}