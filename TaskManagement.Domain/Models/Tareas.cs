using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Domain.Models;

public class Tareas
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Escribir una descripción es obligatorio.")]
    [MinLength(5, ErrorMessage = "La descripción debe tener al menos 5 caracteres.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "La fecha de vencimiento es obligatoria.")]
    public DateTime DueDate { get; set; }

    [Required(ErrorMessage = "El estado es obligatorio (ej: 'Pendiente' 'En proceso' o 'Completada').")]
    public string Status { get; set; }
    public string? Priority { get; set; }

    public string? ExtraData { get; set; }

}
