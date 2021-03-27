using System.Collections.Generic;
using ThaGet.Cqrs.Contract.Abstractions;

namespace ThaGet.Cqrs.Contract.Queries
{
    public abstract class EntityCollectionQuery<TResponse, TResponseType> : IQuery<TResponse>
       where TResponse : class, IEnumerable<TResponseType>
    {
    }
}
