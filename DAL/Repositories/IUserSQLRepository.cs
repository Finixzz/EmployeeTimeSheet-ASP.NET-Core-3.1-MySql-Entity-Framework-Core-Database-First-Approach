using DAL.DTOS;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace DAL.Repositories
{
    public interface IUserSQLRepository
    {

        Task<IEnumerable<UserViewModelDTO>> GetAllUsersAsync();

        Task<IEnumerable<UserViewModelDTO>> GetAllSupervisorsAsync();

        Task<IEnumerable<UserViewModelDTO>> GetAllSupervisedUsersBySupervisorAsync(string supervisorId);

        Task<IEnumerable<UserViewModelDTO>> GetAllUsersWithWorkingHours();

        Task<IEnumerable<UserViewModelDTO>> GetAllSupervisedUsersWithWorkingHours(string supervisorId);
        Task<Aspnetusers> GetUserByIdAsync(string userId);

        Aspnetusers GetUserById(string userId);

        Task<UserViewModelDTO> GetUserViewModelDTOByIdAsync(string userId);


        Task<Aspnetusers> EditUserAsync(string userId, string username, string supervisedBy, string roleName);
             
        Task<Aspnetusers> DeleteUserAsync(string userId);

        Task<IEnumerable<Aspnetroles>> GetAllRolesAsync();

        Task<Aspnetroles> GetUserRoleAsync(string userId);

        //Additional methods
        Task<Aspnetusers> CreateUserAsync(string username,string supervisedBy, string roleName);

        Task<bool> SignInAsync(string username, string password, bool rememberMe);


        Task<bool> SignOutAsync();

        Task<bool> IsUsernameInUseAsync(string username);




    }
}
