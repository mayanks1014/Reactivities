using Application.Errors;
using MediatR;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities
{
    public class Delete
    {
        public class Command: IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly AppDbContext _context;

            public Handler(AppDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Id);
                if (activity == null) throw new RestException(System.Net.HttpStatusCode.NotFound,"Could not find activity to delete");

                _context.Activities.Remove(activity);
                var success = await _context.SaveChangesAsync();
                return success > 0 ? Unit.Value : throw new Exception("Problem deleting activity");
            }
        }
    }
}
