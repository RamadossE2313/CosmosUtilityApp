using CosmosUtilityApp;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;


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
var items = await iterator.ReadNextAsync();

// using parallel foreach to update the documents
try
{
    await Task.Run(() =>
    {
        Parallel.ForEach(items,
    new ParallelOptions
    {
        // we have restricted to run the parrelle thread to use/run 4 at a time, so that we will not complete all the computer power for this
        MaxDegreeOfParallelism = 4
    }, async (item) =>
    {
        var patchOperations = new[] {
        PatchOperation.Replace("/departments/0/subdepartments/0/divisions/0/name", "division-name-new")
    };

        await container.PatchItemAsync<IItem>(item.id, new PartitionKey(item.pk), patchOperations);

    });
    });

    // you can decide the thread count based on ProcessorCount
    await Parallel.ForEachAsync(items,
    new ParallelOptions { MaxDegreeOfParallelism = (Environment.ProcessorCount / 2) },
    async (item, CancellationToken) =>
    {
        var patchOperations = new[] {
        PatchOperation.Replace("/departments/0/subdepartments/0/divisions/0/name", "division-name-mddified-2")
        };
        await container.PatchItemAsync<IItem>(item.id, new PartitionKey(item.pk), patchOperations);
    });
}
catch (Exception ex)
{
    throw;
}


//using for loop to update the documents
//foreach (var item in items)
//    {
//        var patchOperations = new[] {
//        PatchOperation.Replace("/departments/0/subdepartments/0/divisions/0/name", "division-name-1-new")
//    };

//        await container.PatchItemAsync<IItem>(item.id, new PartitionKey(item.pk), patchOperations);
//    }
#endregion

#region when we don't know the index position, then we have to loop item and needs to update
//var iterator = container.GetItemQueryIterator<EmployeeRootObject>(queryDefinition);
//var items = await iterator.ReadNextAsync();

// using parallel foreach to update the documents 
//try
//{
//    Parallel.ForEach(items, new ParallelOptions { MaxDegreeOfParallelism = 4 }, async (item) =>
//    {
//        bool itemUpdated = false;

//        foreach (var department in item.departments)
//        {
//            foreach (var subdepartment in department.subdepartments)
//            {
//                foreach (var division in subdepartment.divisions)
//                {
//                    if (!string.IsNullOrEmpty(division.name) && division.name != "division-name-new")
//                    {
//                        continue;
//                    }
//                    else
//                    {
//                        if (!itemUpdated)
//                        {
//                            itemUpdated = true;
//                        }
//                        division.name = CosmosAccountInformation.ReplaceValue;
//                    }
//                }
//            }
//        }
//        if (itemUpdated)
//        {
//            var response = await container.ReplaceItemAsync(item, item.id, new PartitionKey(item.pk));
//            if (response.StatusCode == System.Net.HttpStatusCode.OK)
//            {
//                itemUpdated = false;
//            }
//        }
//    });
//}
//catch (Exception ex)
//{

//    throw;
//}

// using for loop to update the documents
//foreach (var item in items)
//{
//    bool itemUpdated = false;

//    foreach (var department in item.departments)
//    {
//        foreach (var subdepartment in department.subdepartments)
//        {
//            foreach (var division in subdepartment.divisions)
//            {
//                if (!string.IsNullOrEmpty(division.name) && division.name != "division-name-2")
//                {
//                    continue;
//                }
//                else
//                {
//                    if (!itemUpdated)
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
//        if (response.StatusCode == System.Net.HttpStatusCode.OK)
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