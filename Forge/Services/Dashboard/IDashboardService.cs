using Forge.Contracts.Dashboard;

namespace Forge.Services.Dashboard
{
    public interface IDashboardService
    {
        List<SupplySummary> GetSupplySummary();
        List<SupplySummary> GetSupplySummaryTotal();
        List<SupplyItem> GetSupplyInventory();
        List<Statistics> GetStatistics();
        List<StatisticsSummary> GetStatisticsSummary();
        List<ProductSummary> GetProductSummaries();
        List<ProductInventory> GetProductInventory();
        List<ProductSold> GetProductsSold();
        List<ProductSold> GetProductsSoldTotal();
        List<MonthlyProductSold> GetMonthlyProductSolds();
        List<CustomerSummary> GetCustomerSummaries();
        List<CustomerSummary> GetCustomerSummariesTotal();
    }
}