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

        public AuthorsController(ICourseLibraryRepository courseLibrary) 
        {
            _CourseLibraryRepository = courseLibrary ??
                throw new ArgumentNullException(nameof(courseLibrary));
        }
        [HttpGet()]
        public IActionResult GetAuthors()
        {
            var authorsFromRepo = _CourseLibraryRepository.GetAuthors();
            return new JsonResult(authorsFromRepo);
        }
        [HttpGet("{authorid}")]
        public IActionResult GetAuthor(Guid authorid)
        {
            var authorFromRepo = _CourseLibraryRepository.GetAuthor(authorid);
            
            if(authorFromRepo == null)
            {
                return NotFound();
            }

            return Ok(authorFromRepo);
        }
    }
}
