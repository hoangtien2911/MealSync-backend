﻿using System.ComponentModel;

namespace MealSync.Application.Common.Enums;

public enum MessageCode
{
    // Account
    [Description("E-Account-InvalidUserNamePassword")]
    E_ACCOUNT_INVALID_USERNAME_PASSWORD,

    [Description("E-Account-Unverified")]
    E_ACCOUNT_UNVERIFIED,

    [Description("E-Account-Banned")]
    E_ACCOUNT_BANNED,

    [Description("E-Account-InvalidRole")]
    E_ACCOUNT_INVALID_ROLE,

    [Description("E-Account-PhoneNumberExist")]
    E_ACCOUNT_PHONE_NUMBER_EXIST,

    [Description("E-Account-EmailExist")]
    E_ACCOUNT_EMAIL_EXIST,

    [Description("E-Account-EmailExistInOtherRole")]
    E_ACCOUNT_EMAIL_EXIST_IN_ORTHER_ROLE,

    [Description("I-Account-RegisterSuccessfully")]
    I_ACCOUNT_REGISTER_SUCCESSFULLY,

    // Platform Category
    [Description("E-PlatformCategory-NotFound")]
    E_PLATFORM_CATEGORY_NOT_FOUND,

    // Shop Category
    [Description("E-ShopCategory-NotFound")]
    E_SHOP_CATEGORY_NOT_FOUND,

    [Description("E-ShopCategory-NotEnough")]
    E_SHOP_CATEGORY_NOT_ENOUGH,

    // Operating Slot
    [Description("E-OperatingSlot-NotFound")]
    E_OPERATING_SLOT_NOT_FOUND,

    [Description("E-OperatingSlot-CodeConfirmNotCorrect")]
    E_OPERATING_SLOT_CODE_CONFIRM_NOT_CORRECT,

    [Description("E-OperatingSlot-Overlap")]
    E_OPERATING_SLOT_OVERLAP,

    [Description("W-OperatingSlot-ChangeIncludeProduct")]
    W_OPERATING_SLOT_CHANGE_INCLUDE_PRODUCT,

    [Description("I-OperatingSlot-ChangeSuccess")]
    I_OPERATING_SLOT_CHANGE_SUCCESS,

    [Description("I-OperatingSlot-AddSuccess")]
    I_OPERATING_SLOT_ADD_SUCCESS,

    // Option Group
    [Description("E-OptionGroup-NotFound")]
    E_OPTION_GROUP_NOT_FOUND,

    [Description("E-OptionGroup-RadioValidate")]
    E_OPTION_GROUP_RADIO_VALIDATE,

    // Building
    [Description("E-Building-NotSelect")]
    E_BUILDING_NOT_SELECT,

    [Description("I-Building-Selected")]
    E_BUILDING_SELECTED,

    // Dormitory
    [Description("E-Dormitory-NotFound")]
    E_DORMITORY_NOT_FOUND,
}