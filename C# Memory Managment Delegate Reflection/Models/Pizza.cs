namespace C__Memory_Managment_Delegate_Reflection.Models
{
    internal class Pizza
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<string> Ingredients { get; set; }=new List<string>();
    }
}
