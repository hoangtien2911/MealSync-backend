﻿using MealSync.Application.Common.Abstractions.Messaging;
using MealSync.Application.Common.Constants;
using MealSync.Application.Common.Enums;
using MealSync.Application.Common.Services.Dapper;
using MealSync.Application.Shared;
using MealSync.Application.UseCases.Dashboards.Models;
using MealSync.Application.UseCases.Orders.Models;
using MealSync.Domain.Enums;

namespace MealSync.Application.UseCases.Dashboards.Queries.GetOrderChartForAdmin;

public class GetOrderChartForAdminHandler : IQueryHandler<GetOrderChartForAdminQuery, Result>
{
    private readonly IDapperService _dapperService;

    public GetOrderChartForAdminHandler(IDapperService dapperService)
    {
        _dapperService = dapperService;
    }

    public async Task<Result<Result>> Handle(GetOrderChartForAdminQuery request, CancellationToken cancellationToken)
    {
        var numDays = (request.DateTo - request.DateFrom).Days;
        numDays = numDays == 0 ? 1 : numDays;
        var maxPoints = Math.Min(ChartConstatnt.MAX_POINT_ORDER_CHART, numDays);
        var maxLabels = Math.Min(ChartConstatnt.MAX_LABEL_ORDER_CHART, numDays);
        var step = 1;
        if (maxPoints >= ChartConstatnt.MAX_POINT_ORDER_CHART)
        {
            step = numDays / (maxPoints - 2);
        }

        var result = new List<PointOfOrderChart>();
        for (var i = 0; i < maxPoints; i++)
        {
            var point = GetOrderChart(request.DateFrom.AddDays(step * i));
            if (i >= maxLabels)
            {
                point.LabelDate = null;
            }

            result.Add(point);
        }

        return Result.Success(result);
    }

    private PointOfOrderChart GetOrderChart(DateTime dateStep)
    {
        var point = _dapperService.SingleOrDefault<PointOfOrderChart>(QueryName.GetPointOfOrderAdminChart, new
        {
            DateFrom = dateStep.ToString("yyyy-MM-dd"),
            DateTo = dateStep.ToString("yyyy-MM-dd"),
            PaymentOnlineList = new PaymentMethods[] { PaymentMethods.VnPay },
            DeliveryFailReportedByCustomer = OrderIdentityCode.ORDER_IDENTITY_DELIVERY_FAIL_REPORTED_BY_CUSTOMER,
            DeliveredReportedByCustomer = OrderIdentityCode.ORDER_IDENTITY_DELIVERED_REPORTED_BY_CUSTOMER,
            DeliveryFailByCustomer = OrderIdentityCode.ORDER_IDENTITY_DELIVERY_FAIL_BY_CUSTOMER,
            CustomerCancel = OrderIdentityCode.ORDER_IDENTITY_CUSTOMER_CANCEL,
            ShopCancel = OrderIdentityCode.ORDER_IDENTITY_SHOP_CANCEL,
            DeliveryFailByShop = OrderIdentityCode.ORDER_IDENTITY_DELIVERY_FAIL_BY_SHOP,
        });

        return point;
    }
}