namespace Repository
{
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public class DocumentDb
    {
        private static string _databaseId;
        private static string _collectionId;
        private static Database _database;
        private static DocumentCollection _collection;
        private static DocumentClient _client;

        public DocumentDb(string database, string collection)
        {
            _databaseId = database;
            _collectionId = collection;
            Initilization = InitializeAsync();
        }
        protected static string DatabaseId
        {
            get { return _databaseId;}
        }
        protected static string CollectionId
        {
            get { return _collectionId; }
        }
        public Task Initilization { get; private set; }

        private async Task InitializeAsync()
        {
            await ReadOrCreateDatabase();
            await ReadOrCreateCollection();
        }

        protected static DocumentClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["endpoint"]), ConfigurationManager.AppSettings["authKey"], new ConnectionPolicy { EnableEndpointDiscovery = false });
                }
                return _client;
            }
        }

        protected static DocumentCollection Collection
        {
            get { return _collection; }
        }

        private static async Task ReadOrCreateCollection()
        {
           
            var collections = Client.CreateDocumentCollectionQuery(UriFactory.CreateDatabaseUri(DatabaseId))
                              .Where(col => col.Id == _collectionId).ToArray();
            if (collections.Any())
            {
                _collection = collections.First();
            }
            else
            {
                _collection = await Client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(_databaseId),
                        new DocumentCollection { Id = _collectionId },
                        new RequestOptions { OfferThroughput = 1000 });
            }
        }

        private static async Task ReadOrCreateDatabase()
        {
            var query = Client.CreateDatabaseQuery()
                            .Where(db => db.Id == _databaseId);

            var databases = query.ToArray();
            if (databases.Any())
            {
                _database = databases.First();
            }
            else
            {
                _database = await Client.CreateDatabaseAsync(new Database { Id = _databaseId });
            }
        }

    }
}
