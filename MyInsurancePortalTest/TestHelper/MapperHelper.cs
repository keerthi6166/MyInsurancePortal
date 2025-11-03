using AutoMapper;
using MyInsurancePortal.AutoMapper;
using Microsoft.Extensions.Logging;

namespace MyInsurancePortalTest.TestHelper
{
    public static class MapperHelper
    {
        private static IMapper? _mapper;

        public static IMapper GetMapper()
        {
            if (_mapper == null)
            {
                var loggerFactory = LoggerFactory.Create(config =>
                {
                    config.AddConsole();
                    config.AddDebug(); //created a dummy logger as the mapperconfiguration don't allow us to pass only on parameter.
                });
                var mapperConfig = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<MappingProfile>(); // your AutoMapper profile class from the main project, we can also create for individual models liks cfg.CreateMap<Claim,ClaimDto>();
                }, loggerFactory);

                _mapper = mapperConfig.CreateMapper();
            }

            return _mapper;
        }
    }
}
