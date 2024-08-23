namespace Entities.DTOs
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public Vehicle? VehicleDetails { get; set; }
        public ICollection<Rating>? Ratings { get; set; }
        public string PasswordHash { get; set; }
        public string VerificationStatus { get; set; } // Possible values: "Pending", "Approved", "Rejected"
        public ICollection<VerificationDocument>? VerificationDocuments { get; set; } // New
        public DateTime? DocumentUploadDate { get; set; } // Nullable date
    }

    public class Vehicle
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string LicensePlate { get; set; }
    }

    public class VerificationDocument
    {
        public string DocumentType { get; set; }
        public string DocumentUrl { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
