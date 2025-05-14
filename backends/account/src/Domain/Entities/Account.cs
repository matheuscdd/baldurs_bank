using Domain.Exceptions;

namespace Domain.Entities;

public class Account
{
    public Guid Id { get; private set; }
    public int Number { get; private set; }
    public string UserId { get; private set; }
    public bool IsActive { get; private set; }

    private Account() {}

    public Account(string id)
    {
        ValidateId(id);
        SetId(id);
    }
    
    public Account(string userId, bool isActive)
    {
        ValidateUserId(userId);
        SetUserId(userId);
        SetStatus(isActive);
        SetNumber();
    }

    public Account(string id, string userId)
    {
        ValidateId(id);
        ValidateUserId(userId);
        SetId(id);
        SetUserId(userId);
    }
    
    public void SetNumber()
    {
        Number = new Random().Next(1000, 1_000_000);
    }

    public void SetStatus(bool isActive)
    {
        IsActive = isActive;
    }
    
    public void SetUserId(string? userId)
    {
        ValidateUserId(userId);
        UserId = userId!;
    }
    
    public void SetId(string? id)
    {
        Id = ValidateId(id);;
    }
    
    private static void ValidateUserId(string? userId)
    {
        const string name = nameof(UserId);
        ValidateEmpty(userId, name);
    }
    
    private static Guid ValidateFormatGuid(string rawValue, string name)
    {
        if (!Guid.TryParse(rawValue, out Guid value))
        {
            throw new ValidationCustomException($"{name} is not a valid guid");
        }
        return value;
    }
    
    public static Guid ValidateId(string rawAccountId)
    {
        var name = nameof(Id);
        var accountId = ValidateFormatGuid(rawAccountId, name);
        ValidateEmpty(accountId, name);
        return accountId;
    }
    
    private static void ValidateEmpty(Guid value, string name)
    {
        if (value == Guid.Empty) 
        {
            throw new ValidationCustomException($"{name} cannot be empty");
        }
    }
    
    private static void ValidateEmpty(string? value, string name)
    {
        if (string.IsNullOrEmpty(value)) 
        {
            throw new ValidationCustomException($"{name} cannot be empty");
        }
    }
    
    
}