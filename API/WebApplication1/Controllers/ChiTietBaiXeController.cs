using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ChiTietBaiXeController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly IWebHostEnvironment _env;
		public ChiTietBaiXeController(IConfiguration configuration, IWebHostEnvironment env)
		{
			_configuration = configuration;
			_env = env;
		}
		[HttpGet]
		public JsonResult Get(int id)
		{
			string query = @"
                    SELECT ID, DatXeID, BaiXeID,
                    MoTa
                    FROM dbo.ChiTietBaiXe where BaiXeID = " + id;
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
		public JsonResult Post(ChiTietBaiXe bx)
		{
			string query = @"
                    INSERT INTO dbo.ChiTietBaiXe
                    (DatXeID, BaiXeID, MoTa)
                    VALUES (@DatXeID, @BaiXeID, @MoTa)";
			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("MyConnect");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@DatXeID", bx.DatXeID);
					myCommand.Parameters.AddWithValue("@BaiXeID", bx.BaiXeID);
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
		public JsonResult Put(ChiTietBaiXe bx)
		{
			string query = @"
                    UPDATE dbo.ChiTietBaiXe
                    SET DatXeID= @DatXeID,
                        BaiXeID=@BaiXeID,
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
					myCommand.Parameters.AddWithValue("@DatXeID", bx.DatXeID);
					myCommand.Parameters.AddWithValue("@BaiXeID", bx.BaiXeID);
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
							delete from dbo.ChiTietBaiXe
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
