﻿using Business.Abstracts;
using Business.Constants;
using Business.Constants.Messages;
using Core.Entities.Concretes;
using Core.Entities.Dtos.Auth;
using Core.Mailing;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(
    IAuthService authService,
    IUserService userService,
    IEmailVerificationService emailVerificationService,
    IMailService mailService) : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDto registerDto)
    {
        var user = authService.Register(registerDto);
        userService.Add(user);
        var emailVerification = emailVerificationService.Add(user.Username);
        mailService.SendWelcomeMail(user.Email, emailVerification.Username,emailVerification.Token);
        return Ok(UserMessages.UserRegistered);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        var user = authService.Login(loginDto);
        var token = authService.CreateAccessToken(user);
        return Ok(token);
    }
    
    [HttpGet("verify-email")]
    public IActionResult VerifyEmail([FromQuery] string username, [FromQuery] string token)
    {
        var emailVerification = new EmailVerification
        {
            Token = token,
            Username = username
        };
        emailVerificationService.VerifyEmail(emailVerification);
        userService.VerifyEmail(username);
        return Ok(EmailVerificationMessages.EmailHasBeenVerified);
    }
    
    [HttpPost("resend-verification-email")]
    public IActionResult ResendVerificationEmail([FromBody] ResendVerificationEmailDto resendVerificationEmailDto)
    {
        var emailVerification = emailVerificationService.Add(resendVerificationEmailDto.Username);
        mailService.SendWelcomeMail(resendVerificationEmailDto.Email, emailVerification.Username,emailVerification.Token);
        return Ok(EmailVerificationMessages.EmailHasBeenSent);
    }
}