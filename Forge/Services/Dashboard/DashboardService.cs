using Forge.Contracts.Dashboard;
using System.Data;
using ErrorOr;
using Forge.Models;
using Forge.ServiceErrors;
using MySql.Data.MySqlClient;
using Dapper;
using System.Diagnostics;
using Forge.Contracts.Products;
using Forge.Contracts.Supplies;

namespace Forge.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {

        private readonly MySqlConnection _dbConnection;

        public DashboardService(MySqlConnection dbConnection)
        {
            _dbConnection = new MySqlConnection("Server=127.0.0.1;Database=dashboard_db;Uid=root;Pwd=;");
        }
        public List<CustomerSummary> GetCustomerSummaries()
        {
            string query = "SELECT * FROM customer_summary";
            var result = _dbConnection.Query<CustomerSummary>(query).ToList();
            return result;
        }

        public List<CustomerSummary> GetCustomerSummariesTotal()
        {
            string query = "SELECT cs.*, (SELECT SUM(quantity) from customer_summary where id = cs.id) totalQuantity, (SELECT SUM(price) from customer_summary where id = cs.id) totalPrice FROM customer_summary cs group by cs.user_id";
            var result = _dbConnection.Query<CustomerSummary>(query).ToList();
            return result;
        }

        public List<MonthlyProductSold> GetMonthlyProductSolds()
        {
            return _dbConnection.Query<MonthlyProductSold>("SELECT * FROM monthly_products_sold").ToList();
        }

        public List<ProductInventory> GetProductInventory()
        {
            return _dbConnection.Query<ProductInventory>("SELECT * FROM product_inventory").ToList();
        }

        public List<ProductSold> GetProductsSold()
        {
            var productsSold = _dbConnection.Query<ProductSold>("SELECT * FROM products_sold").ToList();
            return productsSold;
        }

        public List<ProductSold> GetProductsSoldTotal()
        {
            var productsSold = _dbConnection.Query<ProductSold>("SELECT ps.*, (SELECT SUM(units_sold) from products_sold where id = ps.id) totalUnitsSold, (SELECT SUM(earnings) from products_sold where id = ps.id) totalEarnings  FROM products_sold ps group by ps.product_id").ToList();
            return productsSold;
        }

        public List<ProductSummary> GetProductSummaries()
        {
            return _dbConnection.Query<ProductSummary>("SELECT * FROM product_summary").ToList();
        }

        public List<Statistics> GetStatistics()
        {
            return _dbConnection.Query<Statistics>("SELECT * FROM statistics").ToList();
        }

        public List<StatisticsSummary> GetStatisticsSummary()
        {
             var query = @"
                SELECT 
                    SUM(egress) totalEgress,
                    SUM(income) totalIncome,
                    (SELECT SUM(profits) FROM statistics) totalProfits
                FROM statistics";

            return _dbConnection.Query<StatisticsSummary>(query).ToList();
        }

        public List<SupplyItem> GetSupplyInventory()
        {
            return _dbConnection.Query<SupplyItem>("SELECT * FROM supply_inventory").ToList();
        }

        public List<SupplySummary> GetSupplySummary()
        {
            string query = "SELECT * FROM supply_summary";
            return _dbConnection.Query<SupplySummary>(query).ToList();
        }
    }
}