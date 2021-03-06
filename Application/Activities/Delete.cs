using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistance;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest
        {

            public Guid Id { get; set; }



        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext context;

            public Handler(DataContext context)
            {
                this.context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await context.Activities.FindAsync(request.Id);
                if (activity == null)
                {
                    throw new RestExecption(HttpStatusCode.NotFound, new { activity = "not found" });

                }
                else
                {
                    context.Remove(activity);
                }


                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;
                throw new Exception("Problem Saving Changes");

            }
        }
    }
}
