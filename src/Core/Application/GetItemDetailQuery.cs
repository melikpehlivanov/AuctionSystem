using MediatR;

namespace Application
{
    public class GetItemDetailQuery : IRequest<ItemDetailAppModel>
    {
        public string Id { get; set; }
    }
}
