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
