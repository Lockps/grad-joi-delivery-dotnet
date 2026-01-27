.PHONY: build run test

PROJECT_FILE := grad-joi-delivery-dotnet.csproj
TEST_PROJECT := ../grad-joi-delivery-dotnet.Tests/grad-joi-delivery-dotnet.Tests.csproj

build:
	dotnet build $(PROJECT_FILE)

run: build
	dotnet run --project $(PROJECT_FILE)

# Usage:
#   make test                    - Run all tests
#   make test TEST=SampleProblemTests  - Run tests in SampleProblemTests class
#   make test TEST="CalculateCost"     - Run tests containing "CalculateCost" in name
test: build
ifdef TEST
	dotnet test $(TEST_PROJECT) --filter "FullyQualifiedName~$(TEST)"
else
	dotnet test $(TEST_PROJECT)
endif