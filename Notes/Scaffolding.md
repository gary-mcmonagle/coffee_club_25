This is before a single line of code has been written! 

Some local things to set up


1. Azure MCP: https://github.com/Azure/azure-mcp
This runs via stdio which by the looks of it just uses AZ Cli on your machine


2. Github MCP: https://github.com/github/github-mcp-server 
Authenticated and used to create repo - I guess thats cool

Have created local git repo - Noe lets commit these initial files and see if we can push to that repo via chat 


All worked - Pretty cool

3. Lets get aspire - I have never used this before so will be running through the steps manually

First lets try get dotnet 9 via mcp - Its using brew - Pretty cool! 

Lets first think what we want in aspire - A BE, A FE (blazor probs) and a DB

And lets get the tooling - The aspire cli


- Basic FE / BE scaffolded lets push this 
- And can we deploy to azure? Requires updating azd...
- yes - takes a bit but deployable - deleting again cause need to check out pricing 


- Now time to add a DB! 

The MYSQl one works simply which is cool, adding a schema though looks a little more tricky, might instead opt for EF First approach

- Looking at the naming convention it may be time to actually rescaffold in Pascal 
- Warning for mysql persistence when deploying to acs will pivot to cosmos and the emulator locally - Took some time to get the port forwarding working but seems okay 

- Crud endpoints added - Looks okay 