﻿using Application.Classes;
using Application.DataTransferObjects.VsDuelParticipant;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Repositories;

public class VsDuelParticipantRepository(ApplicationContext context, IMapper mapper, ILogger<VsDuelParticipantRepository> logger) : IVsDuelParticipantRepository
{
    public async Task<Result<VsDuelParticipantDto>> UpdateVsDuelParticipant(VsDuelParticipantDto vsDuelParticipantDto, CancellationToken cancellationToken)
    {
        var participantToUpdate = await context.VsDuelParticipants
            .FirstOrDefaultAsync(vsDuelParticipant => vsDuelParticipant.Id == vsDuelParticipantDto.Id,
                cancellationToken);
        if (participantToUpdate is null) return Result.Failure<VsDuelParticipantDto>(VsDuelParticipantErrors.NotFound);

        mapper.Map(vsDuelParticipantDto, participantToUpdate);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<VsDuelParticipantDto>(participantToUpdate));
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result.Failure<VsDuelParticipantDto>(GeneralErrors.DatabaseError);
        }
    }
}