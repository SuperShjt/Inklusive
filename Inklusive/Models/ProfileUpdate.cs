namespace Inklusive.Models
{
    public class ProfileUpdate
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public string UpdatedField { get; set; } 
        public bool IsApproved { get; set; } = false; 
        public DateTime DateRequested { get; set; } 
        public DateTime? DateApproved { get; set; } 

        public User User { get; set; } 
    }
}
