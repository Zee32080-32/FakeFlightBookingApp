using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    public class ForgotPassword_Poco
    {
        public required string Email { get; set; }
        public required string? newPassword { get; set; }

        public required string code { get; set; } 


    }
}
