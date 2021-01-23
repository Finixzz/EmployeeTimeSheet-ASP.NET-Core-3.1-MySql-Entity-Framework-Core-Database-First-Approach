using DAL.Microservices;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.CQRS.Brokers.ReportBroker
{
    public class Broker : IBroker
    {
        public async Task<dynamic> sendMessage(IAction action)
        {
            return await action.executeAsync();
        }
    }
}
