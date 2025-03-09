using Microsoft.EntityFrameworkCore;
using RKSoftware.Packages.ViewModel.Extensions;

namespace RKSoftware.Packages.ViewModel.EFExtensions;

public static class QueryableExtension
{
    /// <summary>
    /// Get list result model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="requestModel"></param>
    /// <returns></returns>
    public static async Task<BaseListResultViewModel<T>> ToListModel<T>(
        this IQueryable<T> queryable,
        BaseListRequestViewModel requestModel) where T : class
    {
        return await ToListModel<T>(queryable, requestModel, true);
    }

    /// <summary>
    /// Get list result model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="requestModel"></param>
    /// <param name="isSorting"></param>
    /// <returns></returns>
    public static async Task<BaseListResultViewModel<T>> ToListModel<T>(
       this IQueryable<T> queryable,           
       BaseListRequestViewModel requestModel,
       bool isSorting) where T : class
    {
        ArgumentNullException.ThrowIfNull(requestModel, nameof(requestModel));

        var baseList = new BaseListResultViewModel<T>
        {
            PageNumber = requestModel.PageNumber,
            PageSize = requestModel.PageSize,
            Data = (await queryable
                .ApplyList(requestModel, isSorting)
                .ToListAsync())
        };

        baseList.CheckAndSetNext();
        return baseList;
    }

    /// <summary>
    /// Get list result model
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="requestModel"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static async Task<BaseListResultViewModel<TOutput>> ToListModel<TInput, TOutput>(
        this IQueryable<TInput> queryable,
        BaseListRequestViewModel requestModel,
        Func<TInput, TOutput> selector)
        where TOutput : class
    {
        return await ToListModel<TInput, TOutput>(queryable, requestModel, true, selector);
    }

    /// <summary>
    /// Get list result model
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="requestModel"></param>
    /// <param name="isSorting"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static async Task<BaseListResultViewModel<TOutput>> ToListModel<TInput, TOutput>(
        this IQueryable<TInput> queryable,
        BaseListRequestViewModel requestModel,
        bool isSorting,
        Func<TInput, TOutput> selector)
        where TOutput : class
    {
        ArgumentNullException.ThrowIfNull(requestModel, nameof(requestModel));

        ArgumentNullException.ThrowIfNull(selector, nameof(selector));

        var baseList = new BaseListResultViewModel<TOutput>
        {
            PageNumber = requestModel.PageNumber,
            PageSize = requestModel.PageSize,
            Data = (await queryable
                .ApplyList(requestModel, isSorting)
                .ToListAsync())
                .Select(selector)
            .ToList()
        };

        baseList.CheckAndSetNext();
        return baseList;
    }
}
