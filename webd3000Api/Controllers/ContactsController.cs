using Azure.Storage.Queues;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace webd3000Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {


        private readonly ILogger<ContactsController> _logger;
        private readonly IConfiguration _configuration;


        public ContactsController(ILogger<ContactsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }



        [HttpGet]
        public IActionResult Get()
        {
            {
                return Ok("Hello from Contacts Controller - GET");

            }
        }


        [HttpPost]
        public async Task<IActionResult> PostAsync(Contact contact)
        {
            //if (string.IsNullOrEmpty(contact.FirstName))
            //{
            //    return BadRequest("First name is invalid.");
            //}

            //if (string.IsNullOrEmpty(contact.LastName))
            //{
            //    return BadRequest("last name is invalid.");
            //}

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            //post contact to queue

            string queueName = "contacts";
            // Get connection string from secrets.json
            string? connectionString = _configuration["AzureStorageConnectionString"];

            if (string.IsNullOrEmpty(connectionString))
            {
                return BadRequest("An error was encountered");
            }

            QueueClient queueClient = new QueueClient(connectionString, queueName);

            // serialize an object to json
            string message = JsonSerializer.Serialize(contact);

            // send string message to queue
            await queueClient.SendMessageAsync(message);




            return Ok("success - message posted to Storage Queue");
        }

    }
}
