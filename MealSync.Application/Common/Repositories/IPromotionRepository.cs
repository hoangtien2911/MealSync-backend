﻿using MealSync.Domain.Entities;

namespace MealSync.Application.Common.Repositories;

public interface IPromotionRepository : IBaseRepository<Promotion>
{
    Task<IEnumerable<Promotion>> GetShopAvailablePromotionsByShopId(long id);
}