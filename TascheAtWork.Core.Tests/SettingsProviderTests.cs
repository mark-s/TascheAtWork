using NUnit.Framework;
using FluentAssertions;
using TascheAtWork.Core.Infrastructure;
using TascheAtWork.Core.Services;

namespace TascheAtWork.Core.Tests
{

    [TestFixture]
    public class SettingsProviderTests
    {
        private ISettingsProvider _underTest;

        [SetUp]
        public void TestSetup()
        {
            _underTest = new SettingsProvider();
        }

        [Test]
        public void Save_GivenAccessCode_SavesOK()
        {
            // Arrange
            const string valueToSave = "Saved Text";

            // Act
            _underTest.Save(SettingsKey.AccessCode, valueToSave);
            var result = _underTest.Load(SettingsKey.AccessCode);

            // Assert
            result.Should().Be(valueToSave);
        }


        [Test]
        public void Save_GivenUserName_SavesOK()
        {
            // Arrange
            const string valueToSave = "Saved Text 12346";

            // Act
            _underTest.Save(SettingsKey.UserName, valueToSave);
            var result = _underTest.Load(SettingsKey.UserName);

            // Assert
            result.Should().Be(valueToSave);
        }

    }

}