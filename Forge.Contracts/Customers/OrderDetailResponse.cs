namespace Forge.Contracts.Customers
{
//  id 	order_id 	product_id 	quantity 	price 	
    public class OrderDetailResponse{
        public OrderDetailResponse()
        {
        }

        public int Id {get; private set;}
        public int ProductId {get; private set;}
        public int Quantity {get; private set;}
        public double Price {get; private set;}
        public string ProductName {get; private set;}


    }

}