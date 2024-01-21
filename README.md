# Technologies

[.NET8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0), [Docker](https://www.docker.com/products/docker-desktop/)

# Running Images with Docker Compose

Navigate to the solution directory and open the docker-compose.yml file.

Set your CoinAPI key in the file:

```
CoinApi__Key=your-api-key
```

After setting the key, execute the following commands:

```
docker-compose -f docker-compose.yml build
docker-compose -f docker-compose.yml up -d
```
Now you can make requests to the API at http://localhost:5016/execute
