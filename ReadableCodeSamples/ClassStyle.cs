using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReadableCodeSamples
{
    [TestClass]
    public class UserSettingsUpdaterTests
    {
        private const string TARGET_FILE_NAME = "BobUserSettings.cfg";

        private string _userName;
        private UserSettings _userSettings;
        private string _userOffice;
        private string _templateForUserOffice;

        private readonly FakeTemplateReader _templateReader = new FakeTemplateReader();
        private readonly FakeUserSettingReader _userSettingsReader = new FakeUserSettingReader();
        private readonly UserSettingsFileManager _subject;

        public UserSettingsUpdaterTests()
        {
            _subject = new UserSettingsFileManager(_templateReader, _userSettingsReader, new TemplateUtility());
        }
        
        [TestMethod]
        public void SettingsFileUpdaterReplacesToken()
        {
            Given_no_file_exists_for(TARGET_FILE_NAME);
            and_the_target_user_is("Bob");
            and_the_user_setting_is(new UserSettings { FontSize = 16 });
            and_the_target_users_office_is("GA");
            and_the_template_for_the_office_is("font-size:{{FontSize}}");

            When_user_setting_file_is_updated();

            Then_the_resulting_file_should_contain("font-size:16");
        }

        private void Given_no_file_exists_for(string tARGET_FILE_NAME)
        {
            if (File.Exists(TARGET_FILE_NAME))
            {
                File.Delete(TARGET_FILE_NAME);
            }
            Assert.IsFalse(File.Exists(TARGET_FILE_NAME));
        }

        private void and_the_target_user_is(string userName)
        {
            _userName = userName;
        }

        private void and_the_user_setting_is(UserSettings userSettings)
        {
            _userSettings = userSettings;
            _userSettingsReader.Returns(userSettings);
        }

        private void and_the_target_users_office_is(string userOffice)
        {
            _userOffice = userOffice;
        }

        private void and_the_template_for_the_office_is(string template)
        {
            _templateForUserOffice = template;
            _templateReader.Resturns(template);
        }

        private void When_user_setting_file_is_updated()
        {
            _subject.UpdateUserSettingsFile(_userName, _userOffice, TARGET_FILE_NAME);
        }
        
        private void Then_the_resulting_file_should_contain(string expectedTextInFile)
        {
            string[] resultingFileLines = File.ReadAllLines(TARGET_FILE_NAME);
            Assert.AreEqual(expectedTextInFile, resultingFileLines[0]);
        }

        [TestInitialize]
        [TestCleanup]
        public void CleanupFile()
        {
            if(File.Exists(TARGET_FILE_NAME))
            {
                File.Delete(TARGET_FILE_NAME);
            }
            Assert.IsFalse(File.Exists(TARGET_FILE_NAME));
        }
    }

    [TestClass]
    public class TemplateUtilityTests
    {
        IUserSettingsTemplateUtility _subject = new TemplateUtility();

        [TestMethod]
        public void TemplateUtilityReplacesToken()
        {
            var userSettingValues = new[] { new KeyValuePair<string, string>("FontSize", "6") };
            string templateText = "{{FontSize}}";

            string result = _subject.ApplyUserSettingsToTemplate(userSettingValues, templateText);

            Assert.AreEqual("6", result);
        }
    }

    public class UserSettingsFileManager
    {
        ITemplateReader _tempateLoader;
        IUserSettingsReader _userSettingsReader;
        IUserSettingsTemplateUtility _settingsTemplateUtility;

        public UserSettingsFileManager(ITemplateReader tempateLoader,
        IUserSettingsReader userSettingsReader,
        IUserSettingsTemplateUtility settingsTemplateUtility)
        {
            _tempateLoader = tempateLoader;
            _userSettingsReader = userSettingsReader;
            _settingsTemplateUtility = settingsTemplateUtility;
        }


        public void UpdateUserSettingsFile(string userName, string settingsTemplateId, string saveSettingsFileAs)
        {
            var userSettings = GetSettingValuesForUser(userName);
            var settingTemplate = GetSettingsTemplate(settingsTemplateId);
            var userSettingsFileContents = ApplyUserSettingsToTemplate(userSettings, settingTemplate);
            SaveUserSettings(userSettingsFileContents, saveSettingsFileAs);
        }

        private string GetSettingsTemplate(string settingTemplateId)
        {
            return _tempateLoader.GetTemplateAsString(settingTemplateId);
        }

        private UserSettings GetSettingValuesForUser(string userName)
        {
            return _userSettingsReader.GetSettingsForUser(userName);
        }

        private string ApplyUserSettingsToTemplate(UserSettings userSettings, string settingTemplate)
        {
            var settingValuesForTemplate = ConvertUserSettingsForTemplate(userSettings);
            return _settingsTemplateUtility.ApplyUserSettingsToTemplate(settingValuesForTemplate, settingTemplate);
        }

        private void SaveUserSettings(string userSettings, string saveSettingsFileAs)
        {
            byte[] userSettingsForSaving = ConvertUserSettingsForFileSave(userSettings);
            File.WriteAllBytes(saveSettingsFileAs, userSettingsForSaving);
        }

        private static IEnumerable<KeyValuePair<string, string>> ConvertUserSettingsForTemplate(UserSettings userSettings)
        {
            return userSettings.ToKeyValuePairs();
        }

        private byte[] ConvertUserSettingsForFileSave(string userSettings)
        {
            return userSettings.ToByteArray();
        }
    }

    public interface IUserSettingsTemplateUtility
    {
        string ApplyUserSettingsToTemplate(IEnumerable<KeyValuePair<string, string>> userSettings, string settingTemplate);
    }

    public interface ITemplateReader
    {
        string GetTemplateAsString(string settingTemplateId);
    }

    public interface IUserSettingsReader
    {
        UserSettings GetSettingsForUser(string userName);
    }

    public class UserSettings
    {
        public int FontSize { get; set; }
    }

    internal static class UserSettingsExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> ToKeyValuePairs(this UserSettings userSettings)
        {
            List<KeyValuePair<string, string>> settingValuesForTemplate = new List<KeyValuePair<string, string>>();
            settingValuesForTemplate.Add(new KeyValuePair<string, string>("FontSize", userSettings.FontSize.ToString()));
            return settingValuesForTemplate;
        }
    }

    internal static class StringExtensions
    {
        public static byte[] ToByteArray(this string inputString)
        {
            return inputString.Select(x => Convert.ToByte(x)).ToArray();
        }
    }

    #region test doubles
    public class FakeTemplateReader : ITemplateReader
    {
        string _template;

        public string GetTemplateAsString(string settingTemplateId)
        {
            return _template;
        }

        internal void Resturns(string template)
        {
            _template = template;
        }
    }

    public class FakeUserSettingReader : IUserSettingsReader
    {
        UserSettings _userSettings;

        public UserSettings GetSettingsForUser(string userName)
        {
            return _userSettings;
        }

        internal void Returns(UserSettings userSettings)
        {
            _userSettings = userSettings;
        }
    }

    public class TemplateUtility : IUserSettingsTemplateUtility
    {
        //token pattern is "{{key}}", however since using string.Format - "{{" == '{' and  "}}" == '}' hence all the mustaches
        private readonly string TOKEN_PATTERN = "{{{{{0}}}}}";

        public string ApplyUserSettingsToTemplate(IEnumerable<KeyValuePair<string, string>> userSettings, string settingTemplate)
        {
            foreach (var setting in userSettings)
            {
                settingTemplate = ReplaceTokensInTemplateWithValue(settingTemplate, setting);
            }

            return settingTemplate;
        }

        private string ReplaceTokensInTemplateWithValue(string settingTemplate, KeyValuePair<string, string> setting)
        {
            string tokenToReplace = ConvertKeyToToken(setting.Key);
            return settingTemplate.Replace(tokenToReplace, setting.Value);
        }

        private string ConvertKeyToToken(string settingKey)
        {
            return string.Format(TOKEN_PATTERN, settingKey);
        }
    }
    #endregion
}

