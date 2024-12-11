namespace ASP.NET.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public User(UserRegisterModel newUser)
        {
            Id = Guid.NewGuid();
            CreateTime = DateTime.Now;
            FullName = newUser.FullName;
            BirthDate = newUser.BirthDate;
            Gender = newUser.Gender;
            PhoneNumber = newUser.PhoneNumber;
            Email = newUser.Email;
            Password = newUser.Password;
        }
    }
}
