using Application.Contexts.Users.Dtos;
using Domain.Messaging;
using MediatR;

namespace Application.Contexts.Users.Queries.GetById;

public class GetByIdUserQuery: IRequest<UserDto?>, IRequireAuth
{
    public required string Id { get; set; }
    public string? TokenId { get; set; }
    public string? TokenEmail { get; set; }
    
    public GetByIdUserQuery(string id)
    {
        Id = id;
    }

    public GetByIdUserQuery() {}
}