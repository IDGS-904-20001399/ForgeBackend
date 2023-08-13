using ErrorOr;
using Forge.Contracts.Customers;

namespace Forge.Models
{
    public class Customer
    {
        public int Id{get;  set;}
        public string Email{get; private set;}
        public string Password {get; private set;}
        public string Names {get; private set;}
        public string Lastnames {get; private set;}
        public string Address {get; private set;}
        public string Phone {get; private set;}
        public int UserId{get; set;}
        public int RoleId  { get; private set;}
        public string Role  { get; private set;}

        public Customer()
        {
            RoleId = 4;
            Role = "Customer";
        }

        private Customer(
            string email, 
            string password, 
            string names, 
            string lastNames,
            string address,
            string phone
        ){
            Email = email;
            Password = password;
            Names = names;
            Lastnames = lastNames;
            Address = address;
            Phone = phone;
            RoleId = 4;
            Role = "Customer";
        }

        public static ErrorOr<Customer> From(CreateCustomerRequest request){
            return Create(
                request.Email,
                request.Password,
                request.Names,
                request.Lastnames,
                request.Address,
                request.Phone
            );
        }

        public static ErrorOr<Customer> Create(
            string email, 
            string password, 
            string names, 
            string lastNames,
            string address,
            string phone,
            int? id = 0
        ){
            return new Customer(email, password, names, lastNames, address, phone){Id = id ?? 0};
        }
    }
}