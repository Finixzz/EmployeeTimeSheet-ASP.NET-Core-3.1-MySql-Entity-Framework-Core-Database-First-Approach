using DAL.DTOS;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class UserSQLRepository : IUserSQLRepository
    {
        private SignInManager<Aspnetusers> _signInManager;
        private UserManager<Aspnetusers> _userManager;
        private employeetimesheetdbContext _appDbContext;

        public UserSQLRepository(UserManager<Aspnetusers> _userManager,
                           SignInManager<Aspnetusers> _signInManager,
                           employeetimesheetdbContext _appDbContext)
        {
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._appDbContext = _appDbContext;
        }

        public async Task<Aspnetusers> CreateUserAsync(string username,string supervisedBy,string roleName)
        {
            Aspnetusers newUser = new Aspnetusers()
            {
                UserName = username,
                SupervisedBy=supervisedBy
                
            };
            
            var result1 = await _userManager.CreateAsync(newUser, "User1122??");
            var result2 = await _userManager.AddToRoleAsync(newUser, roleName);

            if (result1.Succeeded && result2.Succeeded)
                return newUser;
            return null;
        }

        public async Task<IEnumerable<UserViewModelDTO>> GetAllUsersAsync()
        {
            var result1 = from Aspnetusers in _appDbContext.Aspnetusers
                         join Aspnetuserroles in _appDbContext.Aspnetuserroles
                            on Aspnetusers.Id equals Aspnetuserroles.UserId
                         join Aspnetroles in _appDbContext.Aspnetroles
                            on Aspnetuserroles.RoleId equals Aspnetroles.Id
                        where Aspnetuserroles.Role.Name!="Admin"
                         select new
                         {
                             Aspnetusers.Id ,
                             Aspnetusers.UserName,
                             Aspnetroles.Name,
                             Aspnetusers.SupervisedBy
                         };

            var result2=await result1.ToListAsync();

            List<UserViewModelDTO> userViewModelDTOs = new List<UserViewModelDTO>();
            for(int i = 0; i < result2.Count(); i++)
            {
                UserViewModelDTO uvmDto = new UserViewModelDTO()
                {
                    Id = result2.ElementAt(i).Id,
                    Username = result2.ElementAt(i).UserName,
                    RoleName = result2.ElementAt(i).Name,
                    SupervisedBy = result2.ElementAt(i).SupervisedBy
                };
                userViewModelDTOs.Add(uvmDto);
            }
            return userViewModelDTOs;
        }

        public async Task<UserViewModelDTO> GetUserViewModelDTOByIdAsync(string userId)
        {
            
            var result1 = from Aspnetusers in _appDbContext.Aspnetusers
                          join Aspnetuserroles in _appDbContext.Aspnetuserroles
                             on Aspnetusers.Id equals Aspnetuserroles.UserId
                          join Aspnetroles in _appDbContext.Aspnetroles
                             on Aspnetuserroles.RoleId equals Aspnetroles.Id
                          where Aspnetusers.Id==userId
                          select new 
                          {
                              Aspnetusers.Id,
                              Aspnetusers.UserName,
                              Aspnetroles.Name,
                              Aspnetusers.SupervisedBy
                          };

            var result2 = await result1.ToListAsync();


            return new UserViewModelDTO()
            {
                Id = result2[0].Id,
                Username = result2[0].UserName,
                RoleName = result2[0].Name,
                SupervisedBy = result2[0].SupervisedBy
            };


        }




        public async Task<Aspnetusers> GetUserByIdAsync(string userId)
        {
            return await _appDbContext.Aspnetusers.SingleOrDefaultAsync(c => c.Id == userId);
        }

        
        

        public async Task<bool> IsUsernameInUseAsync(string username)
        {
            
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return false;
            return true;   
        }

        public async Task<bool> SignInAsync(string username, string password,bool rememberMe)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password,
                                   rememberMe, false);

            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> SignOutAsync()
        {
            await _signInManager.SignOutAsync();
            return true;
        }

     

        public async Task<IEnumerable<Aspnetroles>> GetAllRolesAsync()
        {
            return await _appDbContext.Aspnetroles.ToListAsync();
        }

        public async Task<Aspnetroles> GetUserRoleAsync(string userId)
        {
            var result =  from Aspnetusers in _appDbContext.Aspnetusers
                         join Aspnetuserroles in _appDbContext.Aspnetuserroles
                             on Aspnetusers.Id equals Aspnetuserroles.UserId
                         join Aspnetroles in _appDbContext.Aspnetroles
                            on Aspnetuserroles.RoleId equals Aspnetroles.Id
                         where Aspnetusers.Id == userId
                         select Aspnetroles;
            var result2= await result.ToListAsync();
            var role = result2.ElementAt(0);

            
           return new Aspnetroles()
            {
                Id=role.Id,
                Name=role.Name,
                NormalizedName=role.NormalizedName,
                ConcurrencyStamp=role.NormalizedName
            };

        }

        public async Task<Aspnetusers> DeleteUserAsync(string userId)
        {
            Aspnetusers userInDb = await GetUserByIdAsync(userId);
            _appDbContext.Aspnetusers.Remove(userInDb);
            await _appDbContext.SaveChangesAsync();
            return userInDb;
        }

        public async Task<IEnumerable<UserViewModelDTO>> GetAllSupervisorsAsync()
        {
            var result1 = from Aspnetusers in _appDbContext.Aspnetusers
                          join Aspnetuserroles in _appDbContext.Aspnetuserroles
                             on Aspnetusers.Id equals Aspnetuserroles.UserId
                          join Aspnetroles in _appDbContext.Aspnetroles
                             on Aspnetuserroles.RoleId equals Aspnetroles.Id
                          where Aspnetuserroles.Role.Name == "Supervisor"
                          select new
                          {
                              Aspnetusers.Id,
                              Aspnetusers.UserName,
                              Aspnetroles.Name,
                              Aspnetusers.SupervisedBy
                          };

            var result2 = await result1.ToListAsync();

            List<UserViewModelDTO> userViewModelDTOs = new List<UserViewModelDTO>();
            for (int i = 0; i < result2.Count(); i++)
            {
                UserViewModelDTO uvmDto = new UserViewModelDTO()
                {
                    Id = result2.ElementAt(i).Id,
                    Username = result2.ElementAt(i).UserName,
                    RoleName = result2.ElementAt(i).Name,
                    SupervisedBy = result2.ElementAt(i).SupervisedBy
                };
                userViewModelDTOs.Add(uvmDto);
            }
            return userViewModelDTOs;
        }

        public async Task<Aspnetusers> EditUserAsync(string userId, string username, string supervisedBy, string roleName)
        {
            Aspnetusers userInDb = await _userManager.FindByIdAsync(userId);
            userInDb.UserName = username;
            userInDb.SupervisedBy = supervisedBy;
            var userRoles = await _userManager.GetRolesAsync(userInDb);
            

            var result1 = await _userManager.RemoveFromRolesAsync(userInDb, userRoles);


            var result2 = await _userManager.AddToRoleAsync(userInDb, roleName);

            if (result1.Succeeded && result2.Succeeded)
                return userInDb;

            return null;
        }

        public async Task<IEnumerable<UserViewModelDTO>> GetAllSupervisedUsersBySupervisorAsync(string supervisorId)
        {
            var result1 = from Aspnetusers in _appDbContext.Aspnetusers
                          join Aspnetuserroles in _appDbContext.Aspnetuserroles
                             on Aspnetusers.Id equals Aspnetuserroles.UserId
                          join Aspnetroles in _appDbContext.Aspnetroles
                             on Aspnetuserroles.RoleId equals Aspnetroles.Id
                          where Aspnetusers.SupervisedBy == supervisorId
                          select new
                          {
                              Aspnetusers.Id,
                              Aspnetusers.UserName,
                              Aspnetroles.Name,
                              Aspnetusers.SupervisedBy
                          };

            var result2 = await result1.ToListAsync();

            List<UserViewModelDTO> userViewModelDTOs = new List<UserViewModelDTO>();
            for (int i = 0; i < result2.Count(); i++)
            {
                UserViewModelDTO uvmDto = new UserViewModelDTO()
                {
                    Id = result2.ElementAt(i).Id,
                    Username = result2.ElementAt(i).UserName,
                    RoleName = result2.ElementAt(i).Name,
                    SupervisedBy = result2.ElementAt(i).SupervisedBy
                };
                userViewModelDTOs.Add(uvmDto);
            }
            return userViewModelDTOs;
        }

        public async Task<IEnumerable<UserViewModelDTO>> GetAllUsersWithWorkingHours()
        {
            var query = from u in _appDbContext.Aspnetusers
                         join pi in _appDbContext.Projectinfo
                              on u.Id equals pi.UserId into q1
                         from pi in q1
                         group new {u} by new {u.Id,u.UserName,u.SupervisedBy} into q 
                         select new
                         {
                             Id=q.Key.Id,
                             Username=q.Key.UserName,
                             SupervisedBy=q.Key.SupervisedBy
                         };
            var queryResult = await query.ToListAsync();

            List<UserViewModelDTO> modelList = new List<UserViewModelDTO>();
            for(int i = 0; i < queryResult.Count(); i++)
            {
                UserViewModelDTO model = new UserViewModelDTO()
                {
                    Id = queryResult.ElementAt(i).Id,
                    Username = queryResult.ElementAt(i).Username,
                    SupervisedBy = queryResult.ElementAt(i).SupervisedBy
                };
                modelList.Add(model);
            }
            return modelList;
        }

        public async Task<IEnumerable<UserViewModelDTO>> GetAllSupervisedUsersWithWorkingHours(string supervisorId)
        {
            var query = from u in _appDbContext.Aspnetusers
                        join pi in _appDbContext.Projectinfo
                             on u.Id equals pi.UserId into q1
                        from pi in q1
                        where u.SupervisedBy==supervisorId
                        group new { u } by new { u.Id, u.UserName, u.SupervisedBy } into q
                        select new
                        {
                            Id = q.Key.Id,
                            Username = q.Key.UserName,
                            SupervisedBy = q.Key.SupervisedBy
                        };
            var queryResult = await query.ToListAsync();

            List<UserViewModelDTO> modelList = new List<UserViewModelDTO>();
            for (int i = 0; i < queryResult.Count(); i++)
            {
                UserViewModelDTO model = new UserViewModelDTO()
                {
                    Id = queryResult.ElementAt(i).Id,
                    Username = queryResult.ElementAt(i).Username,
                    SupervisedBy = queryResult.ElementAt(i).SupervisedBy
                };
                modelList.Add(model);
            }
            return modelList;
        }

        public Aspnetusers GetUserById(string userId)
        {
            return _appDbContext.Users.SingleOrDefault(c => c.Id == userId);
        }
    }
}
