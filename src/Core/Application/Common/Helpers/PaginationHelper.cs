namespace Application.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    public static class PaginationHelper
    {
        public static PagedResponse<T> CreatePaginatedResponse<T>(
            PaginationFilter pagination,
            List<T> response,
            int totalDataCountInDatabase)
        {
            var totalPages = (int) Math.Ceiling(totalDataCountInDatabase / (double) pagination.PageSize);
            var nextPage = pagination.PageNumber >= 1 && pagination.PageNumber < totalPages
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
                TotalPages = totalPages,
                TotalDataCount = totalDataCountInDatabase,
            };
        }
    }
}