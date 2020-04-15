namespace AuctionSystem.Infrastructure
{
    using System;
    using Application.Common.Interfaces;
    using Application.Common.Models;
    using Microsoft.AspNetCore.WebUtilities;

    public class UriService : IUriService
    {
        private readonly string baseUri;

        public UriService(string baseUri)
        {
            this.baseUri = baseUri;
        }

        public Uri GetPaginationUri(PaginationFilter paginationQuery = null)
        {
            var uri = new Uri(this.baseUri);

            if (paginationQuery == null)
            {
                return uri;
            }

            //TODO: Cleaner way to do this
            var modifiedUri = QueryHelpers.AddQueryString(this.baseUri, "pageNumber", paginationQuery.PageNumber.ToString()); 
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", paginationQuery.PageSize.ToString());

            return new Uri(modifiedUri);
        }
    }
}
