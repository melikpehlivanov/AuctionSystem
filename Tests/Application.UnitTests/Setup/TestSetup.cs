namespace Application.UnitTests.Setup
{
    using AutoMapper;
    using global::Common.AutoMapping.Profiles;

    public static class TestSetup
    {
        private static IMapper mapper;
        private static readonly object Sync = new object();
        private static bool mapperInitialized = false;

        public static IMapper InitializeMapper()
        {
            lock (Sync)
            {
                if (mapperInitialized)
                {
                    return mapper;
                }

                var config = new MapperConfiguration(cfg => { cfg.AddProfile<DefaultProfile>(); });

                mapper = config.CreateMapper();
                mapperInitialized = true;

                return mapper;
            }
        }
    }
}
