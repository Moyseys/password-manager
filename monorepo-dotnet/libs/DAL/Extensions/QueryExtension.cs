using System.Linq.Expressions;
using DAL.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DAL.Extensions;

public static class QueryExtension
{
    //Todo transformar em um builder
    //Todo adicionar SORT generico
    public static async Task<PageableDto<TDto>> WithPagination<TEntity, TDto>(this IQueryable<TEntity> query, Expression<Func<TEntity, TDto>> projection, PaginationDto pagination)
    {
        Console.WriteLine($"[WithPagination] Início - Page: {pagination.Page}, Size: {pagination.Size}");
        
        if(pagination.Size <= 0) throw new BadHttpRequestException($"{nameof(pagination.Size)}: Pagination size must be greater than zero.");
        
        var totalItems = await query.CountAsync();
        Console.WriteLine($"[WithPagination] TotalItems: {totalItems}");

        var offSet = pagination.Size * (pagination.Page - 1); 

        var items = await query.Select(projection).Skip(offSet).Take(pagination.Size).ToListAsync();
        Console.WriteLine($"[WithPagination] Items recuperados: {items?.Count ?? 0}");

        var result = new PageableDto<TDto>()
        {
            Page = pagination.Page,
            Size = pagination.Size,
            TotalItems = totalItems,
            TotalPages= (int)Math.Ceiling((decimal)totalItems / pagination.Size),
            Items = items
        };
        
        return result;
    }

    public static async Task<PageableDto<TEntity>> WithPagination<TEntity>(this IQueryable<TEntity> query, PaginationDto pagination)
    {
        if(pagination.Size <= 0) throw new BadHttpRequestException($"{nameof(pagination.Size)}: Pagination size must be greater than zero.");

        var totalItems = await query.CountAsync();

        var offSet = pagination.Size * (pagination.Page - 1); 

        var items = await query.Skip(offSet).Take(pagination.Size).ToListAsync();

        return new PageableDto<TEntity>()
        {
            Page = pagination.Page,
            Size = pagination.Size,
            TotalItems = totalItems,
            TotalPages= (int)Math.Ceiling((decimal)totalItems / pagination.Size),
            Items = items
        };
    }
}