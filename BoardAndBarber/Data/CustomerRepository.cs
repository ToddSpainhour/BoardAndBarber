using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardAndBarber.Models;
using Microsoft.Data.SqlClient;
 

namespace BoardAndBarber.Data
{
    public class CustomerRepository
    {

        // files named xyzRepository deal with storing data around xyz, usually crud methods


        // this list will be our pretend database
        // because the static keyword here I will get one copy of this list for every instance of the respository class
        static List<Customer> _customers = new List<Customer>();

        const string _connectionString = "Server = localHost; Database = BoardAndBarber; Trusted_Connection = True;";




        public void Add(Customer customerToAdd)
        {

            var sql = @"INSERT INTO [dbo].[Customers]
                            ([Name]
                            ,[Birthday]
                            ,[FavoriteBarber]
                            ,[Notes])
                        Output inserted.Id
                        VALUES
                            (@name,@birthday,@favoritebarber,@notes)";

            // name of parameter and value must match

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            // always use Parameters here to prevent sql injection
            cmd.Parameters.AddWithValue("name", customerToAdd.Name);
            cmd.Parameters.AddWithValue("birthday", customerToAdd.Birthday);
            cmd.Parameters.AddWithValue("favoriteBarber", customerToAdd.FavoriteBarber);
            cmd.Parameters.AddWithValue("notes", customerToAdd.Notes);


            var rows = (int) cmd.ExecuteScalar();
            
            if (rows != 1)
            {
                //something bad happened
            }

            // sql now does this for us
            ////get the next id by finding the max current id
            //var newId = 1;
            //if (_customers.Count > 0)
            //{
            //newId = _customers.Select(p => p.Id).Max() + 1;
            //}
            //customerToAdd.Id = newId;

            //_customers.Add(customerToAdd);
        }




        // we want to get everything and return them as a list of customers
        public List<Customer> GetAll()
        {

            using var connection = new SqlConnection(_connectionString);

            connection.Open();

            var command = connection.CreateCommand();

            var sql = "SELECT * FROM customers";

            command.CommandText = sql;

            var reader = command.ExecuteReader();
            var customers = new List<Customer>();

            while (reader.Read())
            {
                //take results from SQL and map them
                var customer = MapToCustomer(reader);
                customers.Add(customer);
            }
            return customers;
            //return _customers;
        }











        public Customer GetById(int id)
        {
            // 1: connect to your database // the using statement here uses a disposing statement which means when it exits the block it closes
            // 'using var' means when it's done, dispose of it
            using var connection = new SqlConnection("Server = localHost; Database = BoardAndBarber; Trusted_Connection = True;");
            

                // 2. Open the Conncetion
                connection.Open();

                // 3. Give SQL Server a command
                var command = connection.CreateCommand();
                var query = $@"select *
                        from Customers
                        WHERE id = {id}";

                // 4. set that command's command text
                command.CommandText = query;

                // 5.  make it go!

                //command.ExecuteNonQuery(); // run this query, I don't care about the results (like a delete) , returns int

                //command.ExecuteScalar();  // run this query and only return top left cell, returns object

                var reader = command.ExecuteReader(); // run this query and give me all the rows back one at a time, then you do something with that row, then move to the next row, returns SqlDataReader

                reader.Read(); // return boolean, true if there is a row to read
                               //sql server has excuted the command and is waiting to give us results

                if (reader.Read())
                {
                    


                return MapToCustomer(reader);

                }
                else
                {
                    connection.Close();
                    //no results? What do we do?
                    return null;
                }

                // only so many connections are possible so use a using statement at the top
                // return _customers.FirstOrDefault(c => c.Id == id);
        }





        public Customer Update (int id, Customer customer)
        {
            var customerToUpdate = GetById(id);

            customerToUpdate.Birthday = customer.Birthday;
            customerToUpdate.FavoriteBarber = customer.FavoriteBarber;
            customerToUpdate.Notes = customer.Notes;
            customerToUpdate.Name = customer.Name;

            return customerToUpdate;
        }





        public void Remove (int id)
        {
            var customerToDelete = GetById(id);

            _customers.Remove(customerToDelete);
        }


        private Customer MapToCustomer(SqlDataReader reader)
        {

            var customerFromDb = new Customer();

            customerFromDb.Id = (int)reader["id"]; // the (int) is an 'explicit cast' telling compiler it will be an int, throws exception if it's not the type identified
            customerFromDb.Notes = reader["Notes"] as string; // implicit cast
            customerFromDb.Birthday = DateTime.Parse(reader["Birthday"].ToString()); // parsing
            customerFromDb.FavoriteBarber = reader["FavoriteBarber"].ToString(); // what's inside the [] must be the same as the Db
            customerFromDb.Notes = reader["Notes"].ToString();

            return customerFromDb;
        }


    }
}
