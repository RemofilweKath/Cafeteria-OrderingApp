using CafeteriaOrderingApp.Models;
using CafeteriaOrderingApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CafeteriaOrderingApp.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: Employees
        public IActionResult Index()
        {
            var employees = _employeeService.GetAllEmployees();
            return View(employees);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            var employee = new Employee
            {
                LastDepositMonth = DateTime.Now // Sets to current date, but model setter will normalize to first of the month
            };
            return View();
        }

        //// GET: Employees/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id != null)
            {
                var employee = _employeeService.GetEmployeeById(id);
                if (employee == null)
                {
                    return NotFound();
                }

                return View(employee);
            }

            return NotFound();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _employeeService.AddEmployee(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public IActionResult Edit(int id)
        {
            var employee = _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _employeeService.UpdateEmployee(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _employeeService.GetEmployeeById(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _employeeService.DeleteEmployee(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Deposit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Deposit(string employeeNumber, decimal amount)
        {
            _employeeService.Deposit(employeeNumber, amount);
            return RedirectToAction(nameof(Index));
        }
    }
}
