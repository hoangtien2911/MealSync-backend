﻿using MealSync.Domain.Enums;

namespace MealSync.Application.Common.Constants;

public static class OrderConstant
{
    public static readonly List<OrderStatus> LIST_ORDER_STATUS_IN_PROCESSING = new List<OrderStatus>()
    {
        OrderStatus.Pending,
        OrderStatus.Confirmed,
        OrderStatus.Preparing,
        OrderStatus.Delivering,
    };

    public static readonly List<OrderStatus> LIST_ORDER_STATUS_IN_SHOP_ASSIGN_PROCESS = new List<OrderStatus>()
    {
        OrderStatus.Preparing,
        OrderStatus.Delivering,
        OrderStatus.Delivered,
        OrderStatus.FailDelivery,
    };

    public static readonly List<OrderStatus> LIST_ORDER_STATUS_FIX_ORDER_ASSIGN_PROCESS = new List<OrderStatus>()
    {
        OrderStatus.Delivering,
        OrderStatus.Delivered,
        OrderStatus.FailDelivery,
        OrderStatus.Completed,
        OrderStatus.IssueReported,
        OrderStatus.UnderReview,
        OrderStatus.Resolved,
    };

    public static readonly int TIME_SHOP_CANCEL_ORDER_CONFIRMED_IN_MINUTES = 60;

    public static readonly int TIME_SHOP_CANCEL_ORDER_CONFIRMED_IN_HOURS = 1;

    public static readonly int TIME_WARNING_SHOP_PREPARE_ORDER_EARLY_IN_MINUTES = 240;

    public static readonly int TIME_WARNING_SHOP_PREPARE_ORDER_EARLY_IN_HOURS = 4;

    public static readonly int TIME_WARNING_SHOP_ASSIGN_ORDER_EARLY_IN_HOURS = 1;

    public static readonly int HOUR_ACCEPT_SHOP_FILL_REASON = 2;
}