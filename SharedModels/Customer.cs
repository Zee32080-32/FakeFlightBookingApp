using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    public class Customer
    {
        [Key] // This attribute specifies that this is the primary key
        public int Id { get; set; }
        public string Salt { get; set; } = string.Empty; // Default to empty string

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }



        // Constructor for the base user
        public Customer(string firstName, string lastName, string userName, string email, string password, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
        }

        public bool ValidatePassword(string inputPassword)
        {
            return Password == inputPassword;
        }

        public void UpdateProfile(string firstName, string lastName, string email, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }


    }
}
