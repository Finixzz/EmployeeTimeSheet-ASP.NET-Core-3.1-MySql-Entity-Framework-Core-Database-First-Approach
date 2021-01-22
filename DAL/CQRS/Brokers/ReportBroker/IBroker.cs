using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.CQRS.Brokers.ReportBroker
{
    public interface IBroker
    {
        Task<dynamic> sendMessage(IQuery query);
    }

}
