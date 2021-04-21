1. Install the dotnet CLI if you haven't already.

2. Copy the entire EEP folder to your chosen directory, e.g. C:\.

3. Open Command Prompt as administrator, and navigate to the EEP folder e.g. 'cd C:\EEP'

4. type 'dotnet restore' and press enter, to restore .NET packages and dependencies.

5. type 'dotnet run' and press enter to run the application.

6. Open Postman or similar API testing tool to test the API endpoints below.

Endpoints:

GET metadata by movie ID:
	https://localhost:5001/metadata/{movieId}

POST metadata:
	https://localhost:5001/metadata
(include JSON structure for Metadata object in request body)

GET movie stats:
	https://localhost:5001/movies/stats


	

