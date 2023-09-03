using CosmosUtilityApp;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

// ReadMe file content
// install Microsoft.Azure.Cosmos package to access the cosmos
// To add app settings jons file install below packages
// install Microsoft.Extensions.Configuration
// install Microsoft.Extensions.Configuration.Json  
// add appsettings.json file and make copy to output directory as "Copy always"

// This is an example of you want to update an property value in the array
// we can't use patch in the array item when we don't know the index position

IConfiguration Configuration = new ConfigurationBuilder()
   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
   .Build();

var connectionString = Configuration.GetConnectionString("cosmosDbConnection");
using CosmosClient cosmosClient = new CosmosClient(connectionString);
var query = "select * from c";
QueryDefinition queryDefinition = new QueryDefinition(query);
var container = cosmosClient.GetContainer(CosmosAccountInformation.Databasename, CosmosAccountInformation.Containername);

#region when we know the index position, to change the propery value , then we can use patch operation or if it's object type we can use it directly
var iterator = container.GetItemQueryIterator<IItem>(queryDefinition);

foreach (var item in await iterator.ReadNextAsync())
{
    var patchOperations = new[] {
        PatchOperation.Replace("/departments/0/subdepartments/0/divisions/0/name", "division-name-1-new")
    };

    await container.PatchItemAsync<IItem>(item.id, new PartitionKey(item.pk), patchOperations);
}
#endregion

#region when we don't know the index position, then we have to loop item and needs to update
//var iterator = container.GetItemQueryIterator<EmployeeRootObject>(queryDefinition);
//foreach (var item in await iterator.ReadNextAsync())
//{
//    bool itemUpdated = false;

//    foreach (var department in item.departments)
//    {
//        foreach (var subdepartment in department.subdepartments)
//        {
//            foreach (var division in subdepartment.divisions)
//            {
//                if(!string.IsNullOrEmpty(division.name) && division.name != "division-name-2")
//                {
//                    continue;
//                }
//                else
//                {
//                    if(!itemUpdated)
//                    {
//                        itemUpdated = true;
//                    }
//                    division.name = CosmosAccountInformation.ReplaceValue;
//                }
//            }
//        }
//    }
//    if (itemUpdated)
//    {
//        var response = await container.ReplaceItemAsync(item, item.id, new PartitionKey(item.pk));
//        if(response.StatusCode == System.Net.HttpStatusCode.OK)
//        {
//            itemUpdated = false;
//        }
//    }
//}

#endregion

Console.WriteLine("data updated");

public class EmployeeRootObject : IItem
{
    public string? name { get; set; }
    public Department[]? departments { get; set; }
    public string? type { get; set; }
}

public class Department
{
    public int id { get; set; }
    public string? name { get; set; }
    public Subdepartment[]? subdepartments { get; set; }
}

public class Subdepartment
{
    public int id { get; set; }
    public string? name { get; set; }
    public Division[]? divisions { get; set; }
}

public class Division
{
    public int id { get; set; }
    public string? name { get; set; }
}