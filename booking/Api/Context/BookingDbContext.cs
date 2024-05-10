using booking_api.Encrypting;
using booking_api.Models;
using Microsoft.EntityFrameworkCore;

namespace booking_api.Context;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Apartment> Apartments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Specifying the relationship between entities
        //
        // modelBuilder.Entity<User>()
        //     .HasMany(x => x.OwnedApartments)
        //     .WithOne(x => x.Owner)
        //     .HasForeignKey(x => x.OwnerId)
        //     .OnDelete(DeleteBehavior.Cascade);

        // modelBuilder.Entity<User>()
        //     .HasMany(x => x.Reservations)
        //     .WithOne(x => x.User)
        //     .HasForeignKey(x => x.UserId)
        //     .OnDelete(DeleteBehavior.Cascade);
        //
        // modelBuilder.Entity<Apartment>()
        //     .HasMany(x => x.Reservations)
        //     .WithOne(x => x.Apartment)
        //     .HasForeignKey(x => x.ApartmentId)
        //     .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(x => x.ReservedApartments)
            .WithMany(x => x.Tenants)
            .UsingEntity<Reservation>(
                j=> j
                    .HasOne(x=>x.Apartment)
                    .WithMany(x=>x.Reservations)
                    .HasForeignKey(x=>x.ApartmentId),
                j=> j
                    .HasOne(x=>x.User)
                    .WithMany(x=>x.Reservations)
                    .HasForeignKey(x=>x.UserId),
                j =>
                {
                    j.Property(x => x.CheckOut).IsRequired().HasDefaultValue(DateOnly.FromDateTime(DateTime.Now));
                    j.Property(x => x.CheckIn).IsRequired().HasDefaultValue(DateOnly.FromDateTime(DateTime.Now));
                    j.HasKey(x => new { x.UserId, x.ApartmentId });
                    j.ToTable("Reservations");
                });
        
        //Specifying constraints of the entities
        modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<User>().Property(x => x.Username).IsRequired().HasMaxLength(20);
        modelBuilder.Entity<User>().Property(x => x.Password).IsRequired();
        modelBuilder.Entity<User>().Property(x => x.Name).IsRequired().HasMaxLength(20);
        modelBuilder.Entity<User>().Property(x => x.Email).IsRequired().HasMaxLength(50);

        modelBuilder.Entity<Apartment>().Property(x => x.Address).IsRequired().HasMaxLength(100);
        // modelBuilder.Entity<Apartment>().Property(x => x.OwnerId).IsRequired();

        modelBuilder.Entity<Reservation>().Property(x => x.ApartmentId).IsRequired();
        modelBuilder.Entity<Reservation>().Property(x => x.UserId).IsRequired();
        modelBuilder.Entity<Reservation>().Property(x => x.CheckIn).IsRequired();
        modelBuilder.Entity<Reservation>().Property(x => x.CheckOut).IsRequired();
        
        // Encrypting password
        modelBuilder.Entity<User>().Property(x => x.Password)
            .HasConversion(password => Encrypter.GetHash(password),
                hashedPassword=> hashedPassword);
        base.OnModelCreating(modelBuilder);
    }
}