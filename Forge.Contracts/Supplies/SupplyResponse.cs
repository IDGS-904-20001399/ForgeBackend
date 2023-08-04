namespace Forge.Contracts.Supplies
{
    public record SupplyResponse(
        int Id,
        string Name,
        double cost,
        string buy_unit,
        string use_unit,
        double equivalence,
        string image
    );
}