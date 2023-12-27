using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly IWebHostEnvironment _env;
		public EmployeeController(IConfiguration configuration,IWebHostEnvironment env)
		{
			_configuration = configuration;
			_env = env;
		}
		[HttpGet]
		public JsonResult Get()
		{
			string query = @"
                    SELECT EmployeeId, EmployeeName, Department,
                    CONVERT(varchar(10), DateOfJoining, 120) AS DateOfJoining,
                    PhotoFileName
                    FROM dbo.Employee";
			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("MyConnect");
			SqlDataReader myReader;
			try
			{
				using (SqlConnection myCon = new SqlConnection(sqlDataSource))
				{
					myCon.Open();
					using (SqlCommand myCommand = new SqlCommand(query, myCon))
					{
						myReader = myCommand.ExecuteReader();
						table.Load(myReader);
						myReader.Close();
						myCon.Close();
					}
				}
				return new JsonResult(table);
			}
			catch (Exception ex)
			{
				return new JsonResult(new { Error = ex.Message });
			}
		}
		[HttpPost]
		public JsonResult Post(Employee emp)
		{
			string query = @"
                    INSERT INTO dbo.Employee
                    (EmployeeName, Department, DateOfJoining, PhotoFileName)
                    VALUES (@EmployeeName, @Department, @DateOfJoining, @PhotoFileName)";
			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("MyConnect");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
					myCommand.Parameters.AddWithValue("@Department", emp.Department);
					myCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
					myCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}
			return new JsonResult(table);
		}

		[HttpPut]
		public JsonResult Put(Employee emp)
		{
			string query = @"
                    UPDATE dbo.Employee
                    SET EmployeeName= @EmployeeName,
                        Department=@Department,
                        DateOfJoining=@DateOfJoining,
                        PhotoFileName=@PhotoFileName
                    WHERE EmployeeId=@EmployeeId";
			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("MyConnect");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
					myCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
					myCommand.Parameters.AddWithValue("@Department", emp.Department);
					myCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
					myCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}
			return new JsonResult("Updated Successfully");
		}

		[HttpDelete("{id}")]
		public JsonResult Delete(int id)
		{
			string query = @"
							delete from dbo.Employee
							where EmployeeId=@EmployeeId";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("MyConnect");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@EmployeeId", id);
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}
			return new JsonResult("Delete Successfully");
		}

		[Route("SaveFile")]
		[HttpPost]
		public JsonResult SaveFile()
		{
			try
			{
				var httpRequest = Request.Form;
				var postedFile = httpRequest.Files[0];
				string filename = postedFile.FileName;
				var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

				using(var stream=new FileStream(physicalPath, FileMode.Create))
				{
					postedFile.CopyTo(stream);
				}
				return new JsonResult(filename);
			}
			catch (Exception)
			{
				return new JsonResult("nghia.png");
			}
		}
	}
}
