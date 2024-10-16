﻿using MealSync.Domain.Entities;

namespace MealSync.Application.Common.Repositories;

public interface IPromotionRepository : IBaseRepository<Promotion>
{
    Task<IEnumerable<Promotion>> GetShopAvailablePromotionsByShopId(long id);

    Task<Promotion?> GetByIdAndShopId(long id, long shopId);

    Task<(IEnumerable<Promotion> EligibleList, IEnumerable<Promotion> IneligibleList)> GetShopAvailablePromotionsByShopIdAndTotalPrice(long shopId, double totalPrice);
}