﻿using MealSync.API.Identites;
using MealSync.API.Shared;
using MealSync.Application.UseCases.DeliveryPackages.Commands.UpdateDeliveryPackageGroups;
using MealSync.Application.UseCases.DeliveryPackages.Queries.GetAllDeliveryPackageGroupByTimeFrames;
using MealSync.Application.UseCases.DeliveryPackages.Queries.GetAllDeliveryPackages;
using MealSync.Application.UseCases.DeliveryPackages.Queries.GetAllOwnDeliveryPackagesForWebs;
using MealSync.Application.UseCases.DeliveryPackages.Queries.GetDeliveryPackageDetailByTimeFrames;
using MealSync.Application.UseCases.DeliveryPackages.Queries.GetDeliveryPackageGroupDetailByTimeFrames;
using MealSync.Application.UseCases.DeliveryPackages.Queries.GetDeliveryPackageHistoryForShopWeb;
using MealSync.Application.UseCases.DeliveryPackages.Queries.GetListDeliveryPackageForShopWebs;
using MealSync.Application.UseCases.DeliveryPackages.Queries.GetListTimeFrameUnAssigns;
using MealSync.Application.UseCases.DeliveryPackages.Queries.SuggestAssignDeliveryPackages;
using MealSync.Application.UseCases.DeliveryPackages.Queries.SuggestAssignUpdateDeliveryPackages;
using MealSync.Application.UseCases.Orders.Commands.ShopOrderProcess.ShopCreateDeliveryPackage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MealSync.API.Controllers;

[Route(Endpoints.BASE)]
public class DeliveryPackageController : BaseApiController
{
    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    [HttpPost(Endpoints.CREATE_DELIVERY_PACKAGE)]
    public async Task<IActionResult> AddShopProfile([FromBody] ShopCreateDeliveryPackageCommand command)
    {
        return HandleResult(await Mediator.Send(command));
    }

    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    [HttpGet(Endpoints.GET_DELIVERY_PACKAGE_GROUP)]
    public async Task<IActionResult> AddShopProfile([FromQuery] GetDeliveryPackageGroupDetailByTimeFrameQuery query)
    {
        return HandleResult(await Mediator.Send(query));
    }

    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    [HttpGet(Endpoints.GET_TIME_FRAME_ALL_ORDER_UN_ASSIGN)]
    public async Task<IActionResult> GetTimeFrameOrderUnAssignProfile([FromQuery] GetListTimeFrameUnAssignQuery query)
    {
        return HandleResult(await Mediator.Send(query));
    }

    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    [HttpGet(Endpoints.SUGGEST_CREATE_ASSIGN_ORDER)]
    public async Task<IActionResult> GetSuggestAssignOrder([FromQuery] SuggestAssignDeliveryPackageQuery query)
    {
        return HandleResult(await Mediator.Send(query));
    }

    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    [HttpGet(Endpoints.GET_ALL_DELIVERY_PACKAGE_GROUP_BY_INTERVAL)]
    public async Task<IActionResult> GetSuggestAssignOrder([FromQuery] GetAllDeliveryPackageGroupByTimeFrameQuery query)
    {
        return HandleResult(await Mediator.Send(query));
    }

    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    [HttpPut(Endpoints.UPDATE_DELIVERY_PACKAGE_GROUP)]
    public async Task<IActionResult> GetSuggestAssignOrder([FromBody] UpdateDeliveryPackageGroupCommand command)
    {
        return HandleResult(await Mediator.Send(command));
    }

    [Authorize(Roles = $"{IdentityConst.ShopClaimName}, {IdentityConst.ShopDeliveryClaimName}")]
    [HttpGet(Endpoints.GET_ALL_DELIVERY_PACKAGE)]
    public async Task<IActionResult> GetOwnAllDeliveryPackageOrder([FromQuery] GetAllDeliveryPackageQuery query)
    {
        return HandleResult(await Mediator.Send(query));
    }

    [Authorize(Roles = $"{IdentityConst.ShopClaimName}, {IdentityConst.ShopDeliveryClaimName}")]
    [HttpGet(Endpoints.GET_DELIVERY_PACKAGE)]
    public async Task<IActionResult> GetOwnDeliveryPackageDetailOrder(long id)
    {
        return HandleResult(await Mediator.Send(new GetDeliveryPackageDetailQuery()
        {
            Id = id,
        }));
    }

    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    [HttpGet(Endpoints.GET_DELIVERY_PACKAGE_FOR_WEB)]
    public async Task<IActionResult> GetListDeliveryPackageForShopWebOrder([FromQuery] GetListDeliveryPackageForShopWebQuery query)
    {
        return HandleResult(await Mediator.Send(query).ConfigureAwait(false));
    }

    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    [HttpGet(Endpoints.GET_SHOP_OWN_DELIVERY_PACKAGE_FOR_WEB)]
    public async Task<IActionResult> GetListOwnDeliveryPackageForShopWebOrder([FromQuery] GetAllOwnDeliveryPackageForWebQuery query)
    {
        return HandleResult(await Mediator.Send(query).ConfigureAwait(false));
    }

    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    [HttpGet(Endpoints.SUGGEST_UPDATE_ASSIGN_ORDER)]
    public async Task<IActionResult> GetSuggestAssignUpdateOrder([FromQuery] SuggestAssignUpdateDeliveryPackageQuery query)
    {
        return HandleResult(await Mediator.Send(query));
    }

    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    [HttpGet(Endpoints.GET_DELIVERY_PACKAGE_HISTORY)]
    public async Task<IActionResult> GetDeliveryPackageHistory([FromQuery] GetDeliveryPackageHistoryForShopWebQuery query)
    {
        return HandleResult(await Mediator.Send(query));
    }
}