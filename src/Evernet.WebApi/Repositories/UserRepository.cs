using Evernet.WebApi.Data;
using Evernet.WebApi.Entities;
using Evernet.WebApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Evernet.WebApi.Repositories;

public class UserRepository(EvernetDbContext context) : IUserRepository
{
    public Task<User?> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));
        }

        return context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }

        return context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public Task AddAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null.");
        }

        context.Users.Add(user);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }
}