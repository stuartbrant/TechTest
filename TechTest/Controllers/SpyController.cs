using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TechTest.Core.Interfaces;
using TechTest.Core.Models;
using TechTest.Core.RequestObjects;
using TechTest.Data.Repositories;

namespace TechTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpyController : ControllerBase
    {
        IRepository<Spy> _repository;
        public SpyController(IRepository<Spy> repository)
        {
            _repository = repository;
        }
        // Post api/spy
        public IActionResult Post(SpyRequest request)
        {
            var spyModel = _repository.Get().SingleOrDefault(s => s.Name == request.Spy);
            if (spyModel == null) return BadRequest("Spy not found");
            int codeElement;
            int[] spyArray = spyModel.CodeName.ToCharArray()
                .Where(cn => Int32.TryParse(cn.ToString(), out codeElement))
                .Select(cn => int.Parse(cn.ToString())).ToArray();
            if (spyArray.Count() == 0) return BadRequest("Invalid code name");
            foreach (var element in spyArray)
            {
                int index = Array.IndexOf(request.Message, element);
                if (index == -1)
                {
                    return Ok(false);
                }
                request.Message = request.Message.Skip(index + 1).ToArray();
            }
            return Ok(true);

        }


    }
}
