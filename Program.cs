using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Cosmos.Table;

//
//Quickstart for Cosmosdb Table API
//
namespace CosmosdbTableApiQuickstart
{
    class Program
    {
        //Connection String
        private static string storageConnectionString; //Get the connection string from azure portal

        //Storage account
        private static CloudStorageAccount storageAccount; //extract the storage account from the connection string

        //Table name
        private static string tableName = "SridharTable";

        public static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Begin Cosmosdb Table API Quickstart...\n");

                //get the connection string from the settings file 
                IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile("Settings.json").Build();
                storageConnectionString = configuration["StorageConnectionString"];
                tableName = configuration["TableName"];

                // Retrieve storage account information from connection string.
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);

                // Create a table client for interacting with the table service
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

                // Create a cloud table
                CloudTable table = tableClient.GetTableReference(tableName);
                await table.CreateIfNotExistsAsync();

                ////////////////////////
                //1. Insert row
                ////////////////////////
                Console.WriteLine("Insert Row");
                // Create an instance of a customer entity. See the Model\CustomerEntity.cs for a description of the entity.
                CustomerEntity customer_1 = new CustomerEntity("Kothalanka", "Sridhar","Sridhar@contoso.com","425-555-0101");

                // Create the InsertOrReplace table operation
                TableOperation insertOrMergeOperation_1 = TableOperation.InsertOrMerge(customer_1);

                // Execute the operation.
                TableResult result_1 = await table.ExecuteAsync(insertOrMergeOperation_1);
                CustomerEntity insertCustomer_1 = result_1.Result as CustomerEntity;

                // Get the request units consumed by the current operation. RequestCharge of a TableResult is only applied to Azure Cosmos DB
                if (result_1.RequestCharge.HasValue)
                {
                    Console.WriteLine("Request Charge of InsertOrMerge Operation: " + result_1.RequestCharge + "\n");
                }

                ////////////////////////
                //2. Update row
                ////////////////////////
                Console.WriteLine("Update Row");
                customer_1.PhoneNumber = "425-555-0105";
                TableOperation insertOrMergeOperation_2 = TableOperation.InsertOrMerge(customer_1);
                TableResult result_2 = await table.ExecuteAsync(insertOrMergeOperation_2);
                if (result_2.RequestCharge.HasValue)
                {
                    Console.WriteLine("Request Charge of InsertOrMerge Operation: " + result_2.RequestCharge + "\n");
                }

                ////////////////////////
                //3. Retrieve row
                ////////////////////////
                Console.WriteLine("Retrieve Row");
                TableOperation retrieveOperation_3 = TableOperation.Retrieve<CustomerEntity>("Kothalanka", "Sridhar");
                TableResult result_3 = await table.ExecuteAsync(retrieveOperation_3);
                CustomerEntity customer_3 = result_3.Result as CustomerEntity;
                if (customer_3 != null)
                {
                    Console.WriteLine("\t{0}\t{1}\t{2}\t{3}", customer_3.PartitionKey, customer_3.RowKey, customer_3.Email, customer_3.PhoneNumber);
                }
                if (result_3.RequestCharge.HasValue)
                {
                    Console.WriteLine("Request Charge of Retrieve Operation: " + result_3.RequestCharge + "\n");
                }

                ////////////////////////
                //4. Delete row
                ////////////////////////
                Console.WriteLine("Delete Row");
                CustomerEntity customer_4 = new CustomerEntity("Kothalanka_4", "Sridhar_4", "Sridhar_4@contoso.com", "425-555-0101");
                TableOperation insertOrMergeOperation_4 = TableOperation.InsertOrMerge(customer_4);
                await table.ExecuteAsync(insertOrMergeOperation_4);

                TableOperation deleteOperation_4 = TableOperation.Delete(customer_4);
                TableResult result_4 = await table.ExecuteAsync(deleteOperation_4);
                if (result_4.RequestCharge.HasValue)
                {
                    Console.WriteLine("Request Charge of Retrieve Operation: " + result_4.RequestCharge + "\n");
                }

                ////////////////////////
                //5. Delete table
                ////////////////////////
                //Delete the table
                //await table.DeleteIfExistsAsync()

            }
            catch (CosmosException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}", de.StatusCode, de);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }
            finally
            {
                Console.WriteLine("End of demo, press any key to exit.");
                Console.ReadKey();
            }

        }
    }
}
