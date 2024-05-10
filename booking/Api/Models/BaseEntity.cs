using System.ComponentModel.DataAnnotations;

namespace booking_api.Models;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
}