# Blazor basic

[Azure Static Web Apps](https://docs.microsoft.com/azure/static-web-apps/overview) allows you to easily build [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) apps in minutes. Use this repo with the [Blazor quickstart](https://docs.microsoft.com/azure/static-web-apps/getting-started?tabs=blazor) to build and customize a new static site.

## CI/CD Pipeline

This repository includes a comprehensive CI/CD pipeline that ensures code quality and catches issues before deployment.

### Continuous Integration (CI)

The CI pipeline runs automatically on:
- Pushes to `main` and `development` branches
- Pull requests targeting `main` and `development` branches

#### What the CI Pipeline Does

1. **Build Validation**: Ensures the application builds successfully
2. **Unit Testing**: Runs all unit tests for C# code
3. **Code Coverage**: Generates test coverage reports
4. **HTML Linting**: Validates HTML files for best practices and standards
5. **CSS Linting**: Checks CSS files for style consistency and standards
6. **Deployment Validation**: Verifies the application can be published successfully

#### Test Coverage

The project includes comprehensive unit tests for:
- `PollService` class methods
- Error handling scenarios
- Data validation logic

#### Linting Configuration

- **HTML**: Uses HTMLHint with custom rules (see `.htmlhintrc`)
- **CSS**: Uses Stylelint with standard configuration (see `.stylelintrc.json`)

### Running Tests Locally

To run tests locally:

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Build the application
dotnet build

# Publish for deployment
dotnet publish Client/BlazorBasic.csproj -c Release
```

### Contributing

When contributing to this project:

1. Ensure all tests pass locally before submitting a PR
2. Add tests for new functionality
3. Follow the established coding standards
4. The CI pipeline will validate your changes automatically

The CI pipeline provides feedback directly in pull requests, making it easy to see the status of your changes.