# AGENTS.md - Developer Guidelines for NoLoops

## Project Overview

NoLoops is a C# library providing extension methods for `IEnumerable<T>` and `IAsyncEnumerable<T>` that fill gaps in standard LINQ. The entire library lives in a single source file (`src/NoLoops/NoLoops.cs`).

## Tech Stack

- **Language**: C# (.NET 9.0)
- **Testing**: NUnit 4.x, FsCheck (property-based testing), coverlet (code coverage)
- **Documentation**: DocFX
- **Package**: Distributed via NuGet

## Build Commands

```bash
# Build the entire solution
dotnet build -c Release

# Run all tests
dotnet test -c Release

# Run tests with coverage
dotnet test -c Release --collect:"XPlat Code Coverage"
```

## Code Style Guidelines

### General Principles

- **Single source file**: the main library code resides in `src/NoLoops/NoLoops.cs`. Keep it self-contained.
- **Collection expressions**: use `[]` syntax (e.g., `var list = [1, 2, 3];`).
- **Primary constructors**: use primary constructors for structs and records.
- **Consistent style**: new code follows the style of existing code.

### Method Design

- **Extension methods**: Use `this` keyword as first parameter
- **Overloads**: Provide multiple overloads for flexibility (e.g., with/without equality comparer)
- **Return types**: Prefer `IEnumerable<T>` for lazy evaluation; avoid `IList<T>` unless necessary
- **Yield return**: Use for lazy enumeration to avoid materializing collections

### Error Handling

- Use `InvalidOperationException` for invalid state in enumerators
- Throw argument exceptions (`ArgumentNullException`, `ArgumentException`) for invalid inputs
- Do not catch exceptions unless specifically required by the API contract

### LINQ Usage

- Prefer standard LINQ methods when available
- Use explicit enumerators with `GetEnumerator()`/`MoveNext()` for custom logic
- Avoid side effects in LINQ projections; use the `Do` extension for that purpose

### Code Organization (NoLoops.cs)

The main file follows this structure:
1. MIT License header
2. Using statements
3. Namespace declaration
4. Public static class with extension methods
5. Helper types (nested private classes)
6. Helper static classes (file-local)

Example ordering:
```
NoLoops class (public API)
├── Do() overloads
├── GroupConsecutivesBy() overloads (many)
├── Private nested Group class
└── Private Identity helper

Enumerable (file static class)
└── SideEffect() helper

Enumerator (file static class)
└── Empty() helper
```

## Testing Guidelines

### Test Structure

- Test project: `tests/NoLoops.Tests`
- Test class naming: `<Feature>Tests` (e.g., `DoTests`, `GroupConsecutivesByTests`)
- Test method naming: Descriptive English sentences (e.g., `YieldsTheInputSequenceUnaltered`)
- Use FsCheck for property-based testing with `Prop.ForAll().QuickCheck()`
- Use expression-bodied members for simple test methods

### Test Example Pattern

```csharp
[Test]
public void DescriptionOfBehavior()
    => Prop.ForAll(Arbitrary.RandomSequence, sequence =>
    {
        // Arrange
        List<T> sideEffects = [];

        // Act & Assert
        Assert.That(sequence.Do(sideEffects.Add), Is.EqualTo(sequence));
    }).QuickCheck();

private static class Arbitrary
{
    public static Arbitrary<int[]> RandomSequence
        => Gen.Choose(1, 10).ArrayOf().ToArbitrary();
}
```

### Test Constraints

Use NUnit constraints:
- `Is.EqualTo(expected)`
- `Is.Empty`
- `Has.All.Matches(predicate)`
- Custom `PredicateConstraint` for complex assertions

## Adding New Features

1. Add XML documentation with `<summary>`, `<typeparam>`, `<param>`, `<returns>`, and `<remarks>` sections
2. Provide multiple overloads for common use cases
3. Add corresponding property-based tests using FsCheck
4. Ensure lazy evaluation where appropriate
5. Follow the existing code organization pattern

## Git Conventions

- **Commits**: Use clear, descriptive commit messages
- **Branches**: Feature branches are acceptable; PRs welcome
- **CI**: GitHub Actions run on push to main for publishing NuGet and docs

## References

- [NuGet Package](https://www.nuget.org/packages/NoLoops/)
- [Documentation](https://rucamzu.github.io/NoLoops/)
- [Source Code](https://github.com/rucamzu/NoLoops)
