using Application.Contexts.Users.Dtos;
using Application.Contexts.Users.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Mapster;
using MapsterMapper;
using MediatR;
using FirebaseAdmin.Auth;

namespace Application.Contexts.Users.Commands.Validate;
// TODO - criar um só pra admin
public class ValidateUserHandler : IRequestHandler<ValidateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public ValidateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(
        ValidateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var idExists = await _userRepository.CheckIdExistsAsync(request.Id, cancellationToken);
        if (idExists)
        {
            throw new ConflictCustomException($"{nameof(request.Id)} already exists");
        }

        var emailExists = await _userRepository.CheckEmailExistsAsync(request.Email, cancellationToken);
        if (emailExists)
        {
            throw new ConflictCustomException($"{nameof(request.Email)} already exists");
        }

        // TODO - depois colocar pra receber a senha e fazer as validações
        var entity = new User(request.Id, request.Name, request.Email, false);

        var dto = entity.Adapt<UserDto>();
        return dto;
    }
}