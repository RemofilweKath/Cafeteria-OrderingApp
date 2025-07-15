using CafeteriaOrderingApp.Models;

namespace CafeteriaOrderingApp.Services
{
    public interface IEmployeeService
    {
        void AddEmployee(Employee employee);
        Employee GetEmployeeById(int id);
        Employee GetEmployee(string employeeNumber);
        IEnumerable<Employee> GetAllEmployees();
        void UpdateEmployee(Employee employee);
        decimal Deposit(string employeeNumber, decimal amount);
        void DeleteEmployee(int id);
    }
}