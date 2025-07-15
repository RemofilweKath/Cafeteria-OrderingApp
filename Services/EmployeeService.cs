using CafeteriaOrderingApp.Database;
using CafeteriaOrderingApp.Models;

namespace CafeteriaOrderingApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public Employee GetEmployeeById(int id)
        {
            return _context.Employees.Find(id);
        }

        public Employee GetEmployee(string employeeNumber)
        {
            return _context.Employees.FirstOrDefault(e => e.EmployeeNumber == employeeNumber);
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _context.Employees.ToList();
        }

        public void UpdateEmployee(Employee employee)
        {
            var existingEmployee = _context.Employees.Find(employee.Id);
            if (existingEmployee != null)
            {
                existingEmployee.Name = employee.Name;
                existingEmployee.EmployeeNumber = employee.EmployeeNumber;
                existingEmployee.Balance = employee.Balance; // Update balance only if needed
                existingEmployee.LastDepositMonth = employee.LastDepositMonth; // Update if applicable

                _context.SaveChanges();
            }
        }

        public void DeleteEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
        }

        public decimal Deposit(string employeeNumber, decimal amount)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeNumber == employeeNumber);
            if (employee != null && amount > 0)
            {
                employee.Balance += amount;

                // Bonus Logic
                if ((employee.Balance - amount) % 250 == 0)
                {
                    employee.Balance += 500; // Apply bonus
                }

                employee.LastDepositMonth = DateTime.Now;
                _context.SaveChanges();
                return employee!.Balance;
            }
            return 0.0m; // Return 0
        }
    }
}
