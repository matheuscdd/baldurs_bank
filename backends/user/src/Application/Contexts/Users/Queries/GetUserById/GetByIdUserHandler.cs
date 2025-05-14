using Application.Contexts.Users.Dtos;
using Application.Contexts.Users.Repositories;
using Domain.Exceptions;
using MediatR;
using Mapster;

namespace Application.Contexts.Users.Queries.GetUserById;

public class GetByIdUserHandler: IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUserRepository _userRepository;
    public GetByIdUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto?> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _userRepository.GetByIdAsync(request.UserId,  cancellationToken);
        if (entity == null)
        {
            throw new NotFoundCustomException("User not found");
        }

        var dto = entity.Adapt<UserDto>();
        return dto;
    }
}