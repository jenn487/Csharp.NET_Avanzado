using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Domain.Models;

public class Tareas
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Description { get; set; }
    public DateTime DueDate { get; set; }

    public TaskStatus Status { get; set; }

    public PriorityLevel Priority { get; set; }

    public enum TaskStatus
    {
        Pendiente,
        Completado,
        Cancelado
    }

    public enum PriorityLevel
    {
        Low,
        Normal,
        High
    }

    public string ExtraData { get; set; }
}
