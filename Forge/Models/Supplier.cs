using ErrorOr;
using Forge.Contracts.Suppliers;

namespace Forge.Models
{
    public class Supplier
    {
        public int Id { get; set;}
        public string Name { get; private set;}
        public string Email { get; private set;}
        public string Phone { get; private set;}
        public int Status  { get; private set;}
        public Supplier(){}

        private Supplier(string name, string email, string phone){
            Name = name;
            Email = email;
            Phone = phone;
        }

        public static ErrorOr<Supplier> From(CreateSupplierRequest request){
            return Create(
                request.Name,
                request.Email,
                request.Phone
            );
        }

        public static ErrorOr<Supplier> From(int id, CreateSupplierRequest request){
            return Create(
                request.Name,
                request.Email,
                request.Phone,
                id
            );
        }

        public static ErrorOr<Supplier> Create(string name, string email, string phone, int? id = 0){
            return new Supplier(name, email, phone){Id = id ?? 0};
        }


    }
}