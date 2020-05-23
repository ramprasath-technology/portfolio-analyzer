using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class User
    {
        public ulong UserId { get; set; }
        [Required]
        [MinLength(6)]
        [MaxLength(20)]
        [RegularExpression("^[a-z0-9]*$")]
        public string UserName { get; set; }
    }
}
