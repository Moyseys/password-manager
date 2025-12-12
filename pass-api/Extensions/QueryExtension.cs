using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PasswordManager.SharedDtos;

namespace PasswordManager.Extensions;

public static class QueryExtension
{
    public static async Task<PageableDto<TDto>> WithPagination<TEntity, TDto>(this IQueryable<TEntity> query, Expression<Func<TEntity, TDto>> projection, PaginationDto pagination)
    {
        var totalItems = await query.CountAsync();

        var offSet = pagination.Size * (pagination.Page - 1); 

        var items = await query.Select(projection).Skip(offSet).Take(pagination.Size).ToListAsync();

        return new PageableDto<TDto>()
        {
            Page = pagination.Page,
            Size = pagination.Size,
            TotalItems = totalItems,
            TotalPages= (int)Math.Ceiling((decimal)totalItems / pagination.Size),
            Items = items
        };
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