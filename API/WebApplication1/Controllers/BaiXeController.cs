using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BaiXeController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly IWebHostEnvironment _env;
		public BaiXeController(IConfiguration configuration, IWebHostEnvironment env)
		{
			_configuration = configuration;
			_env = env;
		}
		[HttpGet]
		public JsonResult Get(string? viTri, int? id)
		{
			string query;
			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("MyConnect");
			SqlDataReader myReader;

			try
			{
				using (SqlConnection myCon = new SqlConnection(sqlDataSource))
				{
					myCon.Open();

					if (!string.IsNullOrEmpty(viTri))
					{
						// Tìm kiếm theo vị trí
						query = @"SELECT ID, DanhMucID, TenXe, ViTri, SoLuong, HinhAnh
                          FROM dbo.BaiXe
                          WHERE ViTri LIKE @ViTri";

						using (SqlCommand myCommand = new SqlCommand(query, myCon))
						{
							myCommand.Parameters.AddWithValue("@ViTri", viTri);
							myReader = myCommand.ExecuteReader();
							table.Load(myReader);
							myReader.Close();
						}
					}
					else if (id.HasValue)
					{
						// Tìm kiếm theo ID
						query = @"SELECT ID, DanhMucID, TenXe, ViTri, SoLuong, HinhAnh
                          FROM dbo.BaiXe
                          WHERE ID = @ID";

						using (SqlCommand myCommand = new SqlCommand(query, myCon))
						{
							myCommand.Parameters.AddWithValue("@ID", id.Value);
							myReader = myCommand.ExecuteReader();
							table.Load(myReader);
							myReader.Close();
						}
					}
					else
					{
						// Nếu không có giá trị ID, lấy tất cả danh sách
						query = @"SELECT ID, DanhMucID, TenXe, ViTri, SoLuong, HinhAnh
                          FROM dbo.BaiXe";

						using (SqlCommand myCommand = new SqlCommand(query, myCon))
						{
							myReader = myCommand.ExecuteReader();
							table.Load(myReader);
							myReader.Close();
						}
					}

					myCon.Close();
				}

				return new JsonResult(table);
			}
			catch (Exception ex)
			{
				return new JsonResult(new { Error = ex.Message });
			}
		}

		[HttpPost]
		public JsonResult Post(BaiXe bx)
		{
			string query = @"
                    INSERT INTO dbo.BaiXe
                    (DanhMucID, TenXe, ViTri, SoLuong, HinhAnh)
                    VALUES (@DanhMucID, @TenXe, @ViTri, @SoLuong, @HinhAnh)";
			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("MyConnect");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@DanhMucID", bx.DanhMucID);
					myCommand.Parameters.AddWithValue("@TenXe", bx.TenXe);
					myCommand.Parameters.AddWithValue("@ViTri", bx.ViTri);
					myCommand.Parameters.AddWithValue("@SoLuong", bx.SoLuong);
					myCommand.Parameters.AddWithValue("@HinhAnh", bx.HinhAnh);
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}
			return new JsonResult(table);
		}

		[HttpPut("{id}")]
		public JsonResult Put(int id, BaiXe bx)
		{
			string query = @"
        UPDATE dbo.BaiXe
        SET DanhMucID = @DanhMucID,
            TenXe = @TenXe,
            ViTri = @ViTri,
            SoLuong = @SoLuong,
            HinhAnh = @HinhAnh
        WHERE ID = @ID";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("MyConnect");
			SqlDataReader myReader;

			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@ID", id);
					myCommand.Parameters.AddWithValue("@DanhMucID", bx.DanhMucID);
					myCommand.Parameters.AddWithValue("@TenXe", bx.TenXe);
					myCommand.Parameters.AddWithValue("@ViTri", bx.ViTri);
					myCommand.Parameters.AddWithValue("@SoLuong", bx.SoLuong);
					myCommand.Parameters.AddWithValue("@HinhAnh", bx.HinhAnh);

					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
				}
				myCon.Close();
			}

			return new JsonResult("Updated Successfully");
		}

		[HttpPut]
		public JsonResult Put(BaiXe bx)
		{
			string query = @"
                    UPDATE dbo.BaiXe
                    SET DanhMucID= @DanhMucID,
                        TenXe=@TenXe,
                        ViTri=@ViTri,
                        SoLuong=@SoLuong,
						HinhAnh=@HinhAnh
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
					myCommand.Parameters.AddWithValue("@DanhMucID", bx.DanhMucID);
					myCommand.Parameters.AddWithValue("@TenXe", bx.TenXe);
					myCommand.Parameters.AddWithValue("@ViTri", bx.ViTri);
					myCommand.Parameters.AddWithValue("@SoLuong", bx.SoLuong);
					myCommand.Parameters.AddWithValue("@HinhAnh", bx.HinhAnh);
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
							delete from dbo.BaiXe
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

		[HttpPost("SaveFile")]
		public JsonResult SaveFile(IFormFile file)
		{
			try
			{
				var httpRequest = Request.Form;
				var postedFile = file;
				string filename = file.FileName;
				var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

				using (var stream = new FileStream(physicalPath, FileMode.Create))
				{
					postedFile.CopyTo(stream);
				}
				string imageUrl = "https://localhost:7117/Photos/" + filename;
				return new JsonResult(imageUrl);
			}
			catch (Exception)
			{
				return new JsonResult("nghia.png");
			}
		}
	}
}
