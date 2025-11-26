# 054 Async Testing
## Lecture

[![# Async Testing (Part 1)](https://img.youtube.com/vi/6XL7YSYtQos/0.jpg)](https://www.youtube.com/watch?v=6XL7YSYtQos)
[![# Async Testing (Part 2)](https://img.youtube.com/vi/XKtsefeE2IQ/0.jpg)](https://www.youtube.com/watch?v=XKtsefeE2IQ)

## Instructions

In this lesson you will be testing the method contained in `HomeEnergyApi/Services/RateLimitingService.cs` where a new method `IsWeekend()` has been added. You will need to create test stubs to test this method, in addition to adding tests to `HomeEnergyApi.Tests/Lesson54Tests/RateLimitingService.Tests.cs`. You should NOT change any test files inside of the `HomeEnergyApi.Tests/GradingTests`, these are used to grade your assignment.

- In `HomeEnergyApi.Tests/Stubs/StubLogger.cs`
    - Create a new class `StubLogger<T>` implementing `ILogger<T>
        - Create a new public property `LoggedInfoMessages` of type `List<string>` and initialize it as an empty list of strings
        - Create a new public method `Log<TState>()` with no return type
            - Give `Log<TState>()` the following argument names / types
                - `logLevel` / `LogLevel` 
                - `eventId` / `EventId`
                - `state` / `TState`
                - `exception` / `Exception` 
                - `formatter` /  `Func<TState, Exception, string>`
            - If the argument `logLevel` is equal to the value of `LogLevel.Information`
                - Call `Add()` on the property `LoggedInfoMessages` passing in `formatter(state, exception)`
        - Create a new public method `BeginScope<TState>` with the return type `IDisposable?` and specify it as `where TState : notnull` and give it an argument of type `TState`
            - Throw a new `NotImplementedException()`
        - Create a new public method `IsEnabled` with the return type `bool` and an argument of type `LogLevel`
            - Throw a new `NotImplementedException()`
- In `HomeEnergyApi.Tests/Lesson54Tests/DecryptionAuditService.Tests.cs`
    - Create a new public class `DecryptionAuditServiceTest`
        - Create a new private property `stubLogger` of type `StubLogger<DecryptionAuditService>`
        - Create a new private property `service` of type `DecryptionAuditService`
        - In the constructor...
            - Set the value of `stubLogger` to a new instance of `StubLogger<DecryptionAuditService>`
            - Set the value of `service` to a new `DecryptionAuditService` and pass `stubLogger` into it's constructor
        - Create a new public void method `ShouldLog_WhenValueDecrypted` with the `[Fact]` attribute
            - Initialize two string variables for the test cipher and plain text
            - Call `OnValueDecrypted()` on `service` passing in your string variables
            - Assert that the value of the logged message is equal to the expected value being logged in `HomeEnergyApi/Services/DecryptionAuditService.cs`


## Additional Information

- Some Models may have changed for this lesson from the last, as always all code in the lesson repository is available to view
- Along with `using` statements being added, any packages needed for the assignment have been pre-installed for you, however in the future you may need to add these yourself

## Building toward CSTA Standards:

## Resources
- https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/reflection-and-attributes/

Copyright &copy; 2025 Knight Moves. All Rights Reserved.
