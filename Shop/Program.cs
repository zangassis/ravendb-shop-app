using Raven.Client.Documents.Session;
using Shop.Model;
using Shop.Raven;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop
{
    class Program
    {
        static void Main(string[] args)
        {
            //CreateProduct("Apple", 9.10);
            //CreateProduct("Tea", 11.22);
            //CreateProduct("Banana", 8.99);
            //CreateProduct("Coffee", 27.97);
            //CreateProduct("Carrot", 1.20);

            //GetProduct("products/36-A");

            //GetAllProducts();

            // GetProducts(1, 3);

            //CreateCart("john@deo.com");

            //AddProductToCart("john@deo.com", "products/35-A", 3);
        }


        //Create
        static void CreateProduct(string name, double price)
        {
            Product product = new()
            {
                Name = name,
                Price = price
            };

            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                session.Store(product);
                session.SaveChanges();
            }
        }

        //GET
        static void GetProduct(string id)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                Product product = session.Load<Product>(id);

                Console.WriteLine($"Product: {product.Name} \t\t Price: {product.Price}");
            }
        }

        static void GetAllProducts()
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                List<Product> productList = session.Query<Product>().ToList();

                foreach (var product in productList)
                {
                    Console.WriteLine($"Product: {product.Name} \t\t Price: {product.Price}");
                }
            }
        }

        //GET
        static void GetProducts(int pageIndex, int pageSize)
        {
            int skip = (pageIndex - 1) * pageSize;
            int take = pageSize;

            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                List<Product> page = session.Query<Product>()
                    .Statistics(out QueryStatistics stats)
                    .Skip(skip)
                    .Take(take)
                    .ToList();

                page = session.Query<Product>()
                    .Statistics(out QueryStatistics stats1)
                    .Skip(skip)
                    .Take(take)
                    .ToList();

                Console.WriteLine($"Showing results {skip | 1} to {skip | pageSize} of total {stats.TotalResults}");

                foreach (Product product in page)
                {
                    Console.WriteLine($"Product: {product.Name} \t\t Price: {product.Price}");
                }

                Console.WriteLine($"This was produced in {stats.DurationInMs} ms");
            }
        }

        //CREATE
        static void CreateCart(string customer)
        {
            Cart cart = new()
            {
                Customer = customer
            };

            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                session.Store(cart);
                session.SaveChanges();
            }
        }

        //UPDATE
        static void AddProductToCart(string customer, string productId, int quantity)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                Cart cart = session.Query<Cart>().Single(x => x.Customer == customer);

                Product product = session.Load<Product>(productId);

                cart.Lines.Add(new CartLine
                {
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    Quantity = quantity
                });

                session.SaveChanges();
            }
        }
    }
}
