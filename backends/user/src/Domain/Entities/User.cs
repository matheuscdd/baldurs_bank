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
        Id = id;
        Name = name;
        Email = email;
        IsManager = isManager;
    }
}