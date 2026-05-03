using TaskTrackerApp.Domain.Entities;
using TaskTrackerApp.Domain.Interfaces;
using TaskTrackerApp.Persistence.Data;

namespace TaskTrackerApp.Persistence.Repositories;

public sealed class PasswordResetTokenRepository(AppDbContext context) : GenericRepository<PasswordResetToken>(context), IPasswordResetTokenRepository; 