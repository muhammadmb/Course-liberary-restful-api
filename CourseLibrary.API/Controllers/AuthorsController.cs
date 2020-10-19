using AutoMapper;
using CourseLibrary.API.Models;
using CourseLibrary.API.ResouceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    //[Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _CourseLibraryRepository;

        private readonly IMapper _mapper;

        public AuthorsController(ICourseLibraryRepository courseLibrary, IMapper mapper ) 
        {
            _CourseLibraryRepository = courseLibrary ??
                throw new ArgumentNullException(nameof(courseLibrary));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet()]
        [HttpHead()]
        //public async Task<IActionResult> GetAuthors()
        //{
        //    //throw new Exception("fault");
        //    var authorsFromRepo = await _CourseLibraryRepository.GetAuthors();
        //    return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));
        //}
        public IActionResult GetAuthors([FromQuery] AuthorResourceParameters authorResourceParameters)
        {
            //throw new Exception("fault");
            var authorsFromRepo = _CourseLibraryRepository.GetAuthors(authorResourceParameters);
            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));
        }

        [HttpGet("{authorid}", Name ="GetAuthor")]
        public IActionResult GetAuthor(Guid authorid)
        {
            var authorFromRepo = _CourseLibraryRepository.GetAuthor(authorid);
            
            if(authorFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AuthorDto>(authorFromRepo));
        }
        [HttpPost()]
        public ActionResult<AuthorDto> CreateAuthor(AuthorCreationDto author)
        {
            var authorEntity = _mapper.Map<Entities.Author>(author);
            _CourseLibraryRepository.AddAuthor(authorEntity);
            _CourseLibraryRepository.Save();

            var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);
            return CreatedAtRoute("GetAuthor", 
                new{authorId = authorToReturn.Id},
                authorToReturn);
        }
    }
}
