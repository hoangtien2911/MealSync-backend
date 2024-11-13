﻿using MealSync.Application.Common.Abstractions.Messaging;
using MealSync.Application.Shared;
using MealSync.Domain.Entities;

namespace MealSync.Application.UseCases.Orders.Commands.ShopOrderProcess.ShopDeliveryFailOrder;

public class ShopDeliveryFailOrderCommand : ICommand<Result>
{
    public long OrderId { get; set; }

    public string? Reason { get; set; }

    public int ReasonIndentity { get; set; }

    public List<ShopDeliveyFailEvidence> Evidences { get; set; }
}

