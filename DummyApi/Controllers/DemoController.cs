using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DummyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DemoController : ControllerBase
    {
        private static readonly ConcurrentDictionary<Guid, bool> _delayDict =
            new ConcurrentDictionary<Guid, bool>();
        private static readonly ConcurrentDictionary<(Guid, int), bool> _errorDict =
            new ConcurrentDictionary<(Guid, int), bool>();

        [HttpGet]
        [Route("delay/{requestId:guid}/{delayInMillisecs:int}")]
        public async Task<IActionResult> Delay(
            [FromRoute, Required] Guid requestId,
            [FromRoute, Required] int delayInMillisecs)
        {
            if (_delayDict.TryGetValue(requestId, out var _))
            {
                return Ok($"{requestId} was not delayed.");
            }
            else
            {
                _delayDict[requestId] = true;
                await Task.Delay(delayInMillisecs);
                return Ok($"{requestId} was delayed for {delayInMillisecs}ms.");
            }
        }

        [HttpPost]
        [Route("reset_delay/{requestId:guid}")]
        public IActionResult ResetDelay([FromRoute, Required] Guid requestId)
        {
            if (_delayDict.TryRemove(requestId, out var _))
            {
                return Ok($"Reset delay for requestId: {requestId}");
            }
            else
            {
                return NotFound($"Delay for requestId <{requestId}> does not exist.");
            }
        }

        [HttpGet]
        [Route("error/{requestId:guid}/{errorCode:int}")]
        public IActionResult Error(
            [FromRoute, Required] Guid requestId,
            [FromRoute, Required] int errorCode)
        {
            if (_errorDict.TryGetValue((requestId, errorCode), out var _))
            {
                return Ok($"{requestId} did not throw an error.");
            }
            else
            {
                _errorDict[(requestId, errorCode)] = true;
                return StatusCode(errorCode, $"{requestId} threw error code: {errorCode}");
            }
        }

        [HttpPost]
        [Route("reset_error/{requestId:guid}")]
        public IActionResult ResetError([FromRoute, Required] Guid requestId)
        {
            var keysToDelete = _errorDict.Keys.Where(n => n.Item1 == requestId).ToList();
            if (keysToDelete.Any())
            {
                keysToDelete.ForEach(key => _errorDict.TryRemove(key, out var _));
                return Ok($"Reset error for requestId: {requestId}");
            }
            else
            {
                return NotFound($"error for requestId <{requestId}> does not exist.");
            }
        }
    }
}
