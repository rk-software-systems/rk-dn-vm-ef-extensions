﻿using Microsoft.EntityFrameworkCore;
using RKSoftware.Packages.ViewModel.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RKSoftware.Packages.ViewModel.EFExtensions.Extensions
{
    public static class ProcessQueriableExtension
    {
        /// <summary>
        /// Process Queriable
        /// </summary>
        /// <param name="queriable"></param>
        /// <param name="requestModel"></param>
        /// <param name="selector"></param>
        /// <typeparam name="TInput"></typeparam>
        public static async Task ProcessQueriable<TInput, TOutput>(
            this BaseListResultViewModel<TOutput> baseList, IQueryable<TInput> queriable,
            BaseListRequestViewModel requestModel,
            Func<TInput, TOutput> selector)
            where TOutput : class
        {
            await ProcessQueriable(baseList, queriable, requestModel, true, selector);
        }

        /// <summary>
        /// Process Queriable
        /// </summary>
        /// <param name="queriable"></param>
        /// <param name="requestModel"></param>
        /// <param name="isSorting"></param>
        /// <param name="selector"></param>
        /// <typeparam name="TInput"></typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task ProcessQueriable<TInput, TOUtput>(
            this BaseListResultViewModel<TOUtput> baseList, IQueryable<TInput> queriable,
            BaseListRequestViewModel requestModel, bool isSorting,
            Func<TInput, TOUtput> selector)
            where TOUtput : class
        {
            if (requestModel == null)
            {
                throw new ArgumentNullException(nameof(requestModel));
            }

            baseList.PageNumber = requestModel.PageNumber;
            baseList.PageSize = requestModel.PageSize;

            baseList.Data = (await queriable
                    .ApplyList(requestModel, isSorting)
                    .ToListAsync())
                .Select(selector)
                .ToList();

            requestModel.PageSize++;

            baseList.SetNext(baseList.Data.Count > baseList.PageSize);
        }
    }
}