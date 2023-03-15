using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRS_Backend.Models
{
    [Index("Code", IsUnique = true)]
    public class Vendor
    {
        public int Id { get; set; } // PK for Vendor table

        [StringLength(30)]
        public string Code { get; set; } = string.Empty;

        [StringLength(30)]
        public string Name { get; set; } = string.Empty;

        [StringLength(30)]
        public string Address { get; set; } = string.Empty;

        [StringLength(30)]
        public string City { get; set; } = string.Empty;

        [StringLength(2)]
        public string State { get; set; } = string.Empty;

        [StringLength(5)]
        public string ZipCode { get; set; } = string.Empty;

        [StringLength(12)]
        public string? PhoneNumber { get; set; }

        [StringLength(255)]
        public string? Email { get; set; }
    }
}
