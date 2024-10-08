using MealSync.Application.Common.Repositories;
using MealSync.Domain.Entities;
using MealSync.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MealSync.Infrastructure.Persistence.Repositories;

public class FoodRepository : BaseRepository<Food>, IFoodRepository
{
    public FoodRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public Food GetByIdIncludeAllInfo(long id)
    {
        return DbSet.Include(f => f.PlatformCategory)
            .Include(f => f.ShopCategory)
            .Include(f => f.FoodOperatingSlots).ThenInclude(op => op.OperatingSlot)
            .Include(f => f.FoodOptionGroups).ThenInclude(fog => fog.OptionGroup).ThenInclude(og => og.Options)
            .First(f => f.Id == id);
    }

    public async Task<(int TotalCount, IEnumerable<Food> Foods)> GetTopFood(long dormitoryId, int pageIndex, int pageSize)
    {
        var query = DbSet.Where(food => food.Status == FoodStatus.Active // The food item must be active
                                        && !food.IsSoldOut // The food item must not be sold out
                                        && food.Shop.ShopDormitories
                                            .Select(shopDormitory => shopDormitory.DormitoryId)
                                            .Contains(dormitoryId) // The shop must be in the specified dormitory
                                        && !food.Shop.IsReceivingOrderPaused // The shop must be accepting orders
                                        && food.Shop.Status == ShopStatus.Active // The shop must be active
        ).AsQueryable();
        var totalCount = await query.CountAsync().ConfigureAwait(false);
        List<Food> foods;

        if (totalCount == 0)
        {
            foods = new List<Food>();
        }
        else
        {
            foods = await query.OrderByDescending(food => food.TotalOrder) // Orders the result by the number of total orders in descending order
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        return (totalCount, foods);
    }

    public async Task<List<(long CategoryId, string CategoryName, IEnumerable<Food> Foods)>> GetShopFood(long shopId)
    {
        var groupedFoods = await DbSet
            .Where(f => f.ShopId == shopId && f.Status == FoodStatus.Active && f.ShopCategoryId.HasValue)
            .GroupBy(f => new { f.ShopCategoryId, f.ShopCategory!.DisplayOrder, f.ShopCategory.Name }) // Group by ShopCategoryId and include DisplayOrder
            .OrderBy(g => g.Key.DisplayOrder) // Order by DisplayOrder of ShopCategory
            .Select(g => new { CategoryId = g.Key.ShopCategoryId!.Value, CategoryName = g.Key.Name, Foods = g.ToList() })
            .ToListAsync()
            .ConfigureAwait(false);

        return groupedFoods
            .Select(g => (g.CategoryId, g.CategoryName, g.Foods.AsEnumerable()))
            .ToList();
    }

    public async Task<bool> CheckExistedAndActiveByIdAndShopId(long id, long shopId)
    {
        return await DbSet.AnyAsync(f => f.Id == id && f.ShopId == shopId && f.Status == FoodStatus.Active).ConfigureAwait(false);
    }

    public async Task<bool> CheckForUpdateByIdAndShopId(long id, long shopId)
    {
        return await DbSet.AnyAsync(f => f.Id == id && f.ShopId == shopId && f.Status != FoodStatus.Delete).ConfigureAwait(false);
    }

    public async Task<(int TotalCount, IEnumerable<Food> Foods)> GetAllActiveFoodByShopId(long shopId, int pageIndex, int pageSize)
    {
        var query = DbSet.Where(f => f.ShopId == shopId && f.Status == FoodStatus.Active);
        var totalCount = await query.CountAsync().ConfigureAwait(false);
        var foods = await query.OrderByDescending(f => f.TotalOrder)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync()
            .ConfigureAwait(false);

        return (totalCount, foods);
    }

    public async Task<(List<long> IdsNotFound, IEnumerable<Food> Foods)> GetByIds(List<long> ids)
    {
        var foods = await DbSet
            .Where(f => ids.Contains(f.Id) && f.Status != FoodStatus.Delete)
            .ToListAsync()
            .ConfigureAwait(false);
        var foundIds = foods.Select(f => f.Id).ToList();
        var idsNotFound = ids.Except(foundIds).ToList();

        return (idsNotFound, foods);
    }

    public async Task<bool> CheckAllIdsInOneShop(List<long> ids)
    {
        // Retrieve the foods matching the provided ids
        var foods = await DbSet
            .Where(f => ids.Contains(f.Id))
            .Select(f => f.ShopId) // Select only the ShopId to optimize query
            .Distinct() // Ensure that only distinct ShopIds are retrieved
            .ToListAsync()
            .ConfigureAwait(false);

        // If the distinct list contains only one ShopId, return true (all ids belong to the same shop)
        return foods.Count == 1;
    }
}