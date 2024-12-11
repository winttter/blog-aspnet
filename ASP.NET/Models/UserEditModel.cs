namespace ASP.NET.Models
{
    public class UserEditModel
    {
        public string FullName { get; set; }
        //[Required]
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        //[Required]
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
        //[Required]
    }
}
