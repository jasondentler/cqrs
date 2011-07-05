using System;

namespace Cqrs.Domain
{
    public interface IRepository<T> where T : EventSource
    {
        void Save(EventSource eventSource);
        T GetById(Guid id);
    }
}