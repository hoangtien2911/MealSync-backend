using MealSync.Application.Common.Abstractions.Messaging;
using MealSync.Application.Common.Enums;
using MealSync.Application.Common.Repositories;
using MealSync.Application.Shared;
using MealSync.Application.UseCases.Foods.Models;
using MealSync.Domain.Enums;
using MealSync.Domain.Exceptions.Base;

namespace MealSync.Application.UseCases.Foods.Queries.GetByIds;

public class GetByIdsForCartHandler : IQueryHandler<GetByIdsForCartQuery, Result>
{
    private readonly IFoodRepository _foodRepository;
    private readonly IOptionGroupRepository _optionGroupRepository;
    private readonly IOperatingSlotRepository _operatingSlotRepository;
    private readonly IShopRepository _shopRepository;

    public GetByIdsForCartHandler(
        IFoodRepository foodRepository, IOptionGroupRepository optionGroupRepository,
        IOperatingSlotRepository operatingSlotRepository, IShopRepository shopRepository)
    {
        _foodRepository = foodRepository;
        _optionGroupRepository = optionGroupRepository;
        _operatingSlotRepository = operatingSlotRepository;
        _shopRepository = shopRepository;
    }

    public async Task<Result<Result>> Handle(GetByIdsForCartQuery request, CancellationToken cancellationToken)
    {
        var idsNotFound = new List<string>();
        var foodCartCheckResponse = new FoodCartCheckResponse();
        var foodsResponses = new List<FoodCartCheckResponse.DetailFoodResponse>();
        var shop = _shopRepository.GetById(request.ShopId)!;
        var shopOperatingSlot = await _operatingSlotRepository.GetAvailableForTimeRangeOrder(
                request.ShopId, request.OrderTime.StartTime, request.OrderTime.EndTime)
            .ConfigureAwait(false);

        if (shop.Status == ShopStatus.Banning || shop.Status == ShopStatus.Banned || shop.Status == ShopStatus.Deleted || shop.Status == ShopStatus.UnApprove)
        {
            foodCartCheckResponse.IsRemoveAllCart = true;
            foodCartCheckResponse.IsReceivingOrderPaused = false;
            foodCartCheckResponse.MessageForAllCart = "Cửa hàng đã không còn hoạt động.";
            foodCartCheckResponse.IsPresentFoodNeedRemoveToday = false;
            foodCartCheckResponse.MessageFoodRemoveToday = default;
            foodCartCheckResponse.IdsNotFoundToday = default;
            foodCartCheckResponse.IsPresentFoodNeedRemoveTomorrow = false;
            foodCartCheckResponse.MessageFoodRemoveTomorrow = default;
            foodCartCheckResponse.IdsNotFoundTomorrow = default;
            foodCartCheckResponse.Foods = foodsResponses;
        }
        else if (shop.Status == ShopStatus.InActive)
        {
            foodCartCheckResponse.IsRemoveAllCart = false;
            foodCartCheckResponse.IsReceivingOrderPaused = true;
            foodCartCheckResponse.MessageForAllCart = "Cửa hàng đã đóng cửa, bạn không thể đặt đồ ăn vào bây giờ.";
        }
        else if (shop.IsReceivingOrderPaused)
        {
            foodCartCheckResponse.IsRemoveAllCart = false;
            foodCartCheckResponse.IsReceivingOrderPaused = true;
            foodCartCheckResponse.MessageForAllCart = "Cửa hàng đang tạm ngưng nhận hàng, bạn không thể đặt đồ ăn vào bây giờ.";
        }
        else if (shop.IsAcceptingOrderNextDay)
        {

        }
        if (shopOperatingSlot == default)
        {
            return Result.Success(new FoodCartCheckResponse
            {
                Foods = foodsResponses,
                IdsNotFoundToday = request.Foods.Select(f => f.Id).ToList(),
                IdsNotFoundTomorrow = request.Foods.Select(f => f.Id).ToList(),
            });
        }
        else if (shopOperatingSlot.IsReceivingOrderPaused)
        {
            foodCartCheckResponse.IdsNotFoundToday = request.Foods.Select(f => f.Id).ToList();
        }

        foreach (var foodRequest in request.Foods)
        {
            var validId = long.TryParse(foodRequest.Id.Split('-')[0], out var id);

            if (!validId)
            {
                idsNotFound.Add(foodRequest.Id);
            }
            else
            {
                var food = await _foodRepository.GetActiveFood(id, shopOperatingSlot.Id).ConfigureAwait(false);

                if (food == default)
                {
                    idsNotFound.Add(foodRequest.Id);
                }
                else
                {
                    var foodResponse = new FoodCartCheckResponse.DetailFoodResponse
                    {
                        Id = food.Id,
                        Name = food.Name,
                        Description = food.Description,
                        Price = food.Price,
                        ImageUrl = food.ImageUrl,
                        ShopId = food.ShopId,
                    };
                    bool isNotFound = false;

                    if (foodRequest.OptionGroupRadio != default && foodRequest.OptionGroupRadio.Count > 0 && !isNotFound)
                    {
                        var optionGroupRadioResponses = new List<FoodCartCheckResponse.OptionGroupRadioResponse>();

                        foreach (var optionGroupRequest in foodRequest.OptionGroupRadio)
                        {
                            var optionGroup = await _optionGroupRepository.GetByIdAndOptionIds(
                                optionGroupRequest.Id, new[] { optionGroupRequest.OptionId }
                            ).ConfigureAwait(false);

                            if (optionGroup == default || !optionGroup.Options.Select(o => o.Id).Contains(optionGroupRequest.OptionId))
                            {
                                idsNotFound.Add(foodRequest.Id);
                                isNotFound = true;
                                break;
                            }
                            else
                            {
                                var option = optionGroup.Options.First();
                                optionGroupRadioResponses.Add(new FoodCartCheckResponse.OptionGroupRadioResponse
                                {
                                    Id = optionGroup.Id,
                                    Title = optionGroup.Title,
                                    Option = new FoodCartCheckResponse.OptionResponse
                                    {
                                        Id = option.Id,
                                        Title = option.Title,
                                        ImageUrl = option.ImageUrl,
                                        Price = option.Price,
                                        IsCalculatePrice = option.IsCalculatePrice,
                                    },
                                });
                            }
                        }

                        foodResponse.OptionGroupRadio = optionGroupRadioResponses;
                    }

                    if (foodRequest.OptionGroupCheckbox != default && foodRequest.OptionGroupCheckbox.Count > 0 && !isNotFound)
                    {
                        var optionGroupCheckboxResponses = new List<FoodCartCheckResponse.OptionGroupCheckboxResponse>();

                        foreach (var optionGroupRequest in foodRequest.OptionGroupCheckbox)
                        {
                            var optionGroup = await _optionGroupRepository.GetByIdAndOptionIds(
                                optionGroupRequest.Id, optionGroupRequest.OptionIds
                            ).ConfigureAwait(false);
                            var availableOptionIds = optionGroup?.Options.Select(o => o.Id).ToList() ?? new List<long>();

                            if (optionGroup == default || !optionGroupRequest.OptionIds.All(optionId => availableOptionIds.Contains(optionId)))
                            {
                                idsNotFound.Add(foodRequest.Id);
                                isNotFound = true;
                                break;
                            }
                            else
                            {
                                var matchingOptions = optionGroup.Options
                                    .Where(o => optionGroupRequest.OptionIds.Contains(o.Id))
                                    .Select(o => new FoodCartCheckResponse.OptionResponse
                                    {
                                        Id = o.Id,
                                        Title = o.Title,
                                        ImageUrl = o.ImageUrl,
                                        Price = o.Price,
                                        IsCalculatePrice = o.IsCalculatePrice,
                                    })
                                    .ToList();

                                var optionGroupCheckboxResponse = new FoodCartCheckResponse.OptionGroupCheckboxResponse
                                {
                                    Id = optionGroup.Id,
                                    Title = optionGroup.Title,
                                    Options = matchingOptions,
                                };

                                optionGroupCheckboxResponses.Add(optionGroupCheckboxResponse);
                            }
                        }

                        foodResponse.OptionGroupCheckbox = optionGroupCheckboxResponses;
                    }

                    if (!isNotFound)
                    {
                        foodsResponses.Add(foodResponse);
                    }
                }
            }
        }

        return Result.Success(new FoodCartCheckResponse
        {
            Foods = foodsResponses,
            IdsNotFoundToday = idsNotFound,
        });
    }
}