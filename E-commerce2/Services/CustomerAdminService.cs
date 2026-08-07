using ECommerce2.DTOs.Responses;
using ECommerce2.Services.Interfaces;
using ECommerce2.Models;
using ECommerce2.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ECommerce2.DataAccess;

namespace ECommerce2.Services
{
    public class CustomerAdminService : ICustomerAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;

        public CustomerAdminService(UserManager<User> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<PaginatedList<AdminCustomerSummaryDto>> GetAdminCustomersAsync(int pageIndex, int pageSize, string? searchQuery)
        {
            var query = _userManager.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var lowerSearch = searchQuery.ToLower();
                query = query.Where(u => 
                    (u.FName + " " + u.LName).ToLower().Contains(lowerSearch) ||
                    u.Email!.ToLower().Contains(lowerSearch) ||
                    u.PhoneNumber!.Contains(lowerSearch));
            }

            var users = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            var totalCount = await query.CountAsync();
            var userIds = users.Select(u => u.Id).ToList();

            var orderStats = await _context.Set<Order>()
                .Where(o => userIds.Contains(o.UserId))
                .GroupBy(o => o.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    TotalOrders = g.Count(),
                    TotalSpent = g.Sum(o => o.TotalPrice)
                })
                .ToDictionaryAsync(x => x.UserId);

            var items = users.Select(u => new AdminCustomerSummaryDto
            {
                Id = u.Id,
                FullName = $"{u.FName} {u.LName}",
                Email = u.Email ?? string.Empty,
                PhoneNumber = u.PhoneNumber ?? string.Empty,
                TotalOrders = orderStats.ContainsKey(u.Id) ? orderStats[u.Id].TotalOrders : 0,
                TotalSpent = orderStats.ContainsKey(u.Id) ? orderStats[u.Id].TotalSpent : 0,
                RegisteredAt = u.CreatedAt
            }).ToList();

            return new PaginatedList<AdminCustomerSummaryDto>(items, totalCount, pageIndex, pageSize);
        }
    }
}
