using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : Controller
    {
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper;
        public CommandsController(ICommandRepo repo,IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Getting Commands For {platformId}.Platform.");
            if (!_repo.PlatformExists(platformId))
            {
                return NotFound();
            }
            var commandItem = _repo.GetCommandsForPlatform(platformId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItem));
        }
        [HttpGet("{commandId}",Name ="GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId,int commandId)
        {
            Console.WriteLine($"--> Get Command For Platform {platformId} / {commandId}");
            if (!_repo.PlatformExists(platformId))
            {
                return NotFound();
            }
            var commandItem = _repo.GetCommand(platformId, commandId);

            if (commandItem==null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CommandReadDto>(commandItem));

        }
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId,CommandCreateDto commandDto)
        {
            Console.WriteLine($"--> Create Command For Platform {platformId}");
            if (!_repo.PlatformExists(platformId))
            {
                return NotFound();
            }
            var commandModel = _mapper.Map<Command>(commandDto);
            _repo.CreateCommand(platformId, commandModel);
            _repo.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);
            return CreatedAtRoute(nameof(GetCommandForPlatform),
                new { platformId = platformId, commandId = commandReadDto.Id }, commandReadDto);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
