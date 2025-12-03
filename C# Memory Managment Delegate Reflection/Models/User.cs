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
            Login = login;
            Password = password;
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
    }
}
