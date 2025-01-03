﻿using AutoMapper;
using MealSync.Application.UseCases.Orders.Models;
using MealSync.Domain.Entities;
using MealSync.Domain.Enums;

namespace MealSync.Application.Mappings;

public class DeliveryInforResponseMapping : Profile
{
    public DeliveryInforResponseMapping()
    {
        CreateMap<Order, DeliveryInforResponse>()
            .ForMember(dest => dest.DeliveryStatus, opt => opt.MapFrom(
                src => GetDeliveryStatus(src)));
    }

    private int GetDeliveryStatus(Order order)
    {
        if (order.Status == OrderStatus.Delivered)
            return 1; // success
        if (order.Status == OrderStatus.FailDelivery)
            return 2; // fail
        return 0;
    }
}