using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DatXeController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly IWebHostEnvironment _env;
		public DatXeController(IConfiguration configuration, IWebHostEnvironment env)
		{
			_configuration = configuration;
			_env = env;
		}
		[HttpGet]
		public JsonResult Get(string? trangThai)
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

					if (!string.IsNullOrEmpty(trangThai))
					{
						// Nếu có giá trị ID, thực hiện tìm kiếm theo ID
						query = @"SELECT ID, BaiXeID, NgayDat, BienSo, DanhMucXeID, TrangThai
                          FROM dbo.DatXe
                          WHERE TrangThai LIKE @TrangThai";

						using (SqlCommand myCommand = new SqlCommand(query, myCon))
						{
							myCommand.Parameters.AddWithValue("@TrangThai", trangThai);
							myReader = myCommand.ExecuteReader();
							table.Load(myReader);
							myReader.Close();
						}
					}
					else
					{
						// Nếu không có giá trị ID, lấy tất cả danh sách
						query = @"SELECT ID, BaiXeID, NgayDat, BienSo, DanhMucXeID, TrangThai
                          FROM dbo.DatXe";

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
		[HttpGet("statistics")]
		public JsonResult GetMonthlyStatistics()
		{
			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("MyConnect");

			try
			{
				using (SqlConnection myCon = new SqlConnection(sqlDataSource))
				{
					myCon.Open();

					string query = @"SELECT MONTH(NgayDat) AS Thang, COUNT(*) AS SoLuongDatXe
                             FROM dbo.DatXe
                             GROUP BY MONTH(NgayDat)";

					using (SqlCommand myCommand = new SqlCommand(query, myCon))
					{
						SqlDataReader myReader = myCommand.ExecuteReader();
						table.Load(myReader);
						myReader.Close();
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
		[HttpGet("trangthai")]
		public JsonResult GetStatistics()
		{
			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("MyConnect");

			try
			{
				using (SqlConnection myCon = new SqlConnection(sqlDataSource))
				{
					myCon.Open();

					string query = @"SELECT TrangThai, COUNT(*) AS SoLuong
                             FROM dbo.DatXe
                             GROUP BY TrangThai";

					using (SqlCommand myCommand = new SqlCommand(query, myCon))
					{
						SqlDataReader myReader = myCommand.ExecuteReader();
						table.Load(myReader);
						myReader.Close();
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
		public JsonResult Post(DatXe bx)
		{
			string insertQuery = @"
        INSERT INTO dbo.DatXe
        (BaiXeID, NgayDat, BienSo, DanhMucXeID, TrangThai)
        VALUES (@BaiXeID, @NgayDat, @BienSo, @DanhMucXeID, @TrangThai);
        SELECT CAST(SCOPE_IDENTITY() AS INT)";

			string sqlDataSource = _configuration.GetConnectionString("MyConnect");
			int datXeId;

			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand insertCommand = new SqlCommand(insertQuery, myCon))
				{
					insertCommand.Parameters.AddWithValue("@BaiXeID", bx.BaiXeID);
					insertCommand.Parameters.AddWithValue("@NgayDat", bx.NgayDat);
					insertCommand.Parameters.AddWithValue("@BienSo", bx.BienSo);
					insertCommand.Parameters.AddWithValue("@DanhMucXeID", bx.DanhMucXeID);
					insertCommand.Parameters.AddWithValue("@TrangThai", bx.TrangThai);

					// Thực hiện lệnh insert và lấy ID của DatXe mới thêm vào
					datXeId = (int)insertCommand.ExecuteScalar();
				}

				// Cập nhật SoLuong trong BaiXe bằng cách trừ đi 1
				UpdateSoLuongInBaiXe(bx.BaiXeID);

				myCon.Close();
			}

			// Trả về ID của DatXe mới thêm vào
			return new JsonResult(datXeId);
		}

		private void UpdateSoLuongInBaiXe(int baiXeId)
		{
			string updateQuery = @"
        UPDATE dbo.BaiXe
        SET SoLuong = SoLuong - 1
        WHERE ID = @BaiXeID";

			string sqlDataSource = _configuration.GetConnectionString("MyConnect");

			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand updateCommand = new SqlCommand(updateQuery, myCon))
				{
					updateCommand.Parameters.AddWithValue("@BaiXeID", baiXeId);
					updateCommand.ExecuteNonQuery();
				}
				myCon.Close();
			}
		}

		[HttpPut("{id}")]
		public JsonResult Put(int id, DatXe bx)
		{
			string query = @"
        UPDATE dbo.DatXe
        SET BaiXeID = @BaiXeID,
            NgayDat = @NgayDat,
            BienSo = @BienSo,
            DanhMucXeID = @DanhMucXeID,
            TrangThai = @TrangThai
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
					myCommand.Parameters.AddWithValue("@BaiXeID", bx.BaiXeID);
					myCommand.Parameters.AddWithValue("@NgayDat", bx.NgayDat);
					myCommand.Parameters.AddWithValue("@BienSo", bx.BienSo);
					myCommand.Parameters.AddWithValue("@DanhMucXeID", bx.DanhMucXeID);
					myCommand.Parameters.AddWithValue("@TrangThai", bx.TrangThai);

					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
				}
				myCon.Close();
			}

			return new JsonResult("Updated Successfully");
		}

		[HttpPut]
		public JsonResult Put(DatXe bx)
		{
			string query = @"
                    UPDATE dbo.DatXe
                    SET BaiXeID= @BaiXeID,
                        NgayDat=@NgayDat,
                        BienSo=@BienSo,
						DanhMucXeID=@DanhMucXeID,
						TrangThai=@TrangThai
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
					myCommand.Parameters.AddWithValue("@BaiXeID", bx.BaiXeID);
					myCommand.Parameters.AddWithValue("@NgayDat", bx.NgayDat);
					myCommand.Parameters.AddWithValue("@BienSo", bx.BienSo);
					myCommand.Parameters.AddWithValue("@DanhMucXeID", bx.DanhMucXeID);
					myCommand.Parameters.AddWithValue("@TrangThai", bx.TrangThai);
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
			// Lấy BaiXeID của DatXe cần xóa
			int baiXeId = GetBaiXeIdFromDatXe(id);

			// Thực hiện xóa DatXe
			DeleteDatXe(id);

			// Cộng thêm 1 vào SoLuong trong BaiXe
			UpdateSoLuongInBaiXe(baiXeId, 1);

			return new JsonResult("Delete Successfully");
		}

		private int GetBaiXeIdFromDatXe(int datXeId)
		{
			string query = @"SELECT BaiXeID FROM dbo.DatXe WHERE ID = @ID";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("MyConnect");
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@ID", datXeId);
					SqlDataReader myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
				}
				myCon.Close();
			}

			if (table.Rows.Count > 0)
			{
				return Convert.ToInt32(table.Rows[0]["BaiXeID"]);
			}

			return -1; // Hoặc một giá trị không hợp lệ khác nếu không tìm thấy
		}

		private void DeleteDatXe(int datXeId)
		{
			string query = @"DELETE FROM dbo.DatXe WHERE ID = @ID";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("MyConnect");
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@ID", datXeId);
					SqlDataReader myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
				}
				myCon.Close();
			}
		}

		private void UpdateSoLuongInBaiXe(int baiXeId, int valueToAdd)
		{
			string updateQuery = @"
        UPDATE dbo.BaiXe
        SET SoLuong = SoLuong + @ValueToAdd
        WHERE ID = @BaiXeID";

			string sqlDataSource = _configuration.GetConnectionString("MyConnect");

			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand updateCommand = new SqlCommand(updateQuery, myCon))
				{
					updateCommand.Parameters.AddWithValue("@BaiXeID", baiXeId);
					updateCommand.Parameters.AddWithValue("@ValueToAdd", valueToAdd);
					updateCommand.ExecuteNonQuery();
				}
				myCon.Close();
			}
		}
	}
}
