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

            if (employee == null || amount <= 0)
            {
                throw new ArgumentException("Invalid employee or deposit amount.");
            }

            // Reset Balance every new month
            if (employee.LastDepositMonth.Month != DateTime.Now.Month || employee.LastDepositMonth.Year != DateTime.Now.Year)
            {
                employee.MonthlyDepositBalance = 0;
            }

            // Store the balance before the deposit
            decimal monthlyDepositBefore = employee.MonthlyDepositBalance;
            decimal monthlyDepositAfter = monthlyDepositBefore + amount;

            // Update balance and monthly deposit tracking
            employee.Balance += amount;
            employee.MonthlyDepositBalance += amount;
            employee.LastDepositMonth = DateTime.Now;

            // Bonus Logic: Apply R500 bonus for every R250 increment crossed in current month
            long bonusThresholdBefore = (long)(monthlyDepositBefore / 250);
            long bonusThresholdAfter = (long)(monthlyDepositAfter / 250);

            if (bonusThresholdAfter > bonusThresholdBefore)
            {
                // Calculate how many R250 thresholds were crossed with this deposit
                decimal bonusAmount = (bonusThresholdAfter - bonusThresholdBefore) * 500;
                employee.Balance += bonusAmount;
            }

            return employee.Balance;
        }
    }
}
