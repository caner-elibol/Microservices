using AutoMapper;
using Grpc.Core;
using PlatformService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.SyncDataServices.Grpc
{
    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepo _repos;
        private readonly IMapper _mapper;

        public GrpcPlatformService(IPlatformRepo repos,IMapper mapper)
        {
            _repos = repos;
            _mapper = mapper;
        }

        public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            var response = new PlatformResponse();
            var platforms = _repos.GetAllPlatforms();

            foreach (var plat in platforms)
            {
                response.Platform.Add(_mapper.Map<GrpcPlatformModel>(plat));
            }
            return Task.FromResult(response);
        }
    }
}
