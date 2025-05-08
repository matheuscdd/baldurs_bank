using Application.Contexts.Users.Dtos;
using MediatR;
using Domain.Messaging;
using FirebaseAdmin.Auth;

namespace Application.Contexts.Users.Commands.Validate;

public class ValidateUserCommand : IRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}