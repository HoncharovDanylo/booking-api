namespace booking_api.Models;

public class Reservation : BaseEntity
{
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    
    
    public int UserId { get; set; }
    public User User { get; set; }
    
    public int ApartmentId { get; set; }
    public Apartment Apartment { get; set; }
    
    
    
}