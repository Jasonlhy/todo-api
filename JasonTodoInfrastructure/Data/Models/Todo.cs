﻿using System.ComponentModel.DataAnnotations;

namespace JasonTodoInfrastructure.Data.Models;

/// <summary>
/// Entity Framework DB Model for Todo table
/// </summary>
public class Todo
{
    [Key]
    public long Id { get; set; }

    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(255)]
    public required string Description { get; set; }

    public required DateTime DueDate { get; set; }

    public required int Status { get; set; }
}