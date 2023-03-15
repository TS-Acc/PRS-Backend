using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PRS_Backend.Models
{
    public class RequestLine
    {
        public int Id { get; set; }

        public int RequestId { get; set; } // FK relationship to the Request table
        [JsonIgnore]
        public virtual Request? Requests { get; set; }

        public int ProductId { get; set; } // FK relationship to the Products table
        public virtual Product? Products { get; set; }

        public int Quantity { get; set; } = 1;


    }
}
