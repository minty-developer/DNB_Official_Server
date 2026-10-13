using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(4)]
    public string StudentId { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public int Coin { get; set; } = 0;
}

public class Notice
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class Vote
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public bool IsClosed { get; set; } = false;

    public DateTime ExpiredAt { get; set; }

    // 1:N
    public List<VoteOption> Options { get; set; } = new();
}

public class VoteOption
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int VoteId { get; set; }

    [ForeignKey("VoteId")]
    public Vote Vote { get; set; } = null!;

    [Required]
    public string Text { get; set; } = string.Empty;

    public int VoteCount { get; set; } = 0;
}

public class Schedule
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime ScheduleDate { get; set; }
}