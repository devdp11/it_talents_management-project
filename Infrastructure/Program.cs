using IPVC.ESTG.ES2;

var action = args.Length > 0 ? args[0].ToLower() : "run";

// Check if Docker and Docker Compose are installed
if (Helpers.IsCommandAvailable("docker") && Helpers.IsCommandAvailable("docker-compose"))
{
    // Check if .env file exists, if not, create one from .env.example
    if (!File.Exists(".env"))
    {
        File.Copy(".env.example", ".env");
    }

    switch(action)
    {
        case "start":
            if(Helpers.AreContainersRunning())
            {
                Console.WriteLine("Containers are already running.");
            }
            else
            {
                // Run docker-compose build and up
                Helpers.ExecuteCommand("docker-compose build");
                Helpers.ExecuteCommand("docker-compose up -d");
            }
            break;

        case "stop":
            if(!Helpers.AreContainersRunning())
            {
                Console.WriteLine("No containers are running.");
            }
            else
            {
                // Stop running containers
                Helpers.ExecuteCommand("docker-compose down");
            }
            break;

        case "run":
            if(Helpers.AreContainersRunning())
            {
                Console.WriteLine("Containers are already running.");
            }
            else
            {
                // Run docker-compose build and up, outputting all logs
                Helpers.ExecuteCommand("docker-compose build");
                Helpers.ExecuteCommand("docker-compose up");
            }
            break;

        default:
            Console.WriteLine("Invalid action. Please specify 'start', 'stop', or 'run'.");
            break;
    }
}
else
{
    Console.WriteLine("Error: Docker and/or Docker Compose are not installed on this system.");
}
