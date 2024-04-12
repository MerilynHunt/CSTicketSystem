using CustomerSupport.Models;
using Microsoft.Data.Sqlite;
using System.Data;

namespace CustomerSupport.DataAccess
{
	public class SqliteTicketDataAccess : ITicketDataAccess
	{
		public List<CustomerSupportItem> GetAllTickets()
		{
			List<CustomerSupportItem> ticketList = new();
			using (var con = new SqliteConnection("Data Source=db.sqlite"))
			{
				con.Open();
				using (var cmd = con.CreateCommand())
				{
					cmd.CommandText = "SELECT * FROM TICKETS ORDER BY DueDate ASC";
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							ticketList.Add(new CustomerSupportItem
							{
								Id = reader.GetInt32(0),
								Name = reader.GetString(1),
								Description = reader.GetString(2),
								CreationTime = reader.GetDateTime(3),
								DueDate = reader.GetDateTime(4),
							});
						}
					}
				}
			}
			return ticketList;
		}

		public bool InsertTicket(CustomerSupportItem ticket)
		{
			try
			{
				using (var con = new SqliteConnection("Data Source=db.sqlite"))
				{
					con.Open();
					using (var cmd = con.CreateCommand())
					{
						cmd.CommandText = "INSERT INTO TICKETS (Name, Description, DueDate) VALUES (@Name, @Description, @DueDate)";

						var nameParam = new SqliteParameter("@Name", DbType.String) { Value = ticket.Name };
						var descParam = new SqliteParameter("@Description", DbType.String) { Value = ticket.Description };
						var dueDateParam = new SqliteParameter("@DueDate", DbType.DateTime) { Value = ticket.DueDate };

						cmd.Parameters.Add(nameParam);
						cmd.Parameters.Add(descParam);
						cmd.Parameters.Add(dueDateParam);

						cmd.ExecuteNonQuery();
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}

		}

		public bool DeleteTicketById(int id)
		{
			try
			{
				using (var con = new SqliteConnection("Data Source=db.sqlite"))
				{
					con.Open();
					using (var cmd = con.CreateCommand())
					{
						cmd.CommandText = "DELETE FROM TICKETS WHERE Id = @Id";
						cmd.Parameters.Add(new SqliteParameter("@Id", DbType.Int32) { Value = id });
						cmd.ExecuteNonQuery();
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				// Log the exception
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public CustomerSupportItem GetTicketById(int id)
		{
			CustomerSupportItem ticket = new();

			using (var con = new SqliteConnection("Data Source=db.sqlite"))
			{
				con.Open();
				using (var cmd = con.CreateCommand())
				{
					cmd.CommandText = "SELECT * FROM TICKETS WHERE Id = @Id";
					cmd.Parameters.AddWithValue("@Id", id);

					using (var reader = cmd.ExecuteReader())
					{
						if (reader.HasRows)
						{
							reader.Read();
							ticket.Id = reader.GetInt32(0);
							ticket.Name = reader.GetString(1);
							ticket.Description = reader.GetString(2);
							ticket.CreationTime = reader.GetDateTime(3);
							ticket.DueDate = reader.GetDateTime(4);
						}
					}
				}
			}

			return ticket;
		}

		public bool UpdateTicket(CustomerSupportItem ticket)
		{
			try
			{
				using (var con = new SqliteConnection("Data Source=db.sqlite"))
				{
					con.Open();
					using (var cmd = con.CreateCommand())
					{
						cmd.CommandText = "UPDATE TICKETS SET Name = @Name, Description = @Description, DueDate = @DueDate WHERE Id = @Id";

						cmd.Parameters.AddWithValue("@Id", ticket.Id);
						cmd.Parameters.AddWithValue("@Name", ticket.Name);
						cmd.Parameters.AddWithValue("@Description", ticket.Description);
						cmd.Parameters.AddWithValue("@DueDate", ticket.DueDate);

						cmd.ExecuteNonQuery();
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}
	}
}
