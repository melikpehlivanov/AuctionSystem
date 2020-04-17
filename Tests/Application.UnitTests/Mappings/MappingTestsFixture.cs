namespace Application.UnitTests.Mappings
{
    using AutoMapper;
    using global::Common.AutoMapping.Profiles;

    public class MappingTestsFixture
    {
        public MappingTestsFixture()
        {
            this.ConfigurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DefaultProfile>();
            });

            this.Mapper = this.ConfigurationProvider.CreateMapper();
        }

        public IConfigurationProvider ConfigurationProvider { get; }

        public IMapper Mapper { get; }
    }
}
