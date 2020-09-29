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





        public void Add(Customer customerToAdd)
        {
            //get the next id by finding the max current id
            var newId = 1;
            if (_customers.Count > 0)
            {
            newId = _customers.Select(p => p.Id).Max() + 1;
            }
            customerToAdd.Id = newId;

            _customers.Add(customerToAdd);
        }





        public List<Customer> GetAll()
        {
            return _customers;
        }





        public Customer GetById(int id)
        {
            // 1: connect to your database
            var connection = new SqlConnection("Server = localHost; Database = BoardAndBarber; Trusted_Connection = True;");

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

            var reader = command.ExecuteReader(); // run this query and give me results on at a time, then you do something with that row, then move to the next row, returns SqlDataReader

            reader.Read(); // return true if there is a row to read
            //sql server has excuted the command and is waiting to give us results

            if (reader.Read())
            {
                var customerFromDb = new Customer();


                //do something with that one result
                customerFromDb.Id = (int)reader["id"]; // the (int) is an 'explicit cast' telling compiler it will be an int, throws exception if it's not the type identified
                customerFromDb.Notes = reader["Notes"] as string; // implicit cast
                customerFromDb.Birthday = DateTime.Parse(reader["Birthday"].ToString()); // parsing
                customerFromDb.FavoriteBarber = reader["FavoriteBaber"].ToString(); // what's inside the [] must be the same as the Db
                customerFromDb.Notes = reader["Notes"].ToString();

                return customerFromDb;
            }
            else
            {
                //no results? What do we do?
                return null;
            }


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





    }
}
