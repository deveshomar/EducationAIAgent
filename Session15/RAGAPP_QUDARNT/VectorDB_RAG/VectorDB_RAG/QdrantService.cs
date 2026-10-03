using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using Qdrant.Client;
using Qdrant.Client.Grpc;


namespace VectorDB_RAG
{


    public class QdrantService
    {
        private readonly QdrantClient _client;

        private readonly string _collectionName;

        public QdrantService(string host,
                             int port,
                             string collection)
        {
            _client = new QdrantClient(host, port);

            _collectionName = collection;
        }
        public async Task CreateCollectionAsync()
        {
            var exists =
                await _client.CollectionExistsAsync(_collectionName);

            if (exists)
                return;

            await _client.CreateCollectionAsync(

                collectionName: _collectionName,

                vectorsConfig: new VectorParams
                {
                    Size = 1536,

                    Distance = Distance.Cosine
                });

            Console.WriteLine("Collection Created");
        }
        public async Task SaveAsync(
    int id,
    float[] embedding,
    string text)
        {
            var point = new PointStruct
            {
                Id = (ulong)id,

                Vectors = embedding,

                Payload =
        {
            ["text"] = text
        }
            };

            await _client.UpsertAsync(
                _collectionName,
                new List<PointStruct>
                {
            point
                });
        }

        public async Task<List<ScoredPoint>> SearchAsync(
    float[] embedding)
        {
            var result =
                await _client.SearchAsync(

                    collectionName: _collectionName,

                    vector: embedding,

                    limit: 3);

            return result.ToList();
        }
    }
}