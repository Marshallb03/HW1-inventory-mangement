using CRM.Library.Services;
using CRM.Models;

namespace Summer2024_Example
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var contactSvc = InventoryManagementSystem.Current;

            contactSvc.Start();

        }
    }
}
