﻿namespace Core.Security.Authorization;

public class AccessToken
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}