using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRS_Backend.Models
{
    [Index("Username", IsUnique = true)]
    public class User
    {
        public int Id { get; set; } // PK of the User table

        [StringLength(30)]
        public string Username { get; set; } = string.Empty;

        [StringLength(30)]
        public string Password { get; set; } = string.Empty;

        [StringLength(30)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(30)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(12)]
        public string? PhoneNumber { get; set; }

        [StringLength(255)]
        public string? Email { get; set; }

        public bool IsReviewer { get; set; }

        public bool IsAdmin { get; set; }
    }
}
