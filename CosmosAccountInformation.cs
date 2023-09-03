namespace CosmosUtilityApp
{
    public class CosmosAccountInformation
    {
        public static string? Databasename { get; set; } = "employee-db";
        public static string? Containername { get; set; } = "employees";
        public static string? Propertyname { get; set; } = "/departments/subdepartments/divisions[0].name";
        public static string ReplaceValue { get; set; } = "division-name-4";
    }

    public class IItem
    {
        public string? id { get; set; }
        public string? pk { get; set; }
    }
    

}
