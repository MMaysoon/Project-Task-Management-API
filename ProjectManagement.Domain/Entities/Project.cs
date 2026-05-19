using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // FK
        public string UserId { get; set; }= null!;

        // navigation property
        public ApplicationUser User { get; set; } = null!;

        // One Project -> Many Tasks
        public ICollection<TaskItem> TaskItems { get; set; }= new HashSet<TaskItem>();
    }
}
