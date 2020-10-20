using CourseLibrary.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Models
{
    [CourseTitleMustBeDifferentFromDescription(ErrorMessage = "The provided description should be different from the title.")]
    public class CourseCreationDto //: IValidatableObject
    {
        [Required(ErrorMessage = "You should fill out the title")]
        [MaxLength(100, ErrorMessage = "the title should not have more than 100 characters.")]
        public string Title { get; set; }
        [MaxLength(1500, ErrorMessage = "the description should not have more than 1500 characters.")]
        public string Description { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Title == Description)
        //    {
        //        yield return new ValidationResult(
        //            "The provided description should be different from the title.",
        //            new[] { "CourseCreationDto" }
        //        );
        //    }
        //    if (Title.IndexOf('W').Equals(0))
        //    {
        //        yield return new ValidationResult(
        //            "The first charactre is not avilable.",
        //            new[] { "CourseCreationDto" }
        //        );
        //    }
        //}
    }
}
