using System.Reflection;

public class LessonTests
{
    private static string Lesson53FilePath = @"../../../Lesson53Tests/DecryptionAuditService.Tests.cs";
    private string Lesson53Content = File.ReadAllText(Lesson53FilePath);
    private readonly string[] _requiredTestMethods =
    {
        "ShouldLog_WhenValueDecrypted",
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