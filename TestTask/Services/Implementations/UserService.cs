using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User> GetUser()
        {
            var userIdWithMaxTotal = await _dbContext.Orders
                .Where(order => order.Status == OrderStatus.Delivered && order.CreatedAt.Year == 2003)
                .GroupBy(order => order.UserId)
                .Select(group => new
                {
                    UserId = group.Key,
                    TotalDeliveredSum = group.Sum(order => order.Price * order.Quantity)
                })
                .OrderByDescending(g => g.TotalDeliveredSum)
                .Select(g => g.UserId)
                .FirstOrDefaultAsync();

            return await _dbContext.Users.FindAsync(userIdWithMaxTotal);
        }


        public async Task<List<User>> GetUsers()
        {
            return await _dbContext.Users
                           .Where(u => u.Orders
                               .Any(o => (o.Status == Enums.OrderStatus.Paid && o.CreatedAt.Year == 2010)))
                           .ToListAsync();
        }
    }
}
