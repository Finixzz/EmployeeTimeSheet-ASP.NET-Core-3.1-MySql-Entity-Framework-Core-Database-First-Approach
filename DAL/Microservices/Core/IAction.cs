using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Microservices
{
    public interface IAction
    {
        Task<dynamic> executeAsync();
    }
}
