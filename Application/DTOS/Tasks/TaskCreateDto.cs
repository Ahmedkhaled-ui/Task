using Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOS.Tasks
{
   
        public class TaskCreateDto
        {
            [Required(ErrorMessage = "Title is required")]
            [StringLength(150)]
            public string Title { get; set; } = string.Empty;

            public string Description { get; set; } = string.Empty;

            [Required(ErrorMessage = "Status is required")]
        public Domain.Enums.TaskStatus Status { get; set; } = Domain.Enums.TaskStatus.ToDo;

        [Required(ErrorMessage = "Priority is required")]
            public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        [Required(ErrorMessage = "Due date is required")]
            public DateTime DueDate { get; set; } 

            [Required(ErrorMessage = "ProjectId is required")]
            public int ProjectId { get; set; } 
        }
    }
