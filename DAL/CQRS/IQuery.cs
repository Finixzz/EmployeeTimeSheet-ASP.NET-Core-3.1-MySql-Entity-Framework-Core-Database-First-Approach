using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.CQRS
{

    public interface IQuery
    {
        Task<dynamic> executeAsync();
    }

   
}
