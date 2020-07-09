using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Photoboom.Data;

namespace PhotoBoomTests
{
    public class Tests
    {
        IConfiguration configuration;
        PhotoContext photoContext;
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
            return config;
        }
        [SetUp]
        public void Setup()
        {
            configuration = InitConfiguration();
            photoContext = new PhotoContext();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}