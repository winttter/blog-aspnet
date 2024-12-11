using ASP.NET.Models;

namespace ASP.NET.Services
{
    public interface IUserService
    {
        Task<TokenResponse> Register(UserRegisterModel model);
        //Task<TokenResponse> Login(LoginCredentials loginCredentials);
    }

    public class UserService : IUserService
    {
        //обращение к БД
        private readonly TestContext _context;

        public UserService(TestContext context)
        {
            _context = context;
        }

        public async Task<TokenResponse> Register(UserRegisterModel model)
        {
            User user = new User(model);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            TokenResponse token = new TokenResponse();
            return token;
        }

        /*public async Task<TokenResponse> Login(LoginCredentials loginCredentials)
        {
            User? user = await _context.Users.FindAsync(loginCredentials);
            if (user != null) {
                TokenResponse response = new TokenResponse();
                response.Token = 
            return ;
        }*/
    }

}
