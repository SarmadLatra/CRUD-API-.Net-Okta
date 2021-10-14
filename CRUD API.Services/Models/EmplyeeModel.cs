using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_API.Services.Models
{
    public class EmplyeeModel
    {
        public EmplyeeModel() { }

        public EmplyeeModel(int _EmployeeId, string _Name, string _PhoneNumber, string _Email, string _Address, string _Designation)
        {
            EmployeeId = _EmployeeId;
            Name = _Name;
            PhoneNumber = _PhoneNumber;
            Email = _Email;
            Address = _Address;
            Designation = _Designation;
        }
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Designation { get; set; }
    }
}
