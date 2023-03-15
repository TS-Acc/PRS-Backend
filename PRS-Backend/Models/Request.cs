using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PRS_Backend.Models
{
    public class Request
    {
        public int Id { get; set; } // PK for Request table

        [StringLength(150)]
        public string Description { get; set; } = string.Empty;

        [StringLength(150)]
        public string Justification { get; set; } = string.Empty;

        [StringLength(150)]
        public string? RejectionReason { get; set; }

        [StringLength(20)]
        public string DeliveryMode { get; set; } = "Pickup";

        [StringLength(10)]
        public string Status { get; set; } = "NEW";

        [Column(TypeName = "decimal(11,2)")]
        public decimal Total { get; set; } = 0;

        public int UserId { get; set; } // FK relationship to the User table
        public virtual User? Users { get; set; }

        public virtual ICollection<RequestLine>? RequestLines { get; set; }
    }
}
