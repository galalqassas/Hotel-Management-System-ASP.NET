using System.Data.SqlClient;
using System.Data;
using Hotel_Management_System.Pages.Models;
using Azure.Core;
namespace Hotel_Management_System.Pages
{
    public class DB
    {
        public SqlConnection con { get; set; }
        public List<Guest> guests { get; set; } = new List<Guest>();

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
        public Guest GetGuestById(int id)
        {
            // create a Guest object to store the result
            Guest guest = null;
            // create a connection object using the connection string
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-DFJM3O9;Initial Catalog=Hotel;Integrated Security=True;"))
            {
                // create a command object using the connection and the query
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Guest WHERE GuestID = @id", con))
                {
                    // add a parameter to the command with the id value
                    cmd.Parameters.AddWithValue("@id", id);
                    // open the connection
                    con.Open();
                    // execute the command and get a data reader
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // if there is a row in the result
                        if (reader.Read())
                        {
                            // create a new Guest object with the values from the reader
                            guest = new Guest();
                            guest.guest_id = (int)reader["GuestID"];
                            guest.first_name = (string)reader["FirstName"];
                            guest.last_name = (string)reader["LastName"];
                            guest.city_code = (string)reader["City_code"];
                            guest.country_code = (string)reader["Country_code"];
                            guest.street_number = (string)reader["Street_number"];
                            guest.email = (string)reader["Email"];
                            guest.not_showingup = (int)reader["notshowingup"];
                            guest.ssn = (int)reader["SSN"];
                        }
                    }
                    // close the connection
                    con.Close();
                }
            }
            // return the guest object or null if not found
            return guest;
        }
        public void UpdateGuestInfo(Guest updated_guest)
        {
            // get the values from the form
            string firstName = updated_guest.first_name;
            string lastName = updated_guest.last_name;
            string cityCode = updated_guest.city_code;
            string countryCode = updated_guest.country_code;
            string streetNumber = updated_guest.street_number;
            string email = updated_guest.email;
            int ssn = updated_guest.ssn;
            int id = updated_guest.guest_id;

            // check the connection state and close it if open
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            // open the connection
            con.Open();
            // create a command object using the connection and the query
            SqlCommand cmd = new SqlCommand("UPDATE Guest SET FirstName = @firstName, LastName = @lastName, City_code = @cityCode, Country_code = @countryCode, Street_number = @streetNumber, Email = @email, SSN = @ssn WHERE GuestID = @id", con);
            // add parameters to the command with the values from the form
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@firstName", firstName);
            cmd.Parameters.AddWithValue("@lastName", lastName);
            cmd.Parameters.AddWithValue("@cityCode", cityCode);
            cmd.Parameters.AddWithValue("@countryCode", countryCode);
            cmd.Parameters.AddWithValue("@streetNumber", streetNumber);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@ssn", ssn);
            // execute the command
            cmd.ExecuteNonQuery();
            // close the connection
            con.Close();
        }

        public void AddReservation(Reservation new_reservation, string room_type, string has_ac)
        {
            // Check the connection state and close it if it is already open
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            con.Open();

            SqlCommand max_id_cmd = new SqlCommand("SELECT MAX (ReservationID) + 1 FROM Reservation", con);
            int max_id = Convert.ToInt32(max_id_cmd.ExecuteScalar());

            new_reservation.reservation_id = max_id;
            new_reservation.is_cancelled = "No";
            new_reservation.guest_arrived = 0;

            // Query the database for the first room that matches the guest preferences
            SqlCommand find_room_cmd = new SqlCommand("SELECT TOP 1 r.RoomNumber FROM Room r JOIN RoomType rt ON r.Type = rt.RoomTypeName WHERE r.Status = 'Vacant' AND rt.RoomTypeName = @room_type AND rt.HasAC = @has_ac ORDER BY r.Construction DESC", con);
            find_room_cmd.Parameters.AddWithValue("@room_type", room_type);
            find_room_cmd.Parameters.AddWithValue("@has_ac", has_ac);

            // Execute the query and get the result
            object result = find_room_cmd.ExecuteScalar();
            Console.WriteLine(result);
            // Check if there is any matching room
            if (result != null)
            {
                // Get the room number
                int room_number = Convert.ToInt32(result);

                // Insert the reservation into the Reservation table
                SqlCommand cmnd = new SqlCommand("Insert Into Reservation (ReservationID, is_cancelled, " +
                    "guests_arrived, CheckINDate, NumGuests) Values (@ReservationID, @is_cancelled," +
                    " @guests_arrived, @CheckINDate, @NumGuests)", con);

                cmnd.Parameters.AddWithValue("@ReservationID", new_reservation.reservation_id);
                cmnd.Parameters.AddWithValue("@is_cancelled", new_reservation.is_cancelled);
                cmnd.Parameters.AddWithValue("@guests_arrived", new_reservation.guest_arrived);
                cmnd.Parameters.AddWithValue("@CheckINDate", new_reservation.check_in_date);
                cmnd.Parameters.AddWithValue("@NumGuests", new_reservation.num_guests);

                cmnd.ExecuteNonQuery();

                // Insert the room booking into the Room_books table
                SqlCommand book_room_cmd = new SqlCommand("Insert Into Room_books (RoomNumber, ReservationID) Values (@RoomNumber, @ReservationID)", con);
                book_room_cmd.Parameters.AddWithValue("@RoomNumber", room_number);
                book_room_cmd.Parameters.AddWithValue("@ReservationID", new_reservation.reservation_id);

                book_room_cmd.ExecuteNonQuery();

                // Update the status of the room in the Room table
                SqlCommand update_room_cmd = new SqlCommand("Update Room SET Status = 'Occupied' WHERE RoomNumber = @RoomNumber", con);
                update_room_cmd.Parameters.AddWithValue("@RoomNumber", room_number);

                update_room_cmd.ExecuteNonQuery();

                // Display a message to confirm the reservation
                Console.WriteLine("Your reservation has been confirmed. Your room number is {0}.", room_number);
            }
            else
            {
                // Display a message to inform the guest that there is no matching room
                Console.WriteLine("Sorry, there is no room that matches your preferences.");
            }

            con.Close();
        }

    }
}
