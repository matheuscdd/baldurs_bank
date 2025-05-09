namespace Application.Contexts.Accounts.Dtos;

public class AccountDto
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public string UserId { get; set; }
    public bool IsActive { get; set; }
    
    public AccountDto() {}
}