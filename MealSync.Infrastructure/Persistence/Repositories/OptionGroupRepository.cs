using MealSync.Application.Common.Repositories;
using MealSync.Domain.Entities;
using MealSync.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MealSync.Infrastructure.Persistence.Repositories;

public class OptionGroupRepository : BaseRepository<OptionGroup>, IOptionGroupRepository
{
    public OptionGroupRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public bool CheckExistedByIdAndShopId(long id, long shopId)
    {
        return DbSet.Any(og => og.Id == id && og.ShopId == shopId && og.Status != OptionGroupStatus.Delete);
    }

    public OptionGroup GetByIdIncludeOption(long id)
    {
        return DbSet.Include(og => og.Options.Where(o => o.Status != OptionStatus.Delete))
            .First(og => og.Id == id && og.Status != OptionGroupStatus.Delete);
    }

    public (int TotalCount, List<OptionGroup> OptionGroups) GetAllShopOptonGroup(long? currentPrincipalId, int requestPageIndex, int requestPageSize)
    {
        var query = DbSet
            .Include(op => op.FoodOptionGroups)
            .Include(op => op.Options)
            .Where(op => op.ShopId == currentPrincipalId.Value && op.Status != OptionGroupStatus.Delete);

        var totalCount = query.Count();
        var optionGroups = query
            .OrderByDescending(op => op.CreatedDate)
            .Skip((requestPageIndex - 1) * requestPageSize)
            .Take(requestPageSize)
            .ToList();

        return (totalCount, optionGroups);
    }

    public bool CheckExistTitleOptionGroup(string title, long? id = null)
    {
        return id == null
            ? DbSet.Any(og => og.Title == title)
            : DbSet.Any(og => og.Title == title && og.Id != id);
    }
}