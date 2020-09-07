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
    // a controller deals with requests coming in from the internet and responding to those requests
    // this class handles requests to store a customer
    // this controller tells other classes how to do their job, and handles the responses itself  


    [Route("api/customers")]
    // we changed to route above to be more clear 
    [ApiController]
    //whatever is to the left of the work 'Controller' that's what my route is
    public class CustomersController : ControllerBase
    {

        CustomerRepository _repo;





        public CustomersController()
        {
            _repo = new CustomerRepository();
        }





        [HttpPost]
        public IActionResult CreateCustomer(Customer customer)
        {
              _repo.Add(customer);

            return Created($"/api/customers/{customer.Id}", customer);
        }





        [HttpGet]
            public IActionResult GetAllCustomers()
        {
            var allCustomers = _repo.GetAll();

            return Ok(allCustomers);
        }





        [HttpPut("{id}")]
        public IActionResult UpdateCustomer (int id, Customer customer)
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
