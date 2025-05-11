using System.ComponentModel.DataAnnotations;
using Domain.Exceptions;

namespace Domain.Entities;

public class User 
{
    public string Id { get; protected set; }
    public string Name { get; protected set; }
    public string Email { get; protected set; }
    public bool IsActive { get; protected set; } = true;
    public bool IsManager { get; protected set; }

    public User(string id, string name, string email, bool isManager)
    {
        ValidateName(name);
        ValidateEmail(email);
        ValidateId(id);
        SetEmail(email);
        SetName(name);
        SetIsManager(isManager);
        SetId(id);
        Id = id;
    }
    
    public User(string? name, string? email, string? password)
    {
        ValidateEmail(email);
        ValidateName(name);
        ValidatePassword(password);
        SetName(name!);
        SetEmail(email!);
    }

    public void SetIsManager(bool isManager)
    {
        IsManager = isManager;
    }
    
    public void SetId(string? id)
    {
        ValidateId(id);
        Id = id!;
    }
    
    public void SetName(string? name)
    {
        ValidateName(name);
        Name = name!;
    }
    
    public void SetEmail(string email)
    {
        ValidateEmail(email);
        Email = email.ToLower();
    }
    
    private static void ValidateEmpty(string? value, string name)
    {
        if (string.IsNullOrEmpty(value)) 
        {
            throw new ValidationCustomException($"{name} cannot be empty");
        }
    }
    
    private static void ValidateLength(string value, string name, int min, int max)
    {
        if (value.Length > max) {
            throw new ValidationCustomException($"{name} cannot be greater than {max} characters");
        } else if (value.Length < min) {
            throw new ValidationCustomException($"{name} cannot be smaller than {min} characters");
        }
    }
    
    private static void ValidateEmailFormat(string email, string name)
    {
        if (new EmailAddressAttribute().IsValid(email)) return;
        throw new ValidationCustomException($"{name} is invalid");
    }
    
    private static void ValidatePasswordFormat(string password, string name)
    {
        if (!password.Any(char.IsNumber))
        {
            throw new ValidationCustomException($"{name} must have at least one digit");
        }
        else if (!password.Any(char.IsLetter))
        {
            throw new ValidationCustomException($"{name} must have at least one letter");
        }
        else if (!password.Any(char.IsUpper))
        {
            throw new ValidationCustomException($"{name} must have at least one uppercase letter");
        }
        else if (!password.Any(char.IsLower))
        {
            throw new ValidationCustomException($"{name} must have at least one lowercase letter");
        }
        else if (!password.Any(c => !char.IsLetterOrDigit(c)))
        {
            throw new ValidationCustomException($"{name} must have at least one non alphanumeric character");
        }
    }
    
    private static void ValidatePassword(string? password)
    {
        const string name = "Password";
        ValidateEmpty(password, name);
        ValidateLength(password!, name, 12, 150);
        ValidatePasswordFormat(password!, name);
    }
    
    private static void ValidateEmail(string? email)
    {
        const string name = nameof(Email);
        ValidateEmpty(email, name);
        ValidateLength(email!, name, 5, 100);
        ValidateEmailFormat(email!, name);
    }

    private static void ValidateName(string? name)
    {
        ValidateEmpty(name, nameof(Name));
        ValidateLength(name!, nameof(Name), 6, 120);
    }
    
    private static void ValidateId(string? id)
    {
        const string name = nameof(Id);
        ValidateEmpty(id, name);
    }
}