using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.ApI.DTos.Author
{
    public class AuthorReadOnlyDto : BaseDto
    {
        public string FirstName { get; set; }
     
        public string LastName { get; set; }
       
        public string Bio { get; set; }
    }
}

