﻿using MealSync.Domain.Entities;

namespace MealSync.Application.Common.Repositories;

public interface IDeliveryPackageRepository : IBaseRepository<DeliveryPackage>
{
    DeliveryPackage GetPackageByShipIdAndTimeFrame(bool isShopOwnerShip, long shipperId, int startTime, int endTime);
}