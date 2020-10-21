using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
        public ActionResult<CourseDto> CreateCourseForAuthor(Guid authorId, CourseCreationDto course)
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
        [HttpPut("{courseId}")]
        public ActionResult UpdateCourseForAuthor(Guid authorId, Guid courseId, courseUpdateDto course)
        {
            if (!_coursesLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var courseForAuthorFromRepo = _coursesLibraryRepository.GetCourse(authorId, courseId);

            if (courseForAuthorFromRepo == null)
            {
                var courseEntity = _mapper.Map<Entities.Course>(course);
                courseEntity.Id = courseId;
                _coursesLibraryRepository.AddCourse(authorId,courseEntity);
                _coursesLibraryRepository.Save();
                var courseToReturn = _mapper.Map<CourseDto>(courseEntity);
                return CreatedAtRoute(
                    "GetCourseForAuthor",
                    new { authorId, courseId = courseToReturn.Id },
                    courseToReturn
                );
            }
            _mapper.Map(course, courseForAuthorFromRepo);
            _coursesLibraryRepository.UpdateCourse(courseForAuthorFromRepo);
            _coursesLibraryRepository.Save();

            return NoContent();
        }
        [HttpPatch("{courseId}")]
        public ActionResult PartiallyUpdateCourseForAuthor (
            Guid authorId, 
            Guid courseId, 
            JsonPatchDocument<courseUpdateDto> patchDocument
        )
        {
            if (!_coursesLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseForAuthorFromRepo = _coursesLibraryRepository.GetCourse(authorId, courseId);

                if (courseForAuthorFromRepo == null)
                {
                    var courseDto = new courseUpdateDto();

                    patchDocument.ApplyTo(courseDto, ModelState);
                    
                    if (!TryValidateModel(courseDto))
                    {
                        return ValidationProblem(ModelState);
                    }

                var courseToAdd = _mapper.Map<Entities.Course>(courseDto);

                    courseToAdd.Id = courseId;

                    _coursesLibraryRepository.AddCourse(authorId , courseToAdd);

                    _coursesLibraryRepository.Save();

                    var courseToReturn = _mapper.Map<CourseDto>(courseToAdd);

                    return CreatedAtRoute("GetCourseForAuthor", new { authorId, courseId = courseToReturn.Id }, courseToReturn);
                }
            var courseToPatch = _mapper.Map<courseUpdateDto>(courseForAuthorFromRepo);

            patchDocument.ApplyTo(courseToPatch, ModelState); // add modelstate to return 400 - client mistake statusCode not server mistake

            // check if there are any problems with input
            if (!TryValidateModel(courseToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(courseToPatch, courseForAuthorFromRepo);

            _coursesLibraryRepository.UpdateCourse(courseForAuthorFromRepo);

            _coursesLibraryRepository.Save();

            return NoContent();
             
        }
        public override ActionResult ValidationProblem()
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
        [HttpDelete("{courseId}")]
        public ActionResult DeleteCourseForAuthor(Guid authorId, Guid courseId)
        {
            if (!_coursesLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var Course = _coursesLibraryRepository.GetCourse(authorId, courseId);

            if (Course == null)
            {
                return NotFound();
            }
            
            _coursesLibraryRepository.DeleteCourse(Course);

            _coursesLibraryRepository.Save();

            return NoContent();
        }
    }
}
