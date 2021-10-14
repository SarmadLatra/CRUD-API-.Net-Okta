using CRUD_API.Data.Entities;
using CRUD_API.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_API.Services
{
    public static class EmplyeeData
    {
        public static List<Employee> GetEmplyeeData()
        {
            return  new List<Employee>() {
               new Employee('1', "Sarmad", "03037498388", "sarmadsaeed13@gmail.com", "sdasdsad", "Engineer"),
               new Employee('2', "Sumair", "03037543388", "sarm@gmail.com", "sdasdsad", "Engineer")};
        }
        public static List<Employee> LoadJson()
        {
            using (StreamReader r = new StreamReader(Constants.jsonFilePath))
            {
                string json = r.ReadToEnd();
                List<Employee> items = JsonConvert.DeserializeObject<List<Employee>>(json);
                return items;
            }
        }
    }
}
