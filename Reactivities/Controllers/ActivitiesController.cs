using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Activities;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Reactivities.Controllers
{
   
    public class ActivitiesController : BaseController
    {

        [HttpGet]
        public async Task<ActionResult<List<Activity>>> Get()
        {
            var query = new List.Query();
            var activities = await Mediator.Send(query);
            return Ok(activities);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Activity>> Get(Guid id)
        {
            var query = new Details.Query { Id = id};
            var activity = await Mediator.Send(query);
            return Ok(activity);
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Add(Create.Command command)
        {
            return await Mediator.Send(command);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id,Update.Command command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            var command = new Delete.Command { Id = id };
            return await Mediator.Send(command);
        }
    }
}