using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DanhMucXeController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly IWebHostEnvironment _env;
		public DanhMucXeController(IConfiguration configuration, IWebHostEnvironment env)
		{
			_configuration = configuration;
			_env = env;
		}
		[HttpGet]
		public JsonResult Get()
		{
			string query = @"
                    SELECT ID, TenLoaiXe, MoTa
                    FROM dbo.DanhMucXe";
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
		public JsonResult Post(DanhMucXe bx)
		{
			string query = @"
                    INSERT INTO dbo.DanhMucXe
                    (TenLoaiXe, MoTa)
                    VALUES (@TenLoaiXe, @MoTa)";
			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("MyConnect");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@TenLoaiXe", bx.TenLoaiXe);
					myCommand.Parameters.AddWithValue("@MoTa", bx.MoTa);
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}
			return new JsonResult(table);
		}

		[HttpPut]
		public JsonResult Put(DanhMucXe bx)
		{
			string query = @"
                    UPDATE dbo.DanhMucXe
                    SET TenLoaiXe= @TenLoaiXe,
                        MoTa=@MoTa
                    WHERE ID=@ID";
			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("MyConnect");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@ID", bx.ID);
					myCommand.Parameters.AddWithValue("@TenLoaiXe", bx.TenLoaiXe);
					myCommand.Parameters.AddWithValue("@MoTa", bx.MoTa);
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
							delete from dbo.DanhMucXe
							where ID=@ID";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("MyConnect");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@ID", id);
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}
			return new JsonResult("Delete Successfully");
		}
	}
}
