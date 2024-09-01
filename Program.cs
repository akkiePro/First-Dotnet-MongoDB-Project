using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBDemo
{
    class Program
    {
        private static IMongoCollection<Book> _booksCollection;

        static async Task Main(string[] args)
        {
            // Connection string
            var connectionString = "mongodb://localhost:27017";

            // Database name
            var databaseName = "LibraryDb";

            // Collection name
            var collectionName = "Books";

            // Create MongoClient
            var client = new MongoClient(connectionString);

            // Get database
            var database = client.GetDatabase(databaseName);

            // Get collection
            _booksCollection = database.GetCollection<Book>(collectionName);

            Console.WriteLine("Connected to MongoDB!");

            // Perform CRUD operations
            //await CreateBookAsync();
            await ReadBooksAsync();
            GetLastInsertedBookIdAsync();
            //await UpdateBookAsync();
            //await DeleteBookAsync();
        }

        private static async Task<string> GetLastInsertedBookIdAsync()
        {
            // Find the last inserted book by sorting by _id in descending order
            var filter = Builders<Book>.Filter.Empty; // No filter, get all documents
            var sort = Builders<Book>.Sort.Descending("_id"); // Sort by _id in descending order

            // Get the most recent book
            var lastInsertedBook = await _booksCollection
                .Find(filter)
                .Sort(sort)
                .FirstOrDefaultAsync();

            // Return the Id of the last inserted book
            return lastInsertedBook?.Id;
        }

        private static async Task CreateBookAsync()
        {
            var book = new Book
            {
                Title = "The Pragmatic Programmer",
                Author = "Andrew Hunt and David Thomas",
                Price = 30.99,
                PublishedDate = new DateTime(1999, 10, 20)
            };

            await _booksCollection.InsertOneAsync(book);
            Console.WriteLine($"Book inserted with Id: {book.Id}");
        }

        private static async Task ReadBooksAsync()
        {
            var books = await _booksCollection.Find(new BsonDocument()).ToListAsync();

            Console.WriteLine("Books in collection:" + books.Count);
            Console.WriteLine("last id => " + await GetLastInsertedBookIdAsync());
            foreach (var book in books)
            {
                Console.WriteLine($"- {book.Id}. {book.Title} by {book.Author}, Price: {book.Price}");
            }
        }

        private static async Task UpdateBookAsync()
        {
            var filter = Builders<Book>.Filter.Eq("Title", "The Pragmatic Programmer");
            var update = Builders<Book>.Update.Set("Price", 25.99);

            var result = await _booksCollection.UpdateOneAsync(filter, update);

            Console.WriteLine($"{result.ModifiedCount} document(s) updated.");
        }

        private static async Task DeleteBookAsync()
        {
            var filter = Builders<Book>.Filter.Eq("Title", "The Pragmatic Programmer");

            var result = await _booksCollection.DeleteOneAsync(filter);

            Console.WriteLine($"{result.DeletedCount} document(s) deleted.");
        }
    }
}
