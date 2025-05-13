using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public decimal Value { get; private set; }
    public Method Method { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private Transaction() {}

    public Transaction(string accountId, string value, Method method)
    {
        SetMethod(method);
        ValidateAccountId(accountId);
        ValidateValue(value);
        SetValue(value);
        SetAccountId(accountId);
    }

    public Transaction(string accountId)
    {
        ValidateAccountId(accountId);
        SetAccountId(accountId);
    }

    private void SetAccountId(string rawAccountId)
    {
        var accountId = ValidateAccountId(rawAccountId);
        AccountId = accountId;
    }

    private void SetValue(string rawValue)
    {
        var value = ValidateValue(rawValue);
        Value = value;
    }

    private void SetMethod(Method method)
    {
        Method = method;
    }

    public static DateTime ValidateDateTime(string rawValue)
    {
        if (!DateTime.TryParse(rawValue, out DateTime value))
        {
            throw new ValidationCustomException("DateTime is invalid");
        }
        return value;
    }

    private static void ValidateCases(decimal value, int cases, string name)
    {
        if (decimal.Round(value, cases) != value)
        {
            throw new ValidationCustomException($"{name} need to be exact {cases} decimal cases");
        }
    }
    
    private static void ValidateNotZero(decimal value, string name)
    {
        if (value == 0m)
        {
            throw new ValidationCustomException($"{name} cannot be zero");
        }
    }
    
    private static void ValidateEmpty(Guid value, string name)
    {
        if (value == Guid.Empty) 
        {
            throw new ValidationCustomException($"{name} cannot be empty");
        }
    }
    
    public static Guid ValidateId(string rawAccountId)
    {
        var name = nameof(Id);
        var accountId = ValidateFormatGuid(rawAccountId, name);
        ValidateEmpty(accountId, name);
        return accountId;
    }
    
    public static Guid ValidateAccountId(string rawAccountId)
    {
        var name = nameof(AccountId);
        var accountId = ValidateFormatGuid(rawAccountId, name);
        ValidateEmpty(accountId, name);
        return accountId;
    }

    private static void ValidatePositive(decimal value, string name)
    {
        if (value < 0)
        {
            throw new ValidationCustomException($"{name} cannot be negative");
        }
    }
    
    private static void ValidateNegative(decimal value, string name)
    {
        if (value > 0)
        {
            throw new ValidationCustomException($"{name} cannot be positive");
        }
    }

    public static decimal ValidateFormatDecimal(string rawValue, string name)
    {
        if (!decimal.TryParse(rawValue, out decimal value))
        {
            throw new ValidationCustomException($"{name} is not a valid number");
        }
        return value;
    }
    
    private static Guid ValidateFormatGuid(string rawValue, string name)
    {
        if (!Guid.TryParse(rawValue, out Guid value))
        {
            throw new ValidationCustomException($"{name} is not a valid guid");
        }
        return value;
    }

    private decimal ValidateValue(string rawValue)
    {
        var name = nameof(Value);
        var value = ValidateFormatDecimal(rawValue, name);
        ValidateNotZero(value, name);
        ValidateCases(value, 2, name);
        switch (Method)
        {
            case Method.Credit:
                ValidatePositive(value, name);
                break;
            case Method.Debit:
                ValidateNegative(value, name);
                break;
        }
        return value;
    }
    
}