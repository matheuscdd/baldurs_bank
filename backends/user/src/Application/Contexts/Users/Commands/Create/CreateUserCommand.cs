using Application.Contexts.Users.Dtos;
using MediatR;

namespace Application.Contexts.Users.Commands.Create;

public class CreateUserCommand : IRequest<UserDto>
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
}