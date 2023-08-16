using Forge.Contracts.Dashboard;

namespace Forge.Services.Dashboard
{
    public interface IDashboardService
    {
        List<SupplySummary> GetSupplySummary();
        List<SupplyItem> GetSupplyInventory();
        List<Statistics> GetStatistics();
        List<ProductSummary> GetProductSummaries();
        List<ProductInventory> GetProductInventory();
        List<ProductSold> GetProductsSold();
        List<MonthlyProductSold> GetMonthlyProductSolds();
        List<CustomerSummary> GetCustomerSummaries();
        List<CustomerSummary> GetCustomerSummariesTotal();
    }
}