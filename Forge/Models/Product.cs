using ErrorOr;
using Forge.Contracts.Products;
using Forge.ServiceErrors;

namespace Forge.Models
{
    public class Product
    {
        public const int MinFieldLength = 3;
        public const double MinPrice = 0.1; 
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public string Category { get; }
        public double Price { get; }

        private Product(Guid id,
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

        public static ErrorOr<Product> From(CreateProductRequest request){
            return Create(
                request.Name,
                request.Description,
                request.Category,
                request.Price
            );
        }

        public static ErrorOr<Product> From(Guid id, UpsertProductRequest request){
            return Create(
                request.Name,
                request.Description,
                request.Category,
                request.Price,
                id
            );
        }

        public static ErrorOr<Product> Create(
                       string name,
                       string description,
                       string category,
                       double price,
                        Guid? id = null
        )
        {
            List<Error> errors = new ();
            if (name.Length < MinFieldLength){
                errors.Add (Errors.Product.InvalidName);
            }

            if (price < MinPrice){
                errors.Add(Errors.Product.InvalidPrice);
            }

            if (errors.Count > 0){
                return errors;
            }

            return new Product(id ?? Guid.NewGuid(), name, description, category, price);
        }

    }
}