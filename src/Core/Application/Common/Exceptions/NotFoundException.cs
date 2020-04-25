namespace Application.Common.Exceptions
{
    using System;

    public class NotFoundException : Exception
    {
        public NotFoundException(string name)
            : base($"Such '{name}' was not found.")
        {
        }

        //public NotFoundException(string name, object key)
        //    : base($"{name} with Id ({key}) was not found.")
        //{
        //}
    }
}