using System;
using CoffeeClub.Domain.Dtos;
using CoffeeClub.Domain.Entities;

namespace CoffeeClub.Domain.Extensions;

public static class CoffeeExtensions
{
    public static CoffeeDto ToDto(this CoffeeEntity entity)
    {
        return new CoffeeDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Roast = entity.Roast
        };
    }

    public static CoffeeEntity ToEntity(this CoffeeDto dto)
    {
        return new CoffeeEntity
        {
            Id = dto.Id,
            Name = dto.Name,
            Roast = dto.Roast
        };
    }

    public static CoffeeEntity ToEntity(this CreateCoffeeDto createDto)
    {
        return new CoffeeEntity
        {
            Id = Guid.NewGuid(),
            Name = createDto.Name,
            Roast = createDto.Roast
        };
    }
}
