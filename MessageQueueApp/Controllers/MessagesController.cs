﻿using MessageQueueApp.IO;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

namespace MessageQueueApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : Controller
    {
        [HttpPost]
        public async Task<ActionResult<PostMessageResponse>> Post([FromBody] PostMessageRequest request)
        {
            if (string.IsNullOrEmpty(request.Content))
                return BadRequest();

            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };
            var connection = await factory.CreateConnectionAsync();

            using (var channel = await connection.CreateChannelAsync())
            {
                var json = System.Text.Json.JsonSerializer.Serialize(request.Content);
                var body = Encoding.UTF8.GetBytes(json);
                
                await channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: "messages",
                    body);
            }

            var response = new PostMessageResponse(DateTime.Now);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            return Ok(id);
        }

        [HttpGet]
        public async Task<ActionResult> List()
        {
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            return Ok(id);
        }
    }
}
