# WeCheer Image App

A serverless application that displays images and their descriptions in near real-time using AWS Lambda and Angular.

## Architecture

- Backend: .NET 8 AWS Lambda with API Gateway
- Frontend: Angular 17 with Angular Material
- Deployment: AWS Serverless (CloudFormation)

## Features

- Image event submission via REST API
- Real-time image display (5-second polling)
- Event count tracking for the last hour
- Swagger UI documentation
- CORS support
- Serverless architecture

## Project Structure

```
├── WeCheerImageApp.Api/          # Backend API (.NET 8)
│   ├── src/
│   │   └── WeCheerImageApp.Api/  # Main API project
│   └── test/                     # Test projects
└── wecheer-image-app-ui/         # Frontend (Angular)
```

## Prerequisites

- .NET 8 SDK
- Node.js (LTS version)
- Angular CLI
- AWS CLI
- AWS account with appropriate permissions

## Local Development

### Backend

```bash
cd WeCheerImageApp.Api/src/WeCheerImageApp.Api
dotnet run
```

### Frontend

```bash
cd wecheer-image-app-ui
npm install
ng serve
```

## Deployment

### Backend

1. Install AWS .NET Lambda tools:
```bash
dotnet tool install -g Amazon.Lambda.Tools
```

2. Deploy to AWS:
```bash
cd WeCheerImageApp.Api/src/WeCheerImageApp.Api
dotnet lambda deploy-serverless
```

### Frontend

Build and deploy to your preferred static hosting service:

```bash
cd wecheer-image-app-ui
ng build --configuration production
```

## API Documentation

Once deployed, Swagger UI is available at:
`https://[your-api-gateway-url]/swagger`

## License

MIT 