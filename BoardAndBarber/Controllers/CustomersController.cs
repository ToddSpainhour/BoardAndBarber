using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BoardAndBarber.Models;
using BoardAndBarber.Data;

namespace BoardAndBarber.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {

        CustomerRepository _repo;

        [HttpPost]
        public IActionResult CreateCustomer(Customer customer)
        {
            var repo = new CustomerRepository();
                repo.Add(customer);

            return Created($"/api/customers/{customer.Id}", customer);
        }



        [HttpGet]
            public IActionResult GetAllCustomers()
        {
            var repo = new CustomerRepository();
            var allCustomers = repo.GetAll();

            return Ok(allCustomers);
        }



        [HttpPut("{id}")]
        public IActionResult updateCustomer (int id, Customer customer)
        {
            var updatedCustomer = _repo.Update(id, customer);

            return Ok(updatedCustomer);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            if (_repo.GetById(id) == null)
            {
                return NotFound();
            }

            _repo.Remove(id);

            return Ok();

        }




    }
}
