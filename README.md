# Sportradar

Coding exercise.

## Table of Contents

- [Introduction](#introduction)
- [Requirements](#requirements)
- [Getting Started](#getting-started)
- [External NuGet Packages Used](#external-nuget-packages)
- [Implementation Notes](#implementation-notes)

## Introduction

Implementation of the Live Football World Cup Score Board.
Solution consists of 2 projects: C# code library, which can be found in the /src directory and a unit tests project, located in /test directory.

## Requirements

- [.NET SDK](https://dotnet.microsoft.com/download) 8.0 or higher

## Getting Started

To run the unit tests, follow these steps:

1. Clone this repository to your local machine.
2. Navigate to the `test` directory.
3. Open a terminal or command prompt.
4. Run the following command:

```bash
dotnet test
```

## External NuGet Packages Used

- [FluentAssertions](https://www.nuget.org/packages/FluentAssertions/)
  To make it easier to write unit test assertions.
- [XUnit](https://www.nuget.org/packages/xunit/)
  As a unit testing framework of choice.
- [FluentResults](https://www.nuget.org/packages/FluentResults/)
  As a trade-off solution to handle validation concerns without introducing additional complexity.

## Implementation Notes

Intentionally didn't add validations for the following cases:

- Providing a match score that is greater than N, where N is a physical limit in the domain.
- Providing a team name of unreasonably high length.
