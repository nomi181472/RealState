using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realstate.models.ViewModels
{
    public class ApplicationUser:IdentityUser
    {

        public int? VerifiedUserId { get; set; }
        [ForeignKey(nameof(VerifiedUserId))]

        public VerifiedUser VerifiedUserTBL { get; set; }

        [NotMapped]
        public  List<string> Roles { get; set; }
       



    }
}
