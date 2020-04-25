namespace Application.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    public static class PaginationHelpers
    {
        public static PagedResponse<T> CreatePaginatedResponse<T>(
            PaginationFilter pagination,
            List<T> response,
            int totalDataCountInDatabase)
        {
            var nextPage = pagination.PageNumber >= 1
                ? pagination.PageNumber + 1
                : (int?)null;
            var previousPage = pagination.PageNumber - 1 >= 1
                ? pagination.PageNumber - 1
                : (int?)null;

            return new PagedResponse<T>
            {
                Data = response,
                PageNumber = pagination.PageNumber >= 1 ? pagination.PageNumber : 1,
                PageSize = pagination.PageSize >= 1 ? pagination.PageSize : (int?)null,
                NextPage = response.Any() ? nextPage : null,
                PreviousPage = previousPage,
                TotalPages = (int)Math.Ceiling(totalDataCountInDatabase / (double)AppConstants.PageSize)
            };
        }
    }
}