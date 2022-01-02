using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RKSoftware.Packages.ViewModel.Extensions;

namespace RKSoftware.Packages.ViewModel.EFExtensions.Extensions
{
    public static class QueriableExtension
    {
        /// <summary>
        /// Get list result model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queriable"></param>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public static async Task<BaseListResultViewModel<T>> ToListModel<T>(
            this IQueryable<T> queriable,
            BaseListRequestViewModel requestModel) where T : class
        {
            return await ToListModel<T>(queriable, requestModel, true);
        }

        /// <summary>
        /// Get list result model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queriable"></param>
        /// <param name="requestModel"></param>
        /// <param name="isSorting"></param>
        /// <returns></returns>
        public static async Task<BaseListResultViewModel<T>> ToListModel<T>(
           this IQueryable<T> queriable,           
           BaseListRequestViewModel requestModel,
           bool isSorting) where T : class
        {
            if (requestModel == null)
            {
                throw new ArgumentNullException(nameof(requestModel));
            }

            var baseList = new BaseListResultViewModel<T>
            {
                PageNumber = requestModel.PageNumber,
                PageSize = requestModel.PageSize,
                Data = (await queriable
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
        /// <param name="queriable"></param>
        /// <param name="requestModel"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static async Task<BaseListResultViewModel<TOutput>> ToListModel<TInput, TOutput>(
            this IQueryable<TInput> queriable,
            BaseListRequestViewModel requestModel,
            Func<TInput, TOutput> selector)
            where TOutput : class
        {
            return await ToListModel<TInput, TOutput>(queriable, requestModel, true, selector);
        }

        /// <summary>
        /// Get list result model
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="queriable"></param>
        /// <param name="requestModel"></param>
        /// <param name="isSorting"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static async Task<BaseListResultViewModel<TOutput>> ToListModel<TInput, TOutput>(
            this IQueryable<TInput> queriable,
            BaseListRequestViewModel requestModel,
            bool isSorting,
            Func<TInput, TOutput> selector)
            where TOutput : class
        {
            if (requestModel == null)
            {
                throw new ArgumentNullException(nameof(requestModel));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            var baseList = new BaseListResultViewModel<TOutput>
            {
                PageNumber = requestModel.PageNumber,
                PageSize = requestModel.PageSize,
                Data = (await queriable
                    .ApplyList(requestModel, isSorting)
                    .ToListAsync())
                    .Select(selector)
                .ToList()
            };

            baseList.CheckAndSetNext();
            return baseList;
        }
    }
}
