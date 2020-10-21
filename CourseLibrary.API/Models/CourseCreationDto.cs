using CourseLibrary.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Models
{
    public class CourseCreationDto : courseForManipulationDto
        //: IValidatableObject
    {
        

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
