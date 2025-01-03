﻿using MealSync.Application.Common.Abstractions.Messaging;
using MealSync.Application.Common.Enums;
using MealSync.Application.Common.Services.Dapper;
using MealSync.Application.Shared;
using MealSync.Application.UseCases.Dashboards.Models;
using MealSync.Domain.Enums;

namespace MealSync.Application.UseCases.Dashboards.Queries.GetRevenueAdminChart;

public class GetRevenueAdminChartHandler : IQueryHandler<GetRevenueAdminChartQuery, Result>
{
    private readonly IDapperService _dapperService;

    public GetRevenueAdminChartHandler(IDapperService dapperService)
    {
        _dapperService = dapperService;
    }

    public async Task<Result<Result>> Handle(GetRevenueAdminChartQuery request, CancellationToken cancellationToken)
    {
        var revenueChart = await this._dapperService.SingleOrDefaultAsync<RevenueChartResponse>(
            QueryName.GetRevenueAdminChart,
            new
            {
                DateOfYear = request.DateOfYear,
                PaymentOnlineList = new PaymentMethods[] { PaymentMethods.VnPay },
                DeliveryFailReportedByCustomer = OrderIdentityCode.ORDER_IDENTITY_DELIVERY_FAIL_REPORTED_BY_CUSTOMER,
                DeliveredReportedByCustomer = OrderIdentityCode.ORDER_IDENTITY_DELIVERED_REPORTED_BY_CUSTOMER,
                DeliveryFailByCustomer = OrderIdentityCode.ORDER_IDENTITY_DELIVERY_FAIL_BY_CUSTOMER,
                CustomerCancel = OrderIdentityCode.ORDER_IDENTITY_CUSTOMER_CANCEL,
                ShopCancel = OrderIdentityCode.ORDER_IDENTITY_SHOP_CANCEL,
                DeliveryFailByShop = OrderIdentityCode.ORDER_IDENTITY_DELIVERY_FAIL_BY_SHOP,
            }).ConfigureAwait(false);
        return Result.Success(revenueChart);
    }
}