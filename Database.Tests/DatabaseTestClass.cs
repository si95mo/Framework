using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Tests
{
    [TestFixture]
    public class DatabaseTestClass
    {
        [OneTimeSetUp]
        public async Task Setup()
        {
            await Database.Initialize(@"Server=localhost\SQLEXPRESS;Database=dummy;Trusted_Connection=True;MultipleActiveResultSets=True");
            Database.IsConnectionOpen.Should().BeTrue();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Database.Close();
        }

        [Test]
        public async Task TestSelect()
        {
            SqlDataReader reader = await Database.Select("*", "DummyTable");
            reader.FieldCount.Should().NotBe(0);

            string key, description, timestamp, id, flag;
            int counter = 0;
            while(reader.Read())
            {
                key = reader["key"].ToString();
                description = reader["description"].ToString();
                timestamp = reader["timestamp"].ToString();
                id = reader["id"].ToString();
                flag = reader["flag"].ToString();

                key.Should().NotBeNullOrEmpty();
                description.Should().NotBeNullOrEmpty();
                timestamp.Should().NotBeNullOrEmpty();
                id.Should().NotBeNullOrEmpty();
                flag.Should().NotBeNullOrEmpty();

                counter++;
            }

            counter.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task TestInsertIntoAndQuery()
        {
            // First add
            bool recordAdded = await Database.InsertInto(
                "dbo.DummyTable",
                "id, description, timestamp, flag",
                new (string Name, object Value)[]
                {
                    ("@id", 1),
                    ("@description", "This record has been added for testing!"),
                    ("@timestamp", DateTime.Now),
                    ("@flag", true)
                }
            );
            recordAdded.Should().BeTrue();

            // Then delete, if the insert into worked,
            // at least one row should be stored in the db
            int deletedRows = await Database.Query("DELETE FROM DummyTable");
            deletedRows.Should().NotBe(0);

            // Special query test
            int rows = await Database.Query("DBCC CHECKIDENT (DummyTable, RESEED, 0)");
            rows.Should().Be(-1); // Default value

            // Leave the DB with at least one record
            recordAdded = await Database.InsertInto(
                "dbo.DummyTable",
                "id, description, timestamp, flag",
                new (string Name, object Value)[]
                {
                    ("@id", 1),
                    ("@description", "This record has been added for testing!"),
                    ("@timestamp", DateTime.Now),
                    ("@flag", true)
                }
            );
            recordAdded.Should().BeTrue();
        }
    }
}
