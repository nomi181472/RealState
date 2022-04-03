using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realstate.models.ViewModels
{
    public class VerifiedUser
    {
        public int VerifiedUserID { get; set; }

        public bool IsVerified { get; set; }
        public string SecurityLevel { get; set; }

    }
}
