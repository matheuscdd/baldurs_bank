using Application.Contexts.Users.Dtos;
using MediatR;
using Domain.Messaging;
using FirebaseAdmin.Auth;

namespace Application.Contexts.Users.Commands.Create;

public class CreateUserCommand : IRequest<UserDto>, IRequireAuth
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? TokenId { get; set; }
    public string? TokenEmail { get; set; }
    public bool? TokenIsManager { get; set; }
    public FirebaseToken? firebaseToken { get; set; }
}