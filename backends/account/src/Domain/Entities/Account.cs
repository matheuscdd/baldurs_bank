using Domain.Exceptions;

namespace Domain.Entities;

public class Account
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public string UserId { get; set; }
    public bool IsActive { get; set; }

    public Account(string userId, bool isActive)
    {
        ValidateUserId(userId);
        SetUserId(userId);
        SetIsActive(isActive);
        SetNumber();
    }
    
    public void SetNumber()
    {
        Number = new Random().Next(1000, 1_000_000);
    }

    public void SetIsActive(bool isActive)
    {
        IsActive = isActive;
    }
    
    public void SetUserId(string? userId)
    {
        ValidateUserId(userId);
        UserId = userId!;
    }
    
    private static void ValidateUserId(string? userId)
    {
        const string name = nameof(UserId);
        ValidateEmpty(userId, name);
    }
    
    private static void ValidateEmpty(string? value, string name)
    {
        if (string.IsNullOrEmpty(value)) 
        {
            throw new ValidationCustomException($"{name} cannot be empty");
        }
    }
    
    
}