﻿namespace Gizo.Application.Users.Dtos;

public class CurrentUserDto
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Token { get; set; }
}