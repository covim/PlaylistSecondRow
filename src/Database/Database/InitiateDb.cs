﻿using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class InitiateDb
    {

        public InitiateDb()
        {
            InitAndTestDb();
        }
        
        public void InitAndTestDb()
        {
            // Open database (or create if doesn't exist)
            var db = new LiteDatabase(@"C:\Temp\MyData.db");
            // Get a collection (or create, if doesn't exist)
            var col = db.GetCollection<Customer>("customers");

            // Create your new customer instance
            var customer = new Customer
            {
                Name = "John Doe",
                Phones = new string[] { "8000-0000", "9000-0000" },
                IsActive = true
            };

            // Insert new customer document (Id will be auto-incremented)
            col.Insert(customer);

            // Update a document inside a collection
            customer.Name = "Jane Doe";

            col.Update(customer);

            // Index document using document Name property
            col.EnsureIndex(x => x.Name);

            // Use LINQ to query documents (filter, sort, transform)
            var results = col.Query()
                .Where(x => x.Name.StartsWith("J"))
                .OrderBy(x => x.Name)
                .Select(x => new { x.Name, NameUpper = x.Name.ToUpper() })
                .Limit(10)
                .ToList();

            // Let's create an index in phone numbers (using expression). It's a multikey index
            col.EnsureIndex(x => x.Phones);

            // and now we can query phones
            var r = col.FindOne(x => x.Phones.Contains("8888-5555"));
        }
    }
}
