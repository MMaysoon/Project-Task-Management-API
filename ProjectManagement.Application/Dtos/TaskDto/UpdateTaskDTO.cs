using ProjectManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.Dtos.TaskDto
{
    public class UpdateTaskDTO
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DueDate { get; set; }

        public TaskItemStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
    }
}
