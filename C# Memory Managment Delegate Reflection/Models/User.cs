namespace C__Memory_Managment_Delegate_Reflection.Models
{
    public enum User
    {
        Admin,
        Customer,
    }
    public class UserModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public User UserType { get; set; }
        
        public UserModel(string name, string surname, string login, string password, User userType)
        {
            Name = name;
            Surname = surname;
            if (login.Length<3 || login.Length>16)
            {
                throw new ArgumentException("Login length must be between 3 and 20 characters.");
            }
            else 
            {                 
                Login = login;
            }
            if (password.Length < 6 || password.Length > 16 && !password.Any(char.IsDigit) && !password.Any(char.IsUpper) && !password.Any(char.IsLower))
            {
                throw new ArgumentException("Password length must be between 6 and 20 characters.");
            }
            else
            {
                Password = password;
            }
            UserType = userType;
        }
        public bool IsAdmin()
        {
            return UserType == User.Admin;
        }
        public override string ToString()
        {
            return $"Name: {Name}, Surname: {Surname}, Login: {Login}, UserType: {UserType}";
        }
        public void UpdatePassword(string newPassword) //sil 
        {
            if (newPassword.Length < 6 || newPassword.Length > 16 && !newPassword.Any(char.IsDigit) && !newPassword.Any(char.IsUpper) && !newPassword.Any(char.IsLower))
            {
                throw new ArgumentException("Password length must be between 6 and 20 characters.");
            }
            else
            {
                Password = newPassword;
            }
        }
    }
    
}
