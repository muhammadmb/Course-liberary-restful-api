using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors/{authorId}/courses")]
    public class CoursesController : Controller
    {
        private readonly ICourseLibraryRepository _coursesLibraryRepository;
        private readonly IMapper _mapper;

        public CoursesController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _coursesLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        public IActionResult GetCoursesForAuthor(Guid authorId)
        {
            var coursesForAuthors = _coursesLibraryRepository.GetCourses(authorId);
            if (!_coursesLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<CourseDto>>(coursesForAuthors));
        }
        [HttpGet("{courseId}", Name = "GetCourseForAuthor")]
        public IActionResult GetCourseForAuthor(Guid authorId, Guid courseId)
        {
            if (!_coursesLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var courseForAuthor = _coursesLibraryRepository.GetCourse(authorId, courseId);
            if (courseForAuthor == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CourseDto>(courseForAuthor));
            //return Ok(courseForAuthor);
        }
        [HttpPost]
        public ActionResult<CourseDto> CreateCourseFprAuthor(Guid authorId, CourseCreationDto course)
        {
            if (!_coursesLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var courseEntity = _mapper.Map<Entities.Course>(course);
            _coursesLibraryRepository.AddCourse(authorId, courseEntity);
            _coursesLibraryRepository.Save();

            var courseToReturn = _mapper.Map<CourseDto>(courseEntity);

            return CreatedAtRoute("GetCourseForAuthor", new { authorId = authorId, courseId = courseToReturn.Id }, courseToReturn);

        }
    }
}
