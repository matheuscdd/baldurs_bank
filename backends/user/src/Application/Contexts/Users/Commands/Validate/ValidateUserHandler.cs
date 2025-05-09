using Application.Contexts.Users.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MapsterMapper;
using MediatR;

namespace Application.Contexts.Users.Commands.Validate;

public class ValidateUserHandler : IRequestHandler<ValidateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public ValidateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(
        ValidateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var emailExists = await _userRepository.CheckEmailExistsAsync(request.Email, cancellationToken);
        if (emailExists)
        {
            throw new ConflictCustomException($"{nameof(request.Email)} already exists");
        }

        new User(request.Name, request.Email, request.Password);
    }
}