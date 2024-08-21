﻿namespace Entities.DTOs
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public Vehicle VehicleDetails { get; set; }
        public ICollection<Rating> Ratings { get; set; }
<<<<<<< HEAD
        public string PasswordHash { get; set; }
=======
>>>>>>> origin/dev
    }

    public class Vehicle
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string LicensePlate { get; set; }
    }
}