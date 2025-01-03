﻿using FluentValidation;
using MealSync.Application.Common.Constants;
using MealSync.Application.Common.Utils;

namespace MealSync.Application.UseCases.DeliveryPackages.Queries.GetListDeliveryPackageForShopWebs;

public class GetListDeliveryPackageForShopWebValidator : AbstractValidator<GetListDeliveryPackageForShopWebQuery>
{
    public GetListDeliveryPackageForShopWebValidator()
    {
        RuleFor(x => x.StartTime)
            .Must(TimeUtils.IsValidOperatingSlot)
            .WithMessage("Vui lòng cung cấp thời gian bắt đầu đúng định dạng hhMM");

        RuleFor(x => x.EndTime)
            .Must(TimeUtils.IsValidOperatingSlot)
            .WithMessage("Vui lòng cung cấp thời gian kết thúc đúng định dạng hhMM");

        RuleFor(x => x.IntendedReceiveDate)
            .Must(x => x != default)
            .WithMessage("Vui lòng cung cấp ngày giao hàng");
    }
}