namespace Common
{
    using System;

    public interface IDateTime
    {
        DateTime Now { get; }

        DateTime UtcNow { get; }
    }
}
