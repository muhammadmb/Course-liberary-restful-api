using CourseLibrary.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.ValidationAttributes
{
    public class CourseTitleMustBeDifferentFromDescription : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var course = (courseForManipulationDto)validationContext.ObjectInstance;
            if(course.Title == course.Description)
            {
                return new ValidationResult(
                    ErrorMessage,
                    new[] { "courseForManipulationDto" }
                );
            }
            return ValidationResult.Success;
        }
    }
}
