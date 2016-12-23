using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class RegisterModel
    {
        public string UserName { set; get; }
        public string Name { set; get; }
        public string Family{ set; get; }
        public string Mobile{ set; get; }
        public string Phone{ set; get; }
        public string Email{ set; get; }
        public string Pass{ set; get; }
    }
    public class LoginModel
    {
        public string Mobile{ set; get; }
        public string Pass{ set; get; }
    }

    public class UserModels
    {

    }
}