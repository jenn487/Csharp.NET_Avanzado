using System.Reactive.Subjects;
using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Services.Reactive;

public interface IReactiveTaskQueue
{
    void EnqueueTask(Tareas tarea);
    IObservable<Tareas> TaskQueue { get; }
}

public class ReactiveTaskQueue : IReactiveTaskQueue
{
    private readonly Subject<Tareas> _taskSubject = new();

    public void EnqueueTask(Tareas tarea)
    {
        _taskSubject.OnNext(tarea);
    }

    public IObservable<Tareas> TaskQueue => _taskSubject;
}