using ErrorOr;
using Forge.Contracts.Supplies;
using Forge.ServiceErrors;

namespace Forge.Models
{
    public class Supply
    {
        public const int MinFieldLength = 3;
        public const double MinPrice = 0.1; 
        public int Id { get; set;}
        public string Name { get; private set;}
        public double Cost { get; private set;}

        public string BuyUnit { get; private set;}
        public string UseUnit { get; private set;}
        public double Equivalence{get; private set;}
        public string Image { get; private set;}

        public int Status { get; private set;}
        public double Stock{get; set;}
        public string InventoryStatus{get; set;}

        public Supply(){}

        private Supply(
                       string name,
                       double cost,
                       string buyUnit,
                       string useUnit,
                       double equivalence,
                       string image)
        {
            Name = name;
            Cost = cost;
            BuyUnit = buyUnit;
            UseUnit = useUnit;
            Equivalence = equivalence;
            Image = image;
        }

        public static ErrorOr<Supply> From(CreateSupplyRequest request){
            return Create(
                request.Name,
                request.Cost,
                request.BuyUnit,
                request.UseUnit,
                request.Equivalence,
                request.Image
            );
        }

        public static ErrorOr<Supply> From(int id, CreateSupplyRequest request){
            return Create(
                request.Name,
                request.Cost,
                request.BuyUnit,
                request.UseUnit,
                request.Equivalence,
                request.Image,
                id
            );
        }

        public static ErrorOr<Supply> Create(
                       string name,
                       double cost,
                       string buyUnit,
                       string useUnit,
                       double equivalence,
                       string image,
                        int? id = 0
                       )
        {
            List<Error> errors = new ();
            if (name.Length < MinFieldLength){
                errors.Add (Errors.Supply.InvalidName);
            }

            if (cost < MinPrice){
                errors.Add (Errors.Supply.InvalidPrice);
            }

            if (errors.Count > 0){
                return errors;
            }

            return new Supply(name, cost, buyUnit, useUnit, equivalence, image){Id = id ?? 0};
        }


    }
}