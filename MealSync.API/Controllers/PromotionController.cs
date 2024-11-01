using MealSync.API.Identites;
using MealSync.API.Shared;
using MealSync.Application.UseCases.Promotions.Commands.Create;
using MealSync.Application.UseCases.Promotions.Commands.UpdateInfo;
using MealSync.Application.UseCases.Promotions.Commands.UpdateStatus;
using MealSync.Application.UseCases.Promotions.Queries.GetEligibleAndIneligiblePromotions;
using MealSync.Application.UseCases.Promotions.Queries.GetShopPromotion;
using MealSync.Application.UseCases.Promotions.Queries.Shop.GetPromotionByFilter;
using MealSync.Application.UseCases.Promotions.Queries.Shop.GetPromotionDetail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MealSync.API.Controllers;

[Route(Endpoints.BASE)]
public class PromotionController : BaseApiController
{
    [HttpGet(Endpoints.GET_SHOP_PROMOTION)]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName}")]
    public async Task<IActionResult> GetShopPromotion(long id)
    {
        return HandleResult(await Mediator.Send(new GetShopPromotionQuery()
        {
            ShopId = id,
        }).ConfigureAwait(false));
    }

    [HttpGet(Endpoints.GET_SHOP_PROMOTION_BY_CONDITION)]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName}")]
    public async Task<IActionResult> GetShopPromotionByCondition(long id, double totalPrice)
    {
        return HandleResult(await Mediator.Send(new GetEligibleAndIneligiblePromotionQuery
        {
            ShopId = id,
            TotalPrice = totalPrice,
        }).ConfigureAwait(false));
    }

    [HttpPost(Endpoints.CREATE_PROMOTION)]
    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> CreateShopPromotion(CreatePromotionCommand request)
    {
        return HandleResult(await Mediator.Send(request).ConfigureAwait(false));
    }

    [HttpPut(Endpoints.UPDATE_PROMOTION_INFO)]
    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> UpdatePromotionInfo(UpdatePromotionInfoCommand request)
    {
        return HandleResult(await Mediator.Send(request).ConfigureAwait(false));
    }

    [HttpPut(Endpoints.UPDATE_PROMOTION_STATUS)]
    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> UpdatePromotionStatus(UpdatePromotionStatusCommand request)
    {
        return HandleResult(await Mediator.Send(request).ConfigureAwait(false));
    }

    [HttpGet(Endpoints.GET_SHOP_OWNER_PROMOTION)]
    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetShopOwnerPromotion([FromQuery] GetPromotionByFilterQuery request)
    {
        return HandleResult(await Mediator.Send(request).ConfigureAwait(false));
    }

    [HttpGet(Endpoints.GET_SHOP_OWNER_PROMOTION_DETAIL)]
    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetShopOwnerPromotion(long id)
    {
        return HandleResult(await Mediator.Send(new GetPromotionDetailQuery { Id = id }).ConfigureAwait(false));
    }
}