using System.Linq.Expressions;
using DAL.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DAL.Extensions;

record PaginationInfo(int TotalItems, int OffSet, int TotalPages);

public static class QueryExtension
{
    private static async Task<PaginationInfo> GetPagination<TEntity>(IQueryable<TEntity> query, PaginationDto pagination)
    {
        Console.WriteLine($"[WithPagination] Início - Page: {pagination.Page}, Size: {pagination.Size}");
        
        if(pagination.Size <= 0) throw new BadHttpRequestException($"{nameof(pagination.Size)}: Pagination size must be greater than zero.");

        var totalItems = await query.CountAsync();
        Console.WriteLine($"[WithPagination] TotalItems: {totalItems}");

        var offSet = pagination.Size * (pagination.Page - 1); 

        return new PaginationInfo(totalItems, offSet, (int)Math.Ceiling((decimal)totalItems / pagination.Size));
    }

    public static Task<PageableDto<TEntity>> WithPagination<TEntity>(this IQueryable<TEntity> query, PaginationDto pagination)
        => query.WithPagination(x => x, pagination);

    public static async Task<PageableDto<TDto>> WithPagination<TEntity, TDto>(this IQueryable<TEntity> query, Expression<Func<TEntity, TDto>> projection, PaginationDto pagination)
    {
        var pageInfo = await GetPagination(query, pagination);
        var items = await query.Select(projection).Skip(pageInfo.OffSet).Take(pagination.Size).ToListAsync();
        
        return new PageableDto<TDto>()
        {
            Page = pagination.Page,
            Size = pagination.Size,
            TotalItems = pageInfo.TotalItems,
            TotalPages = pageInfo.TotalPages,
            Items = items
        };
    }
}