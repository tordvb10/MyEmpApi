
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEmpApi.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using MyEmpApi.Data;

namespace MyEmpApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly DatabaseHelper _databaseHelper;

        public EmployeesController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        [HttpGet]
        [Route("GetAllEmployees")]
        public string GetEmployees()
        {
            DataTable dt = _databaseHelper.ExecuteScript("SELECT * FROM Employees");
            List<Employee> employeeList = new List<Employee>();
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Employee employee = new Employee
                    {
                        Id = Convert.ToInt32(dt.Rows[i]["EmpId"]),
                        EmpName = Convert.ToString(dt.Rows[i]["EmpName"]),
                        Password = Convert.ToString(dt.Rows[i]["Password"])
                    };
                    employeeList.Add(employee);
                }
            }
            if (employeeList.Count > 0)
            {
                return JsonConvert.SerializeObject(employeeList);
            }
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No data found";
                return JsonConvert.SerializeObject(response);
            }
        }
    }
}