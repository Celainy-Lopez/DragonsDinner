﻿using DragonsDinner.Abstractions;
using DragonsDinner.Data.Models;
using DragonsDinner.Data;
using DragonsDinner.Domain.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DragonsDinner.Services;

public class OrdenesService(IDbContextFactory<ApplicationDbContext> DbFactory) : IOrdenesService
{
    public async Task<OrdenesDto> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var Orden = await contexto.Ordenes
            .Where(e => e.OrdenId == id).Select(p => new OrdenesDto()
            {
                OrdenId = p.OrdenId,
                Total = p.Total,
                Fecha = p.Fecha,
                OrdenesDetalles = p.OrdenesDetalles,
                Delivery = p.Delivery
            }).FirstOrDefaultAsync();
        return Orden ?? new OrdenesDto();
    }

    public async Task<bool> Eliminar(int ordenId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Ordenes
            .Where(e => e.OrdenId == ordenId)
            .ExecuteDeleteAsync() > 0;
    }

    private async Task<bool> Insertar(OrdenesDto ordenesDto)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var ordenes = new Ordenes()
        {
            Total = ordenesDto.Total,
            Fecha = ordenesDto.Fecha,
            OrdenesDetalles = ordenesDto.OrdenesDetalles,
            Delivery = ordenesDto.Delivery
        };
        contexto.Ordenes.Add(ordenes);
        var guardo = await contexto.SaveChangesAsync() > 0;
        ordenesDto.OrdenId = ordenes.OrdenId;
        return guardo;
    }

    private async Task<bool> Modificar(OrdenesDto ordenDto)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var orden = new Ordenes()
        {
            Total = ordenDto.Total,
            Fecha = ordenDto.Fecha,
            Delivery = ordenDto.Delivery,
            OrdenesDetalles = ordenDto.OrdenesDetalles,
        };
        contexto.Update(orden);
        var modificado = await contexto.SaveChangesAsync() > 0;
        return modificado;
    }

    private async Task<bool> Existe(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Ordenes
            .AnyAsync(e => e.OrdenId == id);
    }

    public async Task<bool> Guardar(OrdenesDto orden)
    {
        if (!await Existe(orden.OrdenId))
            return await Insertar(orden);
        else
            return await Modificar(orden);
    }

    public async Task<List<OrdenesDto>> Listar(Expression<Func<OrdenesDto, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Ordenes.Select(p => new OrdenesDto()
        {
            OrdenId = p.OrdenId,
            Total = p.Total,
            Fecha = p.Fecha,
            OrdenesDetalles = p.OrdenesDetalles,
            Delivery = p.Delivery
        })
        .Where(criterio)
        .ToListAsync();
    }
}
