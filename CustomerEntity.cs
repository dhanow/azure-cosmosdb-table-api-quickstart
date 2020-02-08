namespace CosmosdbTableApiQuickstart
{
    using Microsoft.Azure.Cosmos.Table;
    public class CustomerEntity : TableEntity
    {
        public CustomerEntity()
        {
        }

        public CustomerEntity(string lastName, string firstName)
        {
            PartitionKey = lastName;
            RowKey = firstName;
        }

        public CustomerEntity(string lastName, string firstName, string email, string phoneNumber)
        {
            PartitionKey = lastName;
            RowKey = firstName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}