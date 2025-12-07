using C__Memory_Managment_Delegate_Reflection.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace C__Memory_Managment_Delegate_Reflection
{
    internal class Program
    {
        static List<User> users = new();
        static List<Pizza> pizzas = new();
        static readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };
        
        const string UsersFile = "C:\\Users\\rashided\\Desktop\\C-Memory-Managment-Delegate-Reflection\\C# Memory Managment Delegate Reflection\\Data\\Users.json";
        const string ProductsFile = "C:\\Users\\rashided\\Desktop\\C-Memory-Managment-Delegate-Reflection\\C# Memory Managment Delegate Reflection\\Data\\Products.json";

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            
            Directory.CreateDirectory("Data");
            
            users = LoadList<User>(UsersFile);
            pizzas = LoadList<Pizza>(ProductsFile);

            while (true)
            {
                Console.WriteLine("\n=== Pizza Sifarişi ===");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Qeydiyyat");
                Console.WriteLine("0. Çıxış");
                Console.Write("Seçim: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Login();
                        break;
                    case "2":
                        Register();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Yanlış seçim");
                        break;
                }
            }
        }

        static List<T> LoadList<T>(string path)
        {
            if (!File.Exists(path)) return new List<T>();
            var json = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json)) return new List<T>();
            return JsonSerializer.Deserialize<List<T>>(json, jsonOptions) ?? new List<T>();
        }

        static void SaveList<T>(string path, List<T> list)
        {
            var json = JsonSerializer.Serialize(list, jsonOptions);
            File.WriteAllText(path, json);
        }

        static void Register()
        {
            Console.WriteLine("\n=== Qeydiyyat ===");
            Console.Write("Ad: ");
            string firstName = Console.ReadLine();
            Console.Write("Soyad: ");
            string lastName = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("Ad və soyad boş ola bilməz.");
                return;
            }

            string login;
            while (true)
            {
                Console.Write("Login (3-16 simvol): ");
                login = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(login) || login.Length < 3 || login.Length > 16)
                {
                    Console.WriteLine("Login uzunlığı 3-16 olmalıdır.");
                    continue;
                }
                if (users.Any(u => u.Login.Equals(login, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("Bu login artıq var.");
                    continue;
                }
                break;
            }

            string password;
            while (true)
            {
                Console.Write("Şifrə (6-16, 1 böyük, 1 kiçik, 1 rəqəm): ");
                password = Console.ReadLine();
                if (!ValidatePassword(password))
                {
                    Console.WriteLine("Şifrə tələblərə uyğun deyil.");
                    continue;
                }
                break;
            }

            var role = users.Count == 0 ? UserRole.Admin : UserRole.User;
            
            var newUser = new User
            {
                Id = users.Count == 0 ? 1 : users.Max(u => u.Id) + 1,
                FirstName = firstName,
                LastName = lastName,
                Login = login,
                Password = password,
                Role = role
            };
            users.Add(newUser);
            SaveList(UsersFile, users);
            
            if (role == UserRole.Admin)
                Console.WriteLine("✓ Qeydiyyat uğurla bitdi. Admin səlahiyyətləri aldınız.");
            else
                Console.WriteLine("✓ Qeydiyyat uğurla bitdi.");
        }

        static bool ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6 || password.Length > 16) 
                return false;
            if (!password.Any(char.IsUpper)) 
                return false;
            if (!password.Any(char.IsLower)) 
                return false;
            if (!password.Any(char.IsDigit)) 
                return false;
            return true;
        }

        static void Login()
        {
            Console.WriteLine("\n=== Login ===");
            Console.Write("Login: ");
            var login = Console.ReadLine();
            Console.Write("Şifrə: ");
            var password = Console.ReadLine();

            var user = users.FirstOrDefault(u =>
                u.Login.Equals(login, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);

            if (user == null)
            {
                Console.WriteLine("Yanlış login və ya şifrə.");
                return;
            }

            Console.WriteLine($"\nXoş gəldiniz, {user.FirstName} {user.LastName}!");

            var cart = new Dictionary<int, int>();

            if (user.IsAdmin())
                AdminMenu(cart);
            else
                UserMenu(cart);
        }

        static void UserMenu(Dictionary<int, int> cart)
        {
            while (true)
            {
                Console.WriteLine("\n=== User Menyusu ===");
                Console.WriteLine("1. Pizzalara bax və sifariş et");
                Console.WriteLine("2. Səbətə bax və təsdiqlə");
                Console.WriteLine("0. Çıxış");
                Console.Write("Seçim: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowPizzasAndOrder(cart);
                        break;
                    case "2":
                        ViewCartAndCheckout(cart);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Yanlış seçim.");
                        break;
                }
            }
        }

        static void AdminMenu(Dictionary<int, int> cart)
        {
            while (true)
            {
                Console.WriteLine("\n=== Admin Menyusu ===");
                Console.WriteLine("1. Pizzalara bax və sifariş et");
                Console.WriteLine("2. Səbətə bax və təsdiqlə");
                Console.WriteLine("3. Pizza idarəetməsi");
                Console.WriteLine("4. İstifadəçi idarəetməsi");
                Console.WriteLine("0. Çıxış");
                Console.Write("Seçim: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowPizzasAndOrder(cart);
                        break;
                    case "2":
                        ViewCartAndCheckout(cart);
                        break;
                    case "3":
                        ManagePizzas();
                        break;
                    case "4":
                        ManageUsers();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Yanlış seçim.");
                        break;
                }
            }
        }

        static void ShowPizzasAndOrder(Dictionary<int, int> cart)
        {
            while (true)
            {
                Console.WriteLine("\n=== Pizzalar ===");
                if (!pizzas.Any())
                {
                    Console.WriteLine("Pizza yoxdur.");
                    Console.WriteLine("\n0. Geri");
                    if (Console.ReadLine() == "0") return;
                    continue;
                }

                foreach (var p in pizzas)
                {
                    Console.WriteLine($"\n{p.Id}. {p.Name} - {p.Price:F2} AZN");
                    Console.WriteLine($"   İnqredientlər: {string.Join(", ", p.Ingredients)}");
                }

                Console.WriteLine("\n0. Geri");
                Console.Write("Səbətə əlavə etmək üçün pizza Id: ");
                var input = Console.ReadLine();

                if (input == "0") return;

                if (!int.TryParse(input, out var id))
                {
                    Console.WriteLine("Yanlış Id.");
                    continue;
                }

                var pizza = pizzas.FirstOrDefault(x => x.Id == id);
                if (pizza == null)
                {
                    Console.WriteLine("Bu Id-li pizza tapılmadı.");
                    continue;
                }

                Console.Write("Say: ");
                if (!int.TryParse(Console.ReadLine(), out var count) || count <= 0)
                {
                    Console.WriteLine("Yanlış say.");
                    continue;
                }

                if (cart.ContainsKey(pizza.Id))
                    cart[pizza.Id] += count;
                else
                    cart[pizza.Id] = count;

                Console.WriteLine($"✓ {pizza.Name} x{count} səbətə əlavə olundu.");
            }
        }

        static void ViewCartAndCheckout(Dictionary<int, int> cart)
        {
            if (!cart.Any())
            {
                Console.WriteLine("\nSəbət boşdur.");
                return;
            }

            decimal total = 0;
            Console.WriteLine("\n=== Səbətiniz ===");
            foreach (var kv in cart)
            {
                var pizza = pizzas.FirstOrDefault(p => p.Id == kv.Key);
                if (pizza == null) continue;
                var lineTotal = pizza.Price * kv.Value;
                total += lineTotal;
                Console.WriteLine($"{pizza.Name} x {kv.Value} = {lineTotal:F2} AZN");
            }

            Console.WriteLine($"\nÜmumi məbləğ: {total:F2} AZN");
            Console.Write("\nSifarişi təsdiq edirsiniz? (y/n): ");
            var confirm = Console.ReadLine()?.ToLower();
            
            if (confirm == "y" || confirm == "yes")
            {
                Console.Write("Çatdırılma ünvanı: ");
                Console.ReadLine();
                Console.Write("Telefon nömrəsi: ");
                Console.ReadLine();
                
                Console.WriteLine("\n✓ Sifariş qəbul olundu. Təşəkkürlər!");
                cart.Clear();
            }
            else
            {
                Console.WriteLine("Sifariş ləğv edildi.");
            }
        }

        static void ManagePizzas()
        {
            while (true)
            {
                Console.WriteLine("\n=== Pizza İdarəetməsi ===");
                Console.WriteLine("1. Bütün pizzalara bax");
                Console.WriteLine("2. Yeni pizza əlavə et");
                Console.WriteLine("3. Pizza düzəliş et");
                Console.WriteLine("4. Pizza sil");
                Console.WriteLine("0. Geri");
                Console.Write("Seçim: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllPizzas();
                        break;
                    case "2":
                        AddPizza();
                        break;
                    case "3":
                        EditPizza();
                        break;
                    case "4":
                        DeletePizza();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Yanlış seçim.");
                        break;
                }
            }
        }

        static void ShowAllPizzas()
        {
            Console.WriteLine("\n=== Bütün Pizzalar ===");
            if (!pizzas.Any())
            {
                Console.WriteLine("Pizza yoxdur.");
                return;
            }
            foreach (var p in pizzas)
            {
                Console.WriteLine($"\n{p.Id}. {p.Name} - {p.Price:F2} AZN");
                Console.WriteLine($"   İnqredientlər: {string.Join(", ", p.Ingredients)}");
            }
        }

        static void AddPizza()
        {
            Console.WriteLine("\n=== Yeni Pizza ===");
            Console.Write("Pizza adı: ");
            var name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Ad boş ola bilməz.");
                return;
            }

            Console.Write("Qiymət: ");
            if (!decimal.TryParse(Console.ReadLine(), out var price) || price <= 0)
            {
                Console.WriteLine("Yanlış qiymət.");
                return;
            }

            Console.Write("İnqredientlər (vergüllə ayrılmış): ");
            var ingStr = Console.ReadLine();
            var ingredients = ingStr?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(x => x.Trim())
                                    .Where(x => !string.IsNullOrWhiteSpace(x))
                                    .ToList() ?? new List<string>();

            var pizza = new Pizza
            {
                Id = pizzas.Count == 0 ? 1 : pizzas.Max(p => p.Id) + 1,
                Name = name,
                Price = price,
                Ingredients = ingredients
            };
            pizzas.Add(pizza);
            SaveList(ProductsFile, pizzas);
            Console.WriteLine("✓ Pizza əlavə olundu.");
        }

        static void EditPizza()
        {
            Console.Write("\nDüzəliş üçün pizza Id: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Yanlış Id.");
                return;
            }
            var p = pizzas.FirstOrDefault(x => x.Id == id);
            if (p == null)
            {
                Console.WriteLine("Tapılmadı.");
                return;
            }

            Console.WriteLine($"Cari: {p.Name} - {p.Price:F2} AZN");
            
            Console.Write($"Yeni ad (Enter = dəyişmə): ");
            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
                p.Name = name;

            Console.Write($"Yeni qiymət (Enter = dəyişmə): ");
            var priceStr = Console.ReadLine();
            if (decimal.TryParse(priceStr, out var price) && price > 0)
                p.Price = price;

            Console.Write("Yeni inqredientlər (Enter = dəyişmə): ");
            var ingStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(ingStr))
            {
                p.Ingredients = ingStr.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(x => x.Trim())
                                      .Where(x => !string.IsNullOrWhiteSpace(x))
                                      .ToList();
            }

            SaveList(ProductsFile, pizzas);
            Console.WriteLine("✓ Düzəliş saxlanıldı.");
        }

        static void DeletePizza()
        {
            Console.Write("\nSilinəcək pizza Id: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Yanlış Id.");
                return;
            }
            var p = pizzas.FirstOrDefault(x => x.Id == id);
            if (p == null)
            {
                Console.WriteLine("Tapılmadı.");
                return;
            }

            Console.Write($"'{p.Name}' silmək istədiyinizdən əminsiniz? (y/n): ");
            var confirm = Console.ReadLine()?.ToLower();
            if (confirm != "y" && confirm != "yes")
            {
                Console.WriteLine("Ləğv edildi.");
                return;
            }

            pizzas.Remove(p);
            SaveList(ProductsFile, pizzas);
            Console.WriteLine("✓ Silindi.");
        }

        static void ManageUsers()
        {
            while (true)
            {
                Console.WriteLine("\n=== İstifadəçi İdarəetməsi ===");
                Console.WriteLine("1. Bütün istifadəçilərə bax");
                Console.WriteLine("2. Yeni istifadəçi əlavə et");
                Console.WriteLine("3. İstifadəçi rolunu dəyiş");
                Console.WriteLine("4. İstifadəçi sil");
                Console.WriteLine("0. Geri");
                Console.Write("Seçim: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllUsers();
                        break;
                    case "2":
                        AddUser();
                        break;
                    case "3":
                        ChangeUserRole();
                        break;
                    case "4":
                        DeleteUser();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Yanlış seçim.");
                        break;
                }
            }
        }

        static void ShowAllUsers()
        {
            Console.WriteLine("\n=== Bütün İstifadəçilər ===");
            if (!users.Any())
            {
                Console.WriteLine("İstifadəçi yoxdur.");
                return;
            }
            foreach (var u in users)
                Console.WriteLine($"{u.Id}. {u.FirstName} {u.LastName} [@{u.Login}] - {u.Role}");
        }

        static void AddUser()
        {
            Console.WriteLine("\n=== Yeni İstifadəçi ===");
            Console.Write("Ad: ");
            var firstName = Console.ReadLine();
            Console.Write("Soyad: ");
            var lastName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("Ad və soyad boş ola bilməz.");
                return;
            }

            string login;
            while (true)
            {
                Console.Write("Login (3-16): ");
                login = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(login) || login.Length < 3 || login.Length > 16)
                {
                    Console.WriteLine("Login uzunlığı 3-16 olmalıdır.");
                    continue;
                }
                if (users.Any(u => u.Login.Equals(login, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("Bu login artıq var.");
                    continue;
                }
                break;
            }

            string password;
            while (true)
            {
                Console.Write("Şifrə (6-16, 1 böyük, 1 kiçik, 1 rəqəm): ");
                password = Console.ReadLine();
                if (!ValidatePassword(password))
                {
                    Console.WriteLine("Şifrə tələblərə uyğun deyil.");
                    continue;
                }
                break;
            }

            Console.Write("Admin rolu verək? (y/n): ");
            var roleChoice = Console.ReadLine()?.ToLower();
            var role = (roleChoice == "y" || roleChoice == "yes") ? UserRole.Admin : UserRole.User;

            var newUser = new User
            {
                Id = users.Count == 0 ? 1 : users.Max(u => u.Id) + 1,
                FirstName = firstName,
                LastName = lastName,
                Login = login,
                Password = password,
                Role = role
            };
            users.Add(newUser);
            SaveList(UsersFile, users);
            Console.WriteLine($"✓ İstifadəçi {role} rolu ilə əlavə olundu.");
        }

        static void ChangeUserRole()
        {
            Console.Write("\nİstifadəçi Id: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Yanlış Id.");
                return;
            }
            var u = users.FirstOrDefault(x => x.Id == id);
            if (u == null)
            {
                Console.WriteLine("Tapılmadı.");
                return;
            }

            Console.WriteLine($"Hazırki: {u.FirstName} {u.LastName} - {u.Role}");
            Console.Write("Admin rolu verək? (y/n): ");
            var roleChoice = Console.ReadLine()?.ToLower();
            u.Role = (roleChoice == "y" || roleChoice == "yes") ? UserRole.Admin : UserRole.User;
            SaveList(UsersFile, users);
            Console.WriteLine($"✓ Rol {u.Role} olaraq dəyişdirildi.");
        }

        static void DeleteUser()
        {
            Console.Write("\nSilinəcək istifadəçi Id: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Yanlış Id.");
                return;
            }
            var u = users.FirstOrDefault(x => x.Id == id);
            if (u == null)
            {
                Console.WriteLine("Tapılmadı.");
                return;
            }

            Console.Write($"'{u.FirstName} {u.LastName}' silmək istədiyinizdən əminsiniz? (y/n): ");
            var confirm = Console.ReadLine()?.ToLower();
            if (confirm != "y" && confirm != "yes")
            {
                Console.WriteLine("Ləğv edildi.");
                return;
            }

            users.Remove(u);
            SaveList(UsersFile, users);
            Console.WriteLine("✓ Silindi.");
        }
    }
}
...