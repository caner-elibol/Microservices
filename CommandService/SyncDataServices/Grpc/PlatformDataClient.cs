using AutoMapper;
using CommandService.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using PlatformService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandService.SyncDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration _conf;
        private readonly IMapper _mapper;

        public PlatformDataClient(IConfiguration conf,IMapper mapper)
        {
            _conf = conf;
            _mapper = mapper;
        }

        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            Console.WriteLine($"--> Calling GRPC Service{_conf["GrpcPlatform"]}");

            var channel = GrpcChannel.ForAddress(_conf["GrpcPlatform"]);
            var client = new GrpcPlatform.GrpcPlatformClient(channel);

            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllPlatforms(request);
                Console.WriteLine("--> Success for Call GRPC Server");

                return _mapper.Map<IEnumerable<Platform>>(reply.Platform);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Error Couldn't call Grpc Server : {ex.Message}");
                return null;
            }
        }
    }
}
