namespace Domain.Entities;

public class User 
{
    public Guid Id { get; protected set; }
    public string Name { get; protected set; }

    public User(string name)
    {
        Name = name;
    }
}