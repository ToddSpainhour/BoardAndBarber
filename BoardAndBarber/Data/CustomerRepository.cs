using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardAndBarber.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
 

namespace BoardAndBarber.Data
{
    public class CustomerRepository
    {

        // files named xyzRepository deal with storing data around xyz, usually crud methods


        // this list will be our pretend database
        // because the static keyword here I will get one copy of this list for every instance of the respository class
        readonly string _connectionString;

        public CustomerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("BoardAndBarber");
        }




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

            using var db = new SqlConnection(_connectionString);

            var newId = db.ExecuteScalar<int>(sql, customerToAdd);

            customerToAdd.Id = newId;
        }



        // we want to get everything and return them as a list of customers
        public List<Customer> GetAll()
        {

            using var db = new SqlConnection(_connectionString);

            try
            {
                var customers = db.Query<Customer>("SELECT * FROM Customers");

                return customers.ToList();
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
                //throw;
            }
        }



        public Customer GetById(int customerId)
        {
            using var db = new SqlConnection(_connectionString);

            var query = @"SELECT *
                          FROM Customers
                          WHERE id = @cid";

            var parameters = new { cid = customerId };

            var customer = db.QueryFirstOrDefault<Customer>(query, parameters);

            return customer;
        }



        public Customer Update(int id, Customer customer)
        {
            var sql = @"UPDATE [dbo].[Customers]
                          SET [Name] = @name
                             ,[Birthday] = @birthday
                             ,[FavoriteBarber] = @favoriteBarber
                             ,[Notes] = @notes
                        output inserted.*
                        WHERE id = @id";

            using var db = new SqlConnection(_connectionString);

            // if property names should match the name of the property or variable on the right, you don't have to specify.  
            // you can leave off the property name if the name on the right and the property name match.
            var parameters = new
            {
                Name = customer.Name,
                Birthday = customer.Birthday,
                FavoriteBarber = customer.FavoriteBarber,
                Notes = customer.Notes,
                id = id
            };

            var updatedCustomer = db.QueryFirstOrDefault<Customer>(sql, parameters);

            return updatedCustomer;
        }



        public void Remove (int customerId)
        {
            var sql = @"DELETE 
                        FROM [dbo].[Customers]
                        WHERE Id = @id";

            using var db = new SqlConnection(_connectionString);

            db.Execute(sql, new { id = customerId });
        }


    }
}
