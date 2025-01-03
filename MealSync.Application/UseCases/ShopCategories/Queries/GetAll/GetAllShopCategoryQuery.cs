﻿using MealSync.Application.Common.Abstractions.Messaging;
using MealSync.Application.Common.Models.Requests;
using MealSync.Application.Shared;

namespace MealSync.Application.UseCases.ShopCategories.Queries.GetAll;

public class GetAllShopCategoryQuery : PaginationRequest, IQuery<Result>
{
}