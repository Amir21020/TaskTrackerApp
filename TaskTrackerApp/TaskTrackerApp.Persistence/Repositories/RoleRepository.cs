using TaskTrackerApp.Domain.Entities;
using TaskTrackerApp.Domain.Interfaces;
using TaskTrackerApp.Persistence.Data;

namespace TaskTrackerApp.Persistence.Repositories;

public sealed class RoleRepository(AppDbContext context) : GenericRepository<Role, int>(context), IRoleRepository;