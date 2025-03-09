using Microsoft.EntityFrameworkCore;
using RKSoftware.Packages.ViewModel.Extensions;

namespace RKSoftware.Packages.ViewModel.EFExtensions;

public static class ProcessQueryableExtension
{
    /// <summary>
    /// Process Queryable
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="requestModel"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TInput"></typeparam>        
    public static async Task ProcessQueryable<TInput, TOutput>(
        this BaseListResultViewModel<TOutput> baseList, IQueryable<TInput> queryable,
        BaseListRequestViewModel requestModel,
        Func<TInput, TOutput> selector) where TOutput : class
    {
        await ProcessQueryable(baseList, queryable, requestModel, true, selector);
    }

    /// <summary>
    /// Process Queryable
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="requestModel"></param>
    /// <param name="isSorting"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TInput"></typeparam>
    /// <exception cref="ArgumentNullException"></exception>        
    public static async Task ProcessQueryable<TInput, TOutput>(
        this BaseListResultViewModel<TOutput> baseList, IQueryable<TInput> queryable,
        BaseListRequestViewModel requestModel, bool isSorting,
        Func<TInput, TOutput> selector) where TOutput : class
    {
        ArgumentNullException.ThrowIfNull(requestModel, nameof(requestModel));

        ArgumentNullException.ThrowIfNull(baseList, nameof(baseList));

        baseList.PageNumber = requestModel.PageNumber;
        baseList.PageSize = requestModel.PageSize;

        baseList.Data = (await queryable
                .ApplyList(requestModel, isSorting)
                .ToListAsync())
            .Select(selector)
            .ToList();

        baseList.CheckAndSetNext();
    }
}