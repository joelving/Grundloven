using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grundloven.ViewModels
{
    public abstract class ApiResponse
    {
        public bool Success { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}
