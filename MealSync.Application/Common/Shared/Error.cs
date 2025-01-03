﻿using MealSync.Application.Common.Repositories;

namespace MealSync.Application.Shared;

public class Error : IEquatable<Error>
{
    private static ISystemResourceRepository _resourceRepository;
    public static readonly Error None = new(string.Empty, string.Empty, false, false);
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.", true, false);

    public static void Configure(ISystemResourceRepository resourceRepository)
    {
        _resourceRepository = resourceRepository;
    }

    public Error(string code)
    {
        Code = code;
        Message = _resourceRepository.GetByResourceCode(code) ?? "Message not found.";
        IsClientError = true;
        IsSystemError = false;
    }

    public Error(string code, params object[] args)
    {
        Code = code;
        Message = _resourceRepository.GetByResourceCode(code, args) ?? "Message not found.";
        IsClientError = true;
        IsSystemError = false;
    }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
        IsClientError = true;
        IsSystemError = false;
    }

    public Error(string code, string message, bool isSystemError)
    {
        Code = code;
        Message = message;
        IsClientError = false;
        IsSystemError = isSystemError;
    }

    public Error(string code, string message, bool isClientError, bool isSystemError)
    {
        Code = code;
        Message = message;
        IsClientError = isClientError;
        IsSystemError = isSystemError;
    }

    public bool IsClientError { get; set; } = true;

    public bool IsSystemError { get; set; }

    public string Code { get; }

    public string Message { get; }

    public static implicit operator string(Error error) => error.Code;

    public static bool operator ==(Error? a, Error? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Error? a, Error? b) => !(a == b);

    public virtual bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }

        return Code == other.Code && Message == other.Message;
    }

    public override bool Equals(object? obj) => obj is Error error && Equals(error);

    public override int GetHashCode() => HashCode.Combine(Code, Message);

    public override string ToString() => Code;
}