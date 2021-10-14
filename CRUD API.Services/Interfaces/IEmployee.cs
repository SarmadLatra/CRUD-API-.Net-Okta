using CRUD_API.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_API.Services.Interfaces
{
    public interface IEmployee
    {
        Task<IEnumerable<EmplyeeModel>> GetAllEmployees();
        Task<EmplyeeModel> GetEmployee(int emplyeeId);
        Task<bool> AddNewEmployee(EmplyeeModel emplyeeModel);
        Task<EmplyeeModel> UpdateEmployee(int emplyeeId, EmplyeeModel emplyeeModel);
        Task<EmplyeeModel> DeleteEmployee(int emplyeeId);
    }
}
