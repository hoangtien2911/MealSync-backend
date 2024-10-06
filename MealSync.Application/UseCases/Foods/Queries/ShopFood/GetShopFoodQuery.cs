using MealSync.Application.Common.Abstractions.Messaging;
using MealSync.Application.Shared;

namespace MealSync.Application.UseCases.Foods.Queries.ShopFood;

public class GetShopFoodQuery : IQuery<Result>
{
    public long ShopId { get; set; }
}