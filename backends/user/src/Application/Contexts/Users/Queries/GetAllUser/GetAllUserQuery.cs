using Application.Contexts.Users.Dtos;
using Domain.Messaging;
using MediatR;

namespace Application.Contexts.Users.Queries.GetAllUser;

public class GetAllUserQuery: IRequest<IReadOnlyCollection<UserDto>>, IRequireManager
{
    public GetAllUserQuery() {}
    
    public string? TokenId { get; set; }
    public string? TokenEmail { get; set; }
}