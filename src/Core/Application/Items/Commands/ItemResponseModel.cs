namespace Application.Items.Commands
{
    using System;

    public class ItemResponseModel
    {
        public ItemResponseModel(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; }
    }
}
