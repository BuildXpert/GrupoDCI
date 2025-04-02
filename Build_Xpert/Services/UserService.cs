using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Build_Xpert.Model;

namespace Build_Xpert.Services
{

    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Obtener u
        public async Task<List<ApplicationUser>> GetUsersAsync()
        {
            return await _userManager.Users
                .Select(user => new ApplicationUser
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber
                }).ToListAsync();
        }


        // Crear un usuario
        public async Task<IdentityResult> CreateUserAsync(string email, string password)
        {
            var user = new ApplicationUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Asignar rol "Cliente" automáticamente
                if (!await _roleManager.RoleExistsAsync("Cliente"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Cliente"));
                }
                await _userManager.AddToRoleAsync(user, "Cliente");
            }

            return result;
        }

        // Editar un usuario
        public async Task<IdentityResult> UpdateUserAsync(string userId, string newEmail, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            user.Email = newEmail;
            user.UserName = newEmail; 

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded) return updateResult;

            // Actualizar el rol del usuario
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            return await _userManager.AddToRoleAsync(user, newRole);
        }


        // Eliminar un usuario
        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            return await _userManager.DeleteAsync(user);
        }

        // Obtener roles disponibles
        public async Task<List<string>> GetRolesAsync()
        {
            return await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        }

        // Asignar rol a un usuario
        public async Task<IdentityResult> AssignRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            return await _userManager.AddToRoleAsync(user, role);
        }

        // Remover rol de un usuario
        public async Task<IdentityResult> RemoveRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            return await _userManager.RemoveFromRoleAsync(user, role);
        }
        // Obtener roles de un usuario específico
        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new List<string>();

            return new List<string>(await _userManager.GetRolesAsync(user));
        }



        // Asignar un nuevo rol a un usuario
        public async Task<IdentityResult> ChangeUserRoleAsync(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            return await _userManager.AddToRoleAsync(user, newRole);
        }

        // Obtener lista de usuarios por rol específico
        public async Task<List<ApplicationUser>> GetUsersByRoleAsync(string role)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(role);
            return usersInRole.ToList();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user ?? throw new Exception("Usuario no encontrado");
        }



    }


}
