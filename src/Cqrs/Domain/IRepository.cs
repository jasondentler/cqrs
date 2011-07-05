using System;

namespace Cqrs.Domain
{
    public interface IRepository
    {
        void Save(IEventSource eventSource);
        T GetById<T>(Guid id) where T : class, IEventSource;
    }
}