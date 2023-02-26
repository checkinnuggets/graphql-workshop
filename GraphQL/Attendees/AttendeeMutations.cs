﻿using Microsoft.EntityFrameworkCore;
using ConferencePlanner.GraphQL.Common;
using ConferencePlanner.GraphQL.Data;
using HotChocolate.Language;

namespace ConferencePlanner.GraphQL.Attendees
{
    [ExtendObjectType(OperationType.Mutation)]
    public class AttendeeMutations
    {
        public async Task<RegisterAttendeePayload> RegisterAttendeeAsync(
            RegisterAttendeeInput input,
            ApplicationDbContext context,
            CancellationToken cancellationToken)
        {
            var attendee = new Attendee
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                UserName = input.UserName,
                EmailAddress = input.EmailAddress
            };

            context.Attendees.Add(attendee);

            await context.SaveChangesAsync(cancellationToken);

            return new RegisterAttendeePayload(attendee);
        }

        public async Task<CheckInAttendeePayload> CheckInAttendeeAsync(
            CheckInAttendeeInput input,
            ApplicationDbContext context,
            [Service] ITopicEventSender eventSender,
            CancellationToken cancellationToken)
        {
            Attendee? attendee = await context.Attendees.FirstOrDefaultAsync(
                t => t.Id == input.AttendeeId, cancellationToken);

            if (attendee is null)
            {
                return new CheckInAttendeePayload(
                    new UserError("Attendee not found.", "ATTENDEE_NOT_FOUND"));
            }

            attendee.SessionsAttendees.Add(
                new SessionAttendee
                {
                    SessionId = input.SessionId
                });

            await context.SaveChangesAsync(cancellationToken);

            return new CheckInAttendeePayload(attendee, input.SessionId);
        }
    }
}