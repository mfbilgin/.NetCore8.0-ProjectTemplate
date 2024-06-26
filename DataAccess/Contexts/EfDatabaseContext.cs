﻿using Core.Entities.Concretes;
using Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Contexts;

// If you want to use the Entity Framework Core in your project, you can use this context class.

public sealed class EfDatabaseContext(IConfiguration configuration) : DbContext
{
    private readonly IConfiguration? _configuration = configuration;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_configuration == null) return;
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }
    
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<EmailVerification> EmailVerifications { get; set; }
    public DbSet<Log> Logs { get; set; }
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
}