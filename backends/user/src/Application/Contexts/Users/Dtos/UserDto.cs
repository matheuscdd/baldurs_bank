namespace Application.Contexts.Users.Dtos;

public class UserDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsManager { get; set; }
    public bool IsActive { get; set; }
    public UserDto() {}
}