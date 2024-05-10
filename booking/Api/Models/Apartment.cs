namespace booking_api.Models;

public class Apartment : BaseEntity
{
    
    public string Address { get; set; }
    
    
    // public int OwnerId { get; set; }
    // public User Owner { get; set; }
        
    public List<Reservation> Reservations { get; set; }
    public List<User> Tenants { get; set; }
}