using TaskManagement.Domain.Models;


namespace TaskManagement.Domain.Delegates
{
    public delegate (bool IsValid, string ErrorMessage) TaskValidationDelegate(Tareas tarea);

    public delegate T TaskTransformDelegate<T>(Tareas tarea);

    public delegate bool TaskFilterDelegate(Tareas tarea);
}