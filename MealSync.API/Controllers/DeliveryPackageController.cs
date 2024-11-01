﻿using MealSync.API.Identites;
using MealSync.API.Shared;
using MealSync.Application.UseCases.DeliveryPackages.Queries.GetDeliveryPackageGroupDetailByTimeFrames;
using MealSync.Application.UseCases.DeliveryPackages.Queries.GetListTimeFrameUnAssigns;
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
}