using System.Threading.Tasks;
using TaskManagement.Domain.Delegates;
using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Services.TaskServices
{
    public class TaskValidationService
    {
        public TaskValidationDelegate CompleteValidation { get; private set; }

        public Action<string> NotificationAction { get; set; }

        // Func 
        public Func<DateTime, int> CalculateDaysRemaining { get; private set; }

        public TaskValidationService()
        {
            // aqui son delegados con funciones anonimas
            CompleteValidation = (tarea) =>
            {
                if (string.IsNullOrWhiteSpace(tarea.Description))
                    return (false, "La descripción no puede estar vacía");

                if (tarea.DueDate <= DateTime.Now)
                    return (false, "La fecha de vencimiento debe ser futura");

                return (true, string.Empty);
            };

            // Action 
            NotificationAction = message => Console.WriteLine($"[NOTIFICACIÓN]: {message} - {DateTime.Now}");

            // Func 
            CalculateDaysRemaining = dueDate => (dueDate - DateTime.Now).Days;
        }

        public (bool IsValid, List<string> Errors) ValidateWithMultipleRules(Tareas tarea, params TaskValidationDelegate[] validators)
        {
            var errors = new List<string>();

            foreach (var validator in validators)
            {
                var result = validator(tarea);
                if (!result.IsValid)
                    errors.Add(result.ErrorMessage);
            }

            return (errors.Count == 0, errors);
        }

        public TaskValidationDelegate DescriptionValidation =>
            tarea => string.IsNullOrWhiteSpace(tarea.Description)
                ? (false, "Descripción requerida")
                : (true, string.Empty);

        public TaskValidationDelegate DateValidation =>
            tarea => tarea.DueDate <= DateTime.Now
                ? (false, "Fecha debe ser futura")
                : (true, string.Empty);

        public TaskValidationDelegate ExtraDataValidation =>
            tarea => tarea.ExtraData?.Length > 500
                ? (false, "ExtraData no puede exceder 500 caracteres")
                : (true, string.Empty);
    }
}