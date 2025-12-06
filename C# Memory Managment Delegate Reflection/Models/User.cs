namespace C__Memory_Managment_Delegate_Reflection.Models
{
    public enum UserRole
    {
        User,
        Admin
    }

    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        
        public User() { }
        
        public User(string firstName, string lastName, string login, string password, UserRole role)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            
            if (login.Length < 3 || login.Length > 16)
            {
                throw new ArgumentException("Login length must be between 3 and 16 characters.");
            }
            Login = login;
            
            if (!ValidatePassword(password))
            {
                throw new ArgumentException("Password must be 6-16 characters with at least 1 uppercase, 1 lowercase, and 1 digit.");
            }
            Password = password;
            
            Role = role;
        }
        
        public bool IsAdmin()
        {
            return Role == UserRole.Admin;
        }
        
        public override string ToString()
        {
            return $"Name: {FirstName} {LastName}, Login: {Login}, Role: {Role}";
        }
        
        private static bool ValidatePassword(string password)
        {
            if (password == null || password.Length < 6 || password.Length > 16) 
                return false;
            if (!password.Any(char.IsUpper)) 
                return false;
            if (!password.Any(char.IsLower)) 
                return false;
            if (!password.Any(char.IsDigit)) 
                return false;
            return true;
        }
        // End of User class
    }
}
