using Raven.Client.Documents;
using System;

namespace Shop.Raven
{
    public class DocumentStoreHolder
    {
        
        private static readonly Lazy<IDocumentStore> LazyStore =
            new Lazy<IDocumentStore>(() =>
            {
               IDocumentStore store = new DocumentStore
               {
                   Urls = new[] { "http://127.0.0.1:8080/" },
                   Database = "shop"
               };

               store.Initialize();

               return store;
            });

        public static IDocumentStore Store => LazyStore.Value;
    }
}
