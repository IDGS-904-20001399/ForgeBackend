namespace Forge.Models
{
    public class Product
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public string Category { get; }
        public double Price { get; }

        public Product(Guid id,
                       string name,
                       string description,
                       string category,
                       double price)
        {
            Id = id;
            Name = name;
            Description = description;
            Category = category;
            Price = price;
        }

    }
}