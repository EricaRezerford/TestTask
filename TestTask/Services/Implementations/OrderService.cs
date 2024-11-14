using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _dbContext;
        public OrderService(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<Order> GetOrder()
        {
            return await _dbContext.Orders
                .Where(o => o.Quantity > 1)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetOrders()
        {
            return await _dbContext.Orders.Where(o => o.User.Status == 0).OrderBy(o  => o.CreatedAt).ToListAsync();
        }
    }
}
