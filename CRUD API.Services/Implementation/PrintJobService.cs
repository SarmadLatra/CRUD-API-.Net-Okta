using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_API.Services.Implementation
{
    public class PrintJobService : IPrintJobService
    {
        public void PrintMessage()
        {
            Console.WriteLine("Hangfire Recurring Job");
        }
    }
}
