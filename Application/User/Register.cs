using Application.Errors;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.User
{
   public class Register
    {
        public class Command: IRequest<User>
        {
            public string DisplayName { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string  Password { get; set; }
        }

        public class Validator: AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.DisplayName).NotEmpty();
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.Email).EmailAddress();
                RuleFor(x => x.Password).NotEmpty();
            }

        }

        public class Handler : IRequestHandler<Command, User>
        {
            private readonly UserManager<AppUser> _manager;
            private readonly AppDbContext _context;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(UserManager<AppUser> manager, AppDbContext context, IJwtGenerator jwtGenerator)
            {
                _manager = manager;
                _context = context;
                _jwtGenerator = jwtGenerator;
            }
            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                if (_manager.FindByEmailAsync(request.Email).Result !=null) throw new RestException(System.Net.HttpStatusCode.BadRequest, "Email Address Already Exist");
                if ( await _context.Users.AnyAsync(x => x.UserName==request.UserName)) throw new RestException(System.Net.HttpStatusCode.BadRequest, "UserName Already Exist");

                var user = new AppUser
                {
                    DisplayName = request.DisplayName,
                    UserName = request.UserName,
                    Email = request.Email                    
                };
                var result = await _manager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    return new User
                    { 
                        UserName = request.UserName,
                        DisplayName = request.DisplayName,
                        Image = null,
                        Token = _jwtGenerator.CreateToken(user),
                    };
                }

                throw new Exception("Problem creating user");
            }
        }
    }
}
