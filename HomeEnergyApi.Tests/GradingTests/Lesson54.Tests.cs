using System.Reflection;

public class LessonTests
{
    private static string Lesson54FilePath = @"../../../Lesson54Tests/RateLimitingMiddlewareTest.cs";
    private string Lesson54Content = File.ReadAllText(Lesson54FilePath);
    private readonly string[] _requiredTestMethods =
    {
        "ShouldCallNextMiddleware_WhenRateLimitNotExceeded",
        "ShouldReturn429_WhenRateLimitExceeded"
    };

    [Fact]
    public void Lesson50MathTestsShouldPass()
    {
        var testAssembly = Assembly.GetExecutingAssembly();
        var RateLimitingServiceTests = testAssembly.GetTypes()
            .FirstOrDefault(t => t.Name == "DecryptionAuditServiceTest");

        Assert.NotNull(RateLimitingServiceTests);

        var instance = Activator.CreateInstance(RateLimitingServiceTests);

        foreach (var requiredMethodName in _requiredTestMethods)
        {
            var testMethod = RateLimitingServiceTests.GetMethod(requiredMethodName);
            Assert.True(testMethod != null, $"Method {requiredMethodName} not found in DecryptionAuditServiceTest class");

            try
            {
                testMethod.Invoke(instance, null);
            }
            catch (TargetInvocationException ex)
            {
                Assert.Fail($"Test {requiredMethodName} failed: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }


}