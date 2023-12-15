using System.Data.SqlClient;
using System.Data;
using Hotel_Management_System.Pages.Models;
namespace Hotel_Management_System.Pages
{
    public class DB
    {
        public SqlConnection con { get; set; }
        public List<Guest> guests { get; set; } = new List<Guest>();

        int i = 60;
        public DB()
        {
            string conStr = "Data Source=DESKTOP-DFJM3O9;Initial Catalog=Hotel;Integrated Security=True;";
            con = new SqlConnection(conStr);
        }
        // Read a specific table
        public DataTable ReadTable(string tableName)
        {
            DataTable table = new DataTable();
            string q = "SELECT * FROM " + tableName;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(q, con);
                table.Load(cmd.ExecuteReader());
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return table;
        }

        public void AddGuest(Guest new_guest)
        {
            // Set the notshowingup to be 0 by default
            new_guest.not_showingup = 0;

            // Check the connection state and close it if it is already open
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            // Open the connection
            con.Open();

            // Get the maximum id from the Guest table and add 1 to it
            SqlCommand max_id_cmd = new SqlCommand("SELECT MAX (GuestID) + 1 FROM Guest", con);
            int max_id = Convert.ToInt32(max_id_cmd.ExecuteScalar());

            
            new_guest.guest_id = max_id;

            // insert the guest object into the database
            SqlCommand cmnd = new SqlCommand("Insert Into Guest (GuestID, FirstName, LastName, City_code, Country_code, Street_number, Email, notshowingup, SSN) Values (@GuestId, @FirstName, @LastName, @City_code, @Country_code, @Street_number, @Email, @notshowingup, @SSN)", con);

            cmnd.Parameters.AddWithValue("@GuestId", new_guest.guest_id);
            cmnd.Parameters.AddWithValue("@FirstName", new_guest.first_name);
            cmnd.Parameters.AddWithValue("@LastName", new_guest.last_name);
            cmnd.Parameters.AddWithValue("@City_code", new_guest.city_code);
            cmnd.Parameters.AddWithValue("@Country_code", new_guest.country_code);
            cmnd.Parameters.AddWithValue("@Street_number", new_guest.street_number);
            cmnd.Parameters.AddWithValue("@Email", new_guest.email);
            cmnd.Parameters.AddWithValue("@notshowingup", new_guest.not_showingup);
            cmnd.Parameters.AddWithValue("@SSN", new_guest.ssn);

            cmnd.ExecuteNonQuery();
            con.Close();
        }

        public void AddReservation(Reservation new_reservation)
        {
            // Open the connection
            con.Open();

            // Get the maximum id from the Guest table and add 1 to it
            SqlCommand max_id_cmd = new SqlCommand("SELECT MAX (ReservationID) + 1 FROM Reservation", con);
            int max_id = Convert.ToInt32(max_id_cmd.ExecuteScalar());

            new_reservation.reservation_id = max_id;
            new_reservation.is_cancelled = "No";
            new_reservation.guest_arrived = 0;

            // insert into the Reservation table
            SqlCommand cmnd = new SqlCommand("Insert Into Reservation (ReservationID, is_cancelled, " +
                "guests_arrived, CheckINDate, NumGuests) Values (@ReservationID, @is_cancelled," +
                " @guests_arrived, @CheckINDate, @NumGuests)", con);

            cmnd.Parameters.AddWithValue("@ReservationID", new_reservation.reservation_id);
            cmnd.Parameters.AddWithValue("@is_cancelled", new_reservation.is_cancelled);
            cmnd.Parameters.AddWithValue("@guests_arrived", new_reservation.guest_arrived);
            cmnd.Parameters.AddWithValue("@CheckINDate", new_reservation.check_in_date);
            cmnd.Parameters.AddWithValue("@NumGuests", new_reservation.num_guests);

            cmnd.ExecuteNonQuery();
            con.Close();
        }
    }
}
