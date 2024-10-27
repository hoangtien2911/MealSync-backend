﻿using MealSync.Domain.Entities;

namespace MealSync.Application.Common.Services.Notifications;

public interface INotificationFactory
{
    Notification CreateOrderPendingNotification(Order order, Shop shop);

    Notification CreateOrderConfirmedNotification(Order order, Shop shop);

    Notification CreateOrderRejectedNotification(Order order, Shop shop);

    Notification CreateOrderCancelNotification(Order order, Shop shop);

    Notification CreateOrderConfirmNotification(Order order, Shop shop);

    Notification CreateRefundFaillNotification(Order order, Account accMod);

    Notification CreateOrderPreparingNotification(Order order, Shop shop);

    Notification CreateOrderCustomerDeliveredNotification(Order order, Shop shop);

    Notification CreateOrderShopDeliveredNotification(Order order, Account accShip);

    Notification CreateOrderCustomerDeliveringNotification(Order order, Shop shop);

    Notification CreateOrderAssignedToStaffdNotification(Order order, Account accShip, Shop shop);
}