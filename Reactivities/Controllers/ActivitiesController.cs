using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Activities;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Reactivities.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ActivitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Activity>>> Get()
        {
            var query = new List.Query();
            var activities = await _mediator.Send(query);
            return Ok(activities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> Get(Guid id)
        {
            var query = new Details.Query { Id = id};
            var activity = await _mediator.Send(query);
            return Ok(activity);
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Add(Create.Command command)
        {
            return await _mediator.Send(command);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id,Update.Command command)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            var command = new Delete.Command { Id = id };
            return await _mediator.Send(command);
        }
    }
}