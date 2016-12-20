namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using EntityModels;

    public class DocumentDBRepository<T> : DocumentDb where T : EntityBase
    {
        //private string DatabaseId = ConfigurationManager.AppSettings["database"];
        //private string CollectionId = ConfigurationManager.AppSettings["collection"];
        //private DocumentClient client;
        private static string _userId;
        public DocumentDBRepository()
            : base(ConfigurationManager.AppSettings["database"], ConfigurationManager.AppSettings["collection"]) { }

        public DocumentDBRepository(string collectionId, string UserId)
            : base(ConfigurationManager.AppSettings["database"], collectionId){
                _userId = UserId;
        }

        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                Document document = await Client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                if (((T)(dynamic)document).UserId == _userId)
                    return (T)(dynamic)document;
                else
                    return null;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<T> GetUserItemAsync()
        {
            IDocumentQuery<T> query = Client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(func => func.UserId == _userId)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }
            if (results == null || !results.Any())
                return null;
            return results.First();
        }
        public async Task<List<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = Client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .Where(func => func.UserId == _userId)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<Document> CreateItemAsync(T item)
        {
            item.UserId = _userId;
            return await Client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
        }

        public async Task<Document> UpdateItemAsync(string id, T item)
        {
            return await Client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), item);
        }

        public async Task DeleteItemAsync(string id)
        {
            await Client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
        }

       
    }
}
