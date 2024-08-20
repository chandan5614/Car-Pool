namespace Application.DTOs
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } // "student", "driver", "admin"
        public VehicleDetailsDto VehicleDetails { get; set; }
        public IEnumerable<RatingDto> Ratings { get; set; }
        public string Password { get; set; }
    }

    public class VehicleDetailsDto
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string LicensePlate { get; set; }
    }
}
