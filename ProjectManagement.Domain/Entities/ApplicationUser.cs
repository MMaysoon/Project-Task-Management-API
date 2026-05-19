using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Entities
{
    [Table("Users")]
    public class ApplicationUser:IdentityUser
    {
        public ICollection<Project> Projects { get; set; } = new HashSet<Project>();    
    }
}
