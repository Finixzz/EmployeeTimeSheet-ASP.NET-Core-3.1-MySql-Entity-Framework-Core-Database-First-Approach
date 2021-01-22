using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DTOS
{
    public class UserViewModelDTO
    {
        public string Id { get; set; }
        public string Username { get; set; }

        public string RoleName { get; set; }

        public string SupervisedBy { get; set; }
    }
}
