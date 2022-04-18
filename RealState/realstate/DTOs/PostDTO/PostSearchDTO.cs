using realstate.models.ViewModels.VMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace realstate.DTOs.PostDTO
{
    public class PostSearchDTO
    {
        public int page { get; set; } = 1;
        public string Category { get; set; }
        public string SearhMaterial { get; set; }

    }
}
