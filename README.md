Run the following to create and install the tool:

```bash
dotnet pack -c 'Release'

// first installation
dotnet tool install -g 'dotnet-prepare'

// update
dotnet tool update -g 'dotnet-prepare'
```