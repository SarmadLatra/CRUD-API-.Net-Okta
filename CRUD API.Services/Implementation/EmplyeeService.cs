using AutoMapper;
using CRUD_API.Data.Entities;
using CRUD_API.Services.Interfaces;
using CRUD_API.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_API.Services.Implementation
{
    public class EmplyeeService : IEmployee
    {
        private readonly IMapper _mapper;

        public EmplyeeService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public List<Employee> emplyeeData = EmplyeeData.LoadJson();
        public async Task<bool> AddNewEmployee(EmplyeeModel emplyeeModel)
        {
            try
            {
                if (emplyeeData.Any(x => x.EmployeeId == emplyeeModel.EmployeeId))
                    return false;

                emplyeeData.Add(_mapper.Map<Employee>(emplyeeModel));
                string json = JsonConvert.SerializeObject(emplyeeData);
                File.WriteAllText(Constants.jsonFilePath, json);
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }

        public async Task<EmplyeeModel> DeleteEmployee(int emplyeeId)
        {
            try
            {
                if (!emplyeeData.Any(x => x.EmployeeId == emplyeeId))
                    throw new Exception(Constants.errorException);

                var itemToRemove = emplyeeData.FirstOrDefault(r => r.EmployeeId == emplyeeId);
                emplyeeData.Remove(itemToRemove);
                string json = JsonConvert.SerializeObject(emplyeeData);
                File.WriteAllText(Constants.jsonFilePath, json);
                return _mapper.Map<EmplyeeModel>(itemToRemove);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<EmplyeeModel>> GetAllEmployees()
        {
            return emplyeeData.Select(s => _mapper.Map<EmplyeeModel>(s));
        }

        public async Task<EmplyeeModel> GetEmployee(int emplyeeId)
        {
            try
            {
                if (!emplyeeData.Any(x => x.EmployeeId == emplyeeId))
                    throw new Exception(Constants.errorException);

                return _mapper.Map<EmplyeeModel>(emplyeeData.FirstOrDefault(x => x.EmployeeId == emplyeeId));
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<EmplyeeModel> UpdateEmployee(int emplyeeId, EmplyeeModel emplyeeModel)
        {
            try
            {
                if (!emplyeeData.Any(x => x.EmployeeId == emplyeeId))
                    throw new Exception(Constants.errorException);

                var itemToUpdate = emplyeeData.FirstOrDefault(x => x.EmployeeId == emplyeeId);
                itemToUpdate.Email = emplyeeModel.Email;
                itemToUpdate.Designation = emplyeeModel.Designation;
                itemToUpdate.Address = emplyeeModel.Address;
                itemToUpdate.Name = emplyeeModel.Name;
                itemToUpdate.PhoneNumber = emplyeeModel.PhoneNumber;
                string json = JsonConvert.SerializeObject(emplyeeData);
                File.WriteAllText(Constants.jsonFilePath, json);
                return _mapper.Map<EmplyeeModel>(itemToUpdate);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}