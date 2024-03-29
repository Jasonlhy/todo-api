﻿using System.Text.Json.Serialization;

namespace JasonTodoApi.ViewModels;

/// <summary>
/// Information of each todo item displaying to user
/// </summary>
public record TodoItem
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description ")]
    public required string Description { get; set; }

    [JsonPropertyName("dueDate")]
    public DateTime DueDate { get; set; }

    [JsonPropertyName("status")]
    public required int Status { get; set; }

    [JsonPropertyName("statusString")]
    public required string StatusString { get; set; }
}


