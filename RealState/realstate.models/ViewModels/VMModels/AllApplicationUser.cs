using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realstate.models.ViewModels.VMModels
{
  public  class AllApplicationUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsVerified { get; set; }
        public bool Role { get; set; }
        public string  Id { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
