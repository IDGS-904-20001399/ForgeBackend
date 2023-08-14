namespace Forge.Contracts.Customers
{
    public class CustomerResponse
    {
        public CustomerResponse()
        {
        }

        public CustomerResponse(
            int id,
            string email,
            string names,
            string lastnames,
            string address,
            string phone
        )
        {
            Id = id;
            Email = email;
            Names = names;
            Lastnames = lastnames;
            Address = address;
            Phone = phone; 
        }

        public int Id {get; set;}
        public string Email {get; set;}
        public string Names {get; set;}
        public string Lastnames {get; set;}
        public string Address {get; set;}
        public string Phone {get; set;}


    }
}