using System.Data.SqlClient;
using System.Data;
using Hotel_Management_System.Pages.Models;
using Azure.Core;
using System.Text.RegularExpressions;
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

            try
            {
                // Check if the first name and last name are letters
                if (!Regex.IsMatch(new_guest.first_name, @"^[a-zA-Z]+$") || !Regex.IsMatch(new_guest.last_name, @"^[a-zA-Z]+$"))
                {
                    throw new ArgumentException("First name and last name must be letters only.");
                }

                // Check if the country code contains numbers
                if (Regex.IsMatch(new_guest.country_code, @"\d"))
                {
                    throw new ArgumentException("Country code must not contain numbers.");
                }

                // Check if the email contains the @ sign
                if (!new_guest.email.Contains("@"))
                {
                    throw new ArgumentException("Email must contain the @ sign.");
                }

                // Check if the SSN is a number and does not contain letters
                if (!Regex.IsMatch(new_guest.ssn.ToString(), @"^\d+$") || Regex.IsMatch(new_guest.ssn.ToString(), @"[a-zA-Z]"))
                {
                    throw new ArgumentException("SSN must be a number and must not contain letters.");
                }

                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                con.Open();

                SqlCommand max_id_cmd = new SqlCommand("SELECT MAX (GuestID) + 1 FROM Guest", con);
                int max_id = Convert.ToInt32(max_id_cmd.ExecuteScalar());

                new_guest.guest_id = max_id;

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
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public Guest GetGuestById(int id)
        {
            
            Guest guest = null;
            
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-DFJM3O9;Initial Catalog=Hotel;Integrated Security=True;"))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Guest WHERE GuestID = @id", con))
                {
                    
                    cmd.Parameters.AddWithValue("@id", id);
                    
                    con.Open();
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
                    con.Close();
                }
            }
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

            
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            
            con.Open();
            
            SqlCommand cmd = new SqlCommand("UPDATE Guest SET FirstName = @firstName, LastName = @lastName, City_code = @cityCode, Country_code = @countryCode, Street_number = @streetNumber, Email = @email, SSN = @ssn WHERE GuestID = @id", con);
            
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@firstName", firstName);
            cmd.Parameters.AddWithValue("@lastName", lastName);
            cmd.Parameters.AddWithValue("@cityCode", cityCode);
            cmd.Parameters.AddWithValue("@countryCode", countryCode);
            cmd.Parameters.AddWithValue("@streetNumber", streetNumber);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@ssn", ssn);
            
            cmd.ExecuteNonQuery();
            
            con.Close();
        }

        public void AddReservation(Reservation new_reservation, string room_type, string has_ac)
        {
            try
            {
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

                if (new_reservation.num_guests <= 0)
                {
                    throw new ArgumentException("The number of guests must be a positive integer.");
                }

                SqlCommand find_room_cmd = new SqlCommand("SELECT TOP 1 r.RoomNumber FROM Room r JOIN RoomType rt ON r.Type = rt.RoomTypeName WHERE r.Status = 'Vacant' AND rt.RoomTypeName = @room_type AND rt.HasAC = @has_ac ORDER BY r.Construction DESC", con);
                find_room_cmd.Parameters.AddWithValue("@room_type", room_type);
                find_room_cmd.Parameters.AddWithValue("@has_ac", has_ac);


                object result = find_room_cmd.ExecuteScalar();
                Console.WriteLine(result);

                if (result != null)
                {
                    int room_number = Convert.ToInt32(result);

                    SqlCommand cmnd = new SqlCommand("Insert Into Reservation (ReservationID, is_cancelled, " +
                        "guests_arrived, CheckINDate, NumGuests) Values (@ReservationID, @is_cancelled," +
                        " @guests_arrived, @CheckINDate, @NumGuests)", con);

                    cmnd.Parameters.AddWithValue("@ReservationID", new_reservation.reservation_id);
                    cmnd.Parameters.AddWithValue("@is_cancelled", new_reservation.is_cancelled);
                    cmnd.Parameters.AddWithValue("@guests_arrived", new_reservation.guest_arrived);
                    cmnd.Parameters.AddWithValue("@CheckINDate", new_reservation.check_in_date);
                    cmnd.Parameters.AddWithValue("@NumGuests", new_reservation.num_guests);

                    cmnd.ExecuteNonQuery();

                    // Insert into the Room_books table
                    SqlCommand book_room_cmd = new SqlCommand("Insert Into Room_books (RoomNumber, ReservationID) Values (@RoomNumber, @ReservationID)", con);
                    book_room_cmd.Parameters.AddWithValue("@RoomNumber", room_number);
                    book_room_cmd.Parameters.AddWithValue("@ReservationID", new_reservation.reservation_id);

                    book_room_cmd.ExecuteNonQuery();

                    // Update the status of the room in the Room table
                    SqlCommand update_room_cmd = new SqlCommand("Update Room SET Status = 'Occupied' WHERE RoomNumber = @RoomNumber", con);
                    update_room_cmd.Parameters.AddWithValue("@RoomNumber", room_number);

                    update_room_cmd.ExecuteNonQuery();

                    Console.WriteLine("Your reservation has been confirmed. Your room number is {0}.", room_number);
                }
                else
                {
                    Console.WriteLine("Sorry, there is no room that matches your preferences for now. Check in later!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: {0}", e.Message);
            }
            finally
            {
                con.Close();
            }
        }


        public void AddRating(Rating new_rating)
        {
            // Get the next available ID for the new rating
            string getIdQuery = "SELECT MAX(RatingID) + 1 FROM Rating";
            int nextId = 0;
            try
            {
                // Open the connection
                con.Open();
                SqlCommand getIdCmd = new SqlCommand(getIdQuery, con);
                object result = getIdCmd.ExecuteScalar();
                nextId = result == DBNull.Value ? 1 : Convert.ToInt32(result);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error getting the next ID: " + ex.Message);
            }
            finally
            {
                con.Close();
            }

            string insertQuery = "INSERT INTO Rating (RatingID, Feedback, Date, RatingScore) VALUES (@RatingID, @Feedback, @Date, @RatingScore)";
            try
            {
                con.Open();
                SqlCommand insertCmd = new SqlCommand(insertQuery, con);
                
                insertCmd.Parameters.AddWithValue("@RatingID", nextId);
                insertCmd.Parameters.AddWithValue("@Feedback", new_rating.feedback);
                insertCmd.Parameters.AddWithValue("@Date", new_rating.date);
                insertCmd.Parameters.AddWithValue("@RatingScore", new_rating.rating_score);
                insertCmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error inserting the new rating: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

    }
}
