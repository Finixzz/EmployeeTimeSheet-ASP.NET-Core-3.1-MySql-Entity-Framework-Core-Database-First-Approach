using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IProjectInfoSQLRepository
    {
        Task<IEnumerable<Projectinfo>> GetAllAsync();
        Task<Projectinfo> GetByIdAsync(int projectInfoId);

        Task<Projectinfo> SaveAsync(Projectinfo projectInfo);

        Task<Projectinfo> DeleteAsync(int projectInfoId);
    }
}
