using ErrorOr;
using Forge.Contracts.Products;
using Forge.Contracts.Supplies;
using Forge.ServiceErrors;

namespace Forge.Models
{
    public class Product
    {
        public const int MinFieldLength = 3;
        public const double MinPrice = 0.1; 
        public int Id { get; set;}
        public string Name { get; private set;}
        public string Description { get; private set;}
        public string Width { get; private set;}
        public string Length { get; private set;}
        public string Height { get; private set;}
        public string Category { get; private set;}
        public double Price { get; private set;}
        public string Image { get; private set;}
        public int Status { get; private set;}
        public int Stock { get; private set;}
        public string InventoryStatus { get; private set;}

        public Product(){}
        private Product(
                       string name,
                       string description,
                       string width,
                       string length,
                       string height,
                       string category,
                       double price,
                       string image)
        {
            Name = name;
            Description = description;
            Width = width;
            Length = length;
            Height = height;
            Category = category;
            Price = price;
            Image = image;
        }

        public static ErrorOr<Product> From(CreateProductRequest request){
            return Create(
                request.Name,
                request.Description,
                request.Width,
                request.Length,
                request.Height,
                request.Category,
                request.Price,
                request.Image
            );
        }

        public static ErrorOr<Product> From(int id, UpsertProductRequest request){
            return Create(
                request.Name,
                request.Description,
                request.Width,
                request.Length,
                request.Height,
                request.Category,
                request.Price,
                request.Image,
                id
            );
        }

        public static ErrorOr<Product> Create(
                       string name,
                       string description,
                       string width,
                       string length,
                       string height,
                       string category,
                       double price,
                       string image,
                        int? id = 0
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

            return new Product(name, description, width, length, height, category, price, image){Id = id ?? 0};
        }

    }
}