using MealSync.API.Identites;
using MealSync.API.Shared;
using MealSync.Application.UseCases.Reports.Commands.CustomerReport;
using MealSync.Application.UseCases.Reports.Commands.ShopReplyCustomerReport;
using MealSync.Application.UseCases.Reports.Commands.UpdateReportStatusForMod;
using MealSync.Application.UseCases.Reports.Queries.GetAllReportForMod;
using MealSync.Application.UseCases.Reports.Queries.GetByOrderId;
using MealSync.Application.UseCases.Reports.Queries.GetByReportIdOfCustomer;
using MealSync.Application.UseCases.Reports.Queries.GetCustomerReport;
using MealSync.Application.UseCases.Reports.Queries.GetReportDetailForMod;
using MealSync.Application.UseCases.Reports.Queries.GetReportForShopByFilter;
using MealSync.Application.UseCases.Reports.Queries.GetReportForShopWebByFilter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MealSync.API.Controllers;

[Route(Endpoints.BASE)]
public class ReportController : BaseApiController
{
    [HttpPost(Endpoints.CUSTOMER_REPORT_ORDER)]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName}")]
    public async Task<IActionResult> CustomerReportOrder([FromForm] CustomerReportCommand command)
    {
        return HandleResult(await Mediator.Send(command).ConfigureAwait(false));
    }

    [HttpGet(Endpoints.GET_CUSTOMER_REPORT_ORDER)]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName}")]
    public async Task<IActionResult> GetReportOrderOfCustomer(long id)
    {
        return HandleResult(await Mediator.Send(new GetCustomerReportQuery { OrderId = id }).ConfigureAwait(false));
    }

    [HttpGet(Endpoints.GET_REPORT_ORDER_OF_SHOP)]
    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetReportOrderOfShop([FromQuery] GetReportForShopByFilterQuery query)
    {
        return HandleResult(await Mediator.Send(query).ConfigureAwait(false));
    }

    [HttpPost(Endpoints.SHOP_REPLY_REPORT_ORDER)]
    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> ShopReplyReportOrder([FromBody] ShopReplyCustomerReportCommand command)
    {
        return HandleResult(await Mediator.Send(command).ConfigureAwait(false));
    }

    [HttpGet(Endpoints.GET_CUSTOMER_REPORT_ORDER_FOR_SHOP)]
    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetReportOrderOfCustomerForShop(long id)
    {
        return HandleResult(await Mediator.Send(new GetByReportIdOfCustomerQuery() { CustomerReportId = id }).ConfigureAwait(false));
    }

    [HttpGet(Endpoints.GET_CUSTOMER_REPORT_BY_ORDER)]
    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetReportByOrderId(long id)
    {
        return HandleResult(await Mediator.Send(new GetByOrderIdQuery { OrderId = id }).ConfigureAwait(false));
    }

    [HttpGet(Endpoints.GET_REPORT_OF_CUSTOMER_FOR_SHOP_WEB)]
    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetReportOfCustomerForShopWeb([FromQuery] GetReportForShopWebByFilterQuery query)
    {
        return HandleResult(await Mediator.Send(query).ConfigureAwait(false));
    }

    [HttpGet(Endpoints.MANAGE_REPORT)]
    [Authorize(Roles = $"{IdentityConst.ModeratorClaimName}")]
    public async Task<IActionResult> ReportForModManage([FromQuery] GetAllReportForModQuery request)
    {
        return HandleResult(await Mediator.Send(request).ConfigureAwait(false));
    }

    [HttpGet(Endpoints.MANAGE_REPORT_DETAIL)]
    [Authorize(Roles = $"{IdentityConst.ModeratorClaimName}")]
    public async Task<IActionResult> ReportDetailForModManage(long id)
    {
        return HandleResult(await Mediator.Send(new GetReportDetailForModQuery { ReportId = id }).ConfigureAwait(false));
    }

    [HttpPut(Endpoints.MANAGE_REPORT_UPDATE_STATUS)]
    [Authorize(Roles = $"{IdentityConst.ModeratorClaimName}")]
    public async Task<IActionResult> UpdateStatusReportForModManage([FromBody] UpdateReportStatusForModCommand command)
    {
        return HandleResult(await Mediator.Send(command).ConfigureAwait(false));
    }
}