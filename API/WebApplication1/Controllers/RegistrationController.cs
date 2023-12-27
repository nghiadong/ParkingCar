using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RegistrationController : ControllerBase
	{
		private readonly IConfiguration _configuration;

		public RegistrationController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpPost]
		[Route("registration")]
		public int Registration(Registration registration)
		{
			using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyConnect")))
			{
				con.Open();
				using (SqlCommand cmd = new SqlCommand("INSERT INTO Registration(Username, Password, Email) VALUES(@Username, @Password, @Email )", con))
				{
					cmd.Parameters.AddWithValue("@Username", registration.Username);
					cmd.Parameters.AddWithValue("@Password", registration.Password);
					cmd.Parameters.AddWithValue("@Email", registration.Email);

					int rowsAffected = cmd.ExecuteNonQuery();
					if (rowsAffected > 0)
					{
						return 1;
					}
					else
					{
						return 0;
					}
				}
			}
		}

		[HttpPost]
		[Route("login")]
		public int Login(string acc, string pass)
		{
			using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyConnect")))
			{
				con.Open();
				using (SqlCommand cmd = new SqlCommand("SELECT * FROM Registration WHERE Username = @acc AND Password = @pass", con))
				{
					cmd.Parameters.AddWithValue("@acc", acc);
					cmd.Parameters.AddWithValue("@pass", pass);

					using (SqlDataAdapter da = new SqlDataAdapter(cmd))
					{
						DataTable dt = new DataTable();
						da.Fill(dt);
						if (dt.Rows.Count > 0)
						{
							return 1;
						}
						else
						{
							return 0;
						}
					}
				}
			}
		}
	}
}
