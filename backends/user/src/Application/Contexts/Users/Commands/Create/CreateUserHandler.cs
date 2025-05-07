using Application.Contexts.Users.Dtos;
using Application.Contexts.Users.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Mapster;
using MapsterMapper;
using MediatR;

namespace Application.Contexts.Users.Commands.Create;
// TODO - criar um só pra admin
public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public CreateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(
        CreateUserCommand request,
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
        
        var entity = await _userRepository.CreateAsync(
            new User(request.Id, request.Name, request.Email, false),
            cancellationToken
        );
        var dto = entity.Adapt<UserDto>();
        return dto;
    }
}