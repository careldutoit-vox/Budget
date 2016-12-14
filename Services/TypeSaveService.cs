using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services
{
    public class TypeSaveService<T> : ITypeSaveService<T> where T : EntityBase
    {
        protected DocumentDBRepository<T> _dbContext;
        #region Ctor
        public TypeSaveService() 
        {
            _dbContext = new DocumentDBRepository<T>(typeof(T).Name);
        }
        public TypeSaveService(string collectionName)
        {
            _dbContext = new DocumentDBRepository<T>(collectionName);
        }
        #endregion

        #region Public Methods
       
        #endregion

        public async Task<T> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<Microsoft.Azure.Documents.Document> CreateItemAsync(T item)
        {
            throw new NotImplementedException();
        }

        public async Task<Microsoft.Azure.Documents.Document> UpdateItemAsync(string id, T item)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
