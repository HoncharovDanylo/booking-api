namespace booking_api.Models;

public class User : BaseEntity
{
    public string Username { get; set; }
    
    public string Password { get; set; }
    
    public string Name { get; set; }
    public string Email { get; set; }
    
    // public List<Apartment> OwnedApartments { get; set; }
    
    public List<Reservation> Reservations { get; set; }
    public List<Apartment> ReservedApartments { get; set; }
}