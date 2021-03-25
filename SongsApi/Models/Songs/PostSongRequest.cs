using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SongsApi.Models.Songs
{
    public class PostSongRequest : IValidatableObject
    {
        [Required][StringLength(100)]
        public string Title { get; set; }
        public string Artist { get; set; }
        [Required]
        public string RecommendedBy { get; set; }

        //Extending IValidatableOjbect allows you to add more complex rules here
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Title.ToLower() == "never gonna give you up")
            {
                yield return new ValidationResult("I Hate That Song", new string[]{
                    nameof(Title), nameof(Artist)
                });
            }
        }
    }

}
