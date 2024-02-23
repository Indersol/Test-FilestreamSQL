That's test of using FILESTREAM database with C# code. 

First you have to configure database to using FILESTREAM.
Later you have to create filestream database with filegrup.... You just find HOW-TO in official SQL documentation:
https://learn.microsoft.com/en-us/sql/relational-databases/blob/create-a-filestream-enabled-database?view=sql-server-ver16

And then use ef to create db from model. There is connection exaple of EntityFramework and ADO.NET to save files in db. 
Maybe someone need that bc there is hard to find universal solution of this problem. 
