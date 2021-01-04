using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leave_Management.Data
{
    public class Employee : IdentityUser
    {
        /*
         Console Package :
        add-migration <NAME>    : Create a migration script in order to modify the database
        update-database         : Apply all scripts.

         */

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string TaxId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateJoined { get; set; }

    }
}
