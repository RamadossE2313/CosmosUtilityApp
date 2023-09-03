<h3>Cosmos Utility Application</h3>

<h5> Packages to Install For Cosmos:</h5>
 <ul>
   <li>Install Microsoft.Azure.Cosmos </li>
   <li>Install Microsoft.Extensions.Configuration (to support configuration)</li>
   <li>Install Microsoft.Extensions.Configuration.Json ( to support configuration to ad</li>
 </ul>
 <h5> Packages to Install to add app configuration file in the console application:</h5>
  <ul>
   <li>Install Microsoft.Extensions.Configuration</li>
   <li>Install Microsoft.Extensions.Configuration.Json</li>
 </ul>

 <h5>Steps to add app configuration file</h5>
 <ul>
   <li>
    Add appsettings.json file and select properties make copy to output directory as "Copy always"
    <span><img src="https://github.com/RamadossE2313/CosmosUtilityApp/blob/master/Images/Image1.png"/></span>
   </li>
   <li>
     IConfiguration Configuration = new ConfigurationBuilder()
     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
     .Build();
   </li>
 </ul>

<h5> Important Points</h5>

<ul>
 <li> PatchItemAsync method can be used for partial update the document to add/update/delete and etc </li>
 <li> PatchItemAsync method can be used in the object</li>
 <li> PatchItemAsync method can be used in the array only when we know the index position</li>
</ul>

<h4>Scenerios with examples</h4>
Json File: <a href ="https://github.com/RamadossE2313/CosmosUtilityApp/blob/master/Examples/Example1.json">Json File</a>
<ul>
 <li>
   Update the document property using PatchItemAsync method <a href ="https://github.com/RamadossE2313/CosmosUtilityApp/blob/master/Example1.cs">PatchItemAsync example</a>
 </li>
  <li>
   Update the document property without using PatchItemAsync method ( we don't know the index position to update) <a href ="https://github.com/RamadossE2313/CosmosUtilityApp/blob/master/Example1.cs">Without using PatchItemAsync method</a>
 </li>
</ul>
