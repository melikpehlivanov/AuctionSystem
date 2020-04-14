namespace Application.Common.Interfaces
{
    using System;
    using Models;

    public interface IUriService
    {
        Uri GetPaginationUri(PaginationQuery paginationQuery = null);
    }
}
