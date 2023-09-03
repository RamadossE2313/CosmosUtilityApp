<h3>Cosmos Utility Application</h3>

<h5> Packages to install For Cosmos:</h5>
 <ul>
   <li>install Microsoft.Azure.Cosmos </li>
   <li>install Microsoft.Extensions.Configuration (to support configuration)</li>
   <li>install Microsoft.Extensions.Configuration.Json ( to support configuration to ad</li>
 </ul>
 <h5> Packages to install to add app configuration file in the console application:</h5>
  <ul>
   <li>install Microsoft.Extensions.Configuration</li>
   <li>install Microsoft.Extensions.Configuration.Json</li>
 </ul>

 <h5>Steps to add app configuration file</h5>
 <ul>
   <li>
    Add appsettings.json file and select properties make copy to output directory as "Copy always"
   </li>
   <li>
     IConfiguration Configuration = new ConfigurationBuilder()
     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
     .Build();
   </li>
 </ul>
