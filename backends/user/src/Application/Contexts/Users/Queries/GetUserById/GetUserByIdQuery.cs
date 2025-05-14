using Application.Contexts.Users.Dtos;
using Domain.Messaging;
using MediatR;

namespace Application.Contexts.Users.Queries.GetUserById;

public class GetUserByIdQuery: IRequest<UserDto?>, IRequireAuth
{
    public string UserId { get; set; }
    public string? TokenId { get; set; }
    public string? TokenEmail { get; set; }
    
    public GetUserByIdQuery() {}
}