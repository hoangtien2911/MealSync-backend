using MealSync.Application.Common.Abstractions.Messaging;
using MealSync.Application.Shared;
using MealSync.Domain.Enums;

namespace MealSync.Application.UseCases.ShopDeliveryStaffs.Commands.Create;

public class CreateDeliveryStaffCommand : ICommand<Result>
{
    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public ShopDeliveryStaffStatus ShopDeliveryStaffStatus { get; set; }
}