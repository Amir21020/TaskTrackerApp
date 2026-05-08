using TaskTrackerApp.Application.Exceptions;
using TaskTrackerApp.Application.Interfaces;
using TaskTrackerApp.Domain.Entities;
using TaskTrackerApp.Domain.Interfaces;

namespace TaskTrackerApp.Infrastructure.Services;

public sealed class UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository) : IUserService
{
    public async Task CompleteOnboardingAsync(Guid userId, int roleId, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(userId, ct);
        if(user is null) throw new NotFoundException("Пользователь не найден");
             
        var role = await roleRepository.GetByIdAsync(roleId, ct);
        if (role is null) throw new BadRequestException("роль не найдена");

        var userRole = new UserRole
        {
            RoleId = roleId,
            UserId = userId,
        };

        await userRoleRepository.AddAsync(userRole, ct);

        await unitOfWork.SaveChangesAsync(ct);
    }
}