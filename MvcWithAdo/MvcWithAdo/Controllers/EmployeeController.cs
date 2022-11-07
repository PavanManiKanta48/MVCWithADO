using Microsoft.AspNetCore.Mvc;
using MvcWithAdo.Models;
using System.Data;
using System.Data.SqlClient;

namespace MvcWithAdo.Controllers
{
    public class EmployeeController : Controller
    {
        public IConfiguration _configuration;
        SqlConnection con;
        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
            con = new SqlConnection(_configuration.GetConnectionString("Pavan"));
        }
        // GET: Employee_Controller
        public ActionResult Index()
        {
            List<Employee> employees = new List<Employee>();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Employee";
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                employees.Add(new Employee
                {
                    Id = Convert.ToInt32(sdr["Id"]),
                    Name = Convert.ToString(sdr["Name"]),
                    Email = Convert.ToString(sdr["Email"])
                });
            }
            return View(employees);
        }
        // GET: Employee_Controller/Details/5
        [HttpGet]
        [ValidateAntiForgeryToken]
        public Employee Detail(int id)
        {
            Employee employee = new Employee();
            try
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Employee Where(Id=" + id + ")";
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                employee = (new Employee
                {
                    Id = Convert.ToInt32(sdr["Id"]),
                    Name = sdr["Name"].ToString(),
                    Email = sdr["Email"].ToString(),
                });
                sdr.Close();
            }
            finally
            {
                con.Close();
            }
            return employee;
        }
        public ActionResult Details(int id)
        {
            return View(Detail(id));
        }
        // GET: Employee_Controller/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Employee_Controller/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            try
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Insert into Employee(Id,Name,Email) values(" + employee.Id + ",'" + employee.Name + "','" + employee.Email + "')";
                con.Open();
                cmd.ExecuteNonQuery();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
            finally
            {
                con.Close();
            }
        }
        // GET: Employee_Controller/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // POST: Employee_Controller/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Employee employee)
        {
            try
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Update Employee Set Name='" + employee.Name + "',Email= '" + employee.Email + "' Where(Id= " + id + ");";
                con.Open();
                cmd.ExecuteNonQuery();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
            finally
            {
                con.Close();
            }
        }
        // GET: Employee_Controller/Delete/5
        public ActionResult Delete(int id)
        {
            return View(Detail(id));
        }
        // POST: Employee_Controller/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Employee employee)
        {
            try
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Delete from Employee Where(ID=" + id + ")";
                con.Open();
                cmd.ExecuteNonQuery();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
            finally
            {
                con.Close();
            }
        }
    }
}
