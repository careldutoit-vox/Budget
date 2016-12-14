namespace Services
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;

    public interface ITypeSaveService<T> where T : EntityBase
    {
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate);

        Task<Document> CreateItemAsync(T item);

        Task<Document> UpdateItemAsync(string id, T item);

        Task DeleteItemAsync(string id);

       
    }
}
