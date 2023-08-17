using Common;
using Microsoft.Win32;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using Selenium.Dev.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OpenQA.Selenium.DevTools.V89.Emulation;
using System.Threading;
using Common.Extensions;

namespace Selenium.Dev
{
    public class SeleniumDev
    {
        public static readonly string[] MOBILE_BROWSER_USER_AGENT = File.ReadAllLines($"{Environment.CurrentDirectory}\\Assets\\mobile-brower-user-agent.txt");
        IWebDriver _driver;
        IWait<IWebDriver> _wait;
        DriverService _service;
        DriverOptions _options;

        private BrowserType _browser;
        private string _version;
        private string _driverPath;
        private Proxy _proxy;
        private string _chromeProfileDir;
        private string _firefoxProfileName;
        private int _port;
        private string _remoteAddress;
        private bool _webRTC;
        private bool _cookieEditor;
        private bool _incognito;
        private bool _hideCommandPromptWindow;
        private bool _mobileEmulationEnable;
        private bool _connectToRunningBrowser;
        private bool _deleteProfileAfterUse;
        private bool _imageBlock;
        private bool _maximize;
        private bool _headless;
        private TimeSpan _commandTimeout;
        private TimeSpan _pageTimeout;
        private Point? _startUpLocation;
        private Size? _size;
        public SeleniumDev(BrowserType browser)
        {
            _maximize = true;
            _browser = browser;
            _driverPath = "Driver/x86";
            _remoteAddress = "127.0.0.1";
            _port = 9222;
#if DEBUG
            _hideCommandPromptWindow = false;
#else
            _hideCommandPromptWindow = true;
#endif
            _deleteProfileAfterUse = false;
            _commandTimeout = TimeSpan.FromMinutes(15);
            _pageTimeout = TimeSpan.FromMinutes(15);
        }

        public IWebDriver Driver => _driver;
        public IWait<IWebDriver> Wait => _wait;
        public DriverService Service => _service;
        public DriverOptions Options => _options;
        public IJavaScriptExecutor JsExecutor => _driver as IJavaScriptExecutor;
        public ReadOnlyCollection<Cookie> Cookies => _driver != null ? _driver.Manage().Cookies.AllCookies : null;
        public string Url => _driver.Url;
        public bool IsReady => _driver != null && _wait != null;
        public BrowserType Browser { get => _browser; set { _browser = value; } }
        public string Version { get => _version; set { _version = value; } }
        public string DriverPath { get => _driverPath; set { _driverPath = value; } }
        public Proxy Proxy { get => _proxy; set { _proxy = value; } }
        public string FirefoxProfileName { get => _firefoxProfileName; set { _firefoxProfileName = value; } }
        public string ChromeProfileDir { get => _chromeProfileDir; set { _chromeProfileDir = value; } }
        public bool WebRTC { get => _webRTC; set { _webRTC = value; } }
        public bool Incognito { get => _incognito; set { _incognito = value; } }
        public bool HideCommandPromptWindow { get => _hideCommandPromptWindow; set { _hideCommandPromptWindow = value; } }
        public bool DeleteProfileAfterUse { get => _deleteProfileAfterUse; set { _deleteProfileAfterUse = value; } }
        public bool MobileEmulationEnable { get => _mobileEmulationEnable; set { _mobileEmulationEnable = value; } }
        public bool ConnectToRunningBrowser { get => _connectToRunningBrowser; set { _connectToRunningBrowser = value; } }
        public bool ImageBlock { get => _imageBlock; set { _imageBlock = value; } }
        public bool Headless { get => _headless; set { _headless = value; } }
        public bool CookieEditor { get => _cookieEditor; set { _cookieEditor = value; } }
        public TimeSpan CommandTimeout { get => _commandTimeout; set { _commandTimeout = value; } }
        public Point? StartUpLocation { get => _startUpLocation; set { _startUpLocation = value; } }
        public Size? Size { get => _size; set { _size = value; } }

        public IWebDriver CreateDriver()
        {
            switch (_browser)
            {
                case BrowserType.Firefox:
                    return InitFirefoxDriver();
                case BrowserType.Chrome:
                    return InitChromeDriver();
                default:
                    if (CheckInstalled("firefox"))
                        return InitFirefoxDriver();
                    else if (CheckInstalled("chrome"))
                        return InitChromeDriver();
                    else
                        throw new Exception("Browser is not installed");
            }
        }

        public IWebDriver CreateChromeDriverCommandLine()
        {
            InitChromeDriverCommandLine();
            if (_size != null)
                _driver.Manage().Window.Size = _size.Value;
            if (_startUpLocation != null)
                _driver.Manage().Window.Position = _startUpLocation.Value;
            return _driver;
        }

        private IWebDriver InitFirefoxDriver()
        {
            FirefoxOptions firefoxOptions = new FirefoxOptions();
            if (!string.IsNullOrEmpty(_firefoxProfileName))
            {
                FirefoxProfileManager manager = new FirefoxProfileManager();
                FirefoxProfile firefoxProfile = manager.GetProfile(_firefoxProfileName);
                firefoxProfile.DeleteAfterUse = false;
                firefoxOptions.Profile = firefoxProfile;
            }
            if (_webRTC)
                firefoxOptions.SetPreference("media.peerconnection.enabled", false);
            if (_mobileEmulationEnable)
                firefoxOptions.SetPreference("general.useragent.override", MOBILE_BROWSER_USER_AGENT[RandomExtensions.Random(0, MOBILE_BROWSER_USER_AGENT.Length)]);
            if (_proxy != null)
                firefoxOptions.Proxy = _proxy;
            if (_headless)
                firefoxOptions.AddArguments("--headless");
            _options = firefoxOptions;
            if (_version == null)
                _version = "v0.29.0";

            FirefoxDriverService firefoxDriverService = FirefoxDriverService.CreateDefaultService($"{_driverPath}/firefox/{_version}");
            _service = firefoxDriverService;
            if (_connectToRunningBrowser)
                firefoxDriverService.ConnectToRunningBrowser = _connectToRunningBrowser;
            if (_hideCommandPromptWindow)
                _service.HideCommandPromptWindow = _hideCommandPromptWindow;
            firefoxDriverService.FirefoxBinaryPath = GetBrowserPath("firefox");
            FirefoxDriver firefoxDriver = new FirefoxDriver(firefoxDriverService, firefoxOptions, _commandTimeout);
            if (_maximize)
                firefoxDriver.Manage().Window.Maximize();
            //start firefox.exe --start-debugger-server 9224 -profile C:\Users\TVCU\AppData\Local\Mozilla\Firefox\Profiles\bg6udslr.dat2
            //if (_imageBlock)
            //    firefoxDriver.InstallAddOnFromFile("WebExtensions/Firefox/image_block-5.0-fx.xpi");
            _driver = firefoxDriver;
            _driver.Manage().Timeouts().PageLoad.Add(_pageTimeout);
            return _driver;
        }
        private IWebDriver InitChromeDriver()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            if (!_headless)
            {
                if (_webRTC)
                    chromeOptions.AddExtension("WebExtensions/Chrome/WebRTC.crx");
                if (_imageBlock)
                    chromeOptions.AddExtension("WebExtensions/Chrome/ImageBlock.crx");
                if (_cookieEditor)
                    chromeOptions.AddExtension("WebExtensions/Chrome/CookieEditor.crx");
            }

            if (_mobileEmulationEnable)
                chromeOptions.EnableMobileEmulation("iPhone 5/SE");
            if (_incognito)
                chromeOptions.AddArguments("--incognito");

            if (_proxy != null)
            {
                chromeOptions.Proxy = _proxy;
                chromeOptions.AddArgument("ignore-certificate-errors");
            }
            if (_maximize)
                chromeOptions.AddArgument("--start-maximized");
            if (_chromeProfileDir != null)
            {
                if (!Directory.Exists(_chromeProfileDir))
                    Directory.CreateDirectory(_chromeProfileDir);
                chromeOptions.AddArgument($"user-data-dir={_chromeProfileDir}");
            }
            if (_headless)
                chromeOptions.AddArguments("headless");

            chromeOptions.AddArgument("no-sandbox");
            _options = chromeOptions;
            _version = GetChromeVersion();
            if (_version == null)
                _version = "89";
            else
            {
                _version = _version.Split('.').FirstOrDefault();
                if (_version == null)
                    _version = "89";
            }
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService($"{_driverPath}/chrome/{_version}");
            _service = chromeDriverService;
            if (_hideCommandPromptWindow)
                _service.HideCommandPromptWindow = _hideCommandPromptWindow;
            _driver = new ChromeDriver(chromeDriverService, chromeOptions, _commandTimeout);

            _driver.Manage().Timeouts().PageLoad.Add(_pageTimeout);
            return _driver;
        }

        private IWebDriver InitChromeDriverCommandLine()
        {
            if (!_connectToRunningBrowser)
            {
                Process proc = new Process();
                proc.StartInfo.FileName = GetBrowserPath("chrome");
                string arguments = $" --remote-debugging-port={_port}";
                if (_chromeProfileDir != null)
                {
                    if (!Directory.Exists(_chromeProfileDir))
                        Directory.CreateDirectory(_chromeProfileDir);
                    arguments += $" --user-data-dir=\"{_chromeProfileDir}\"";
                }


                if (_maximize)
                    arguments += " --start-maximized";
                if (_webRTC)
                    arguments += $" --load-extension=\"{Environment.CurrentDirectory}\\WebExtensions\\Chrome\\WebRTC\"";
                proc.StartInfo.Arguments = arguments;
                proc.Start();
            }

            //if (_proxy != null)
            //{
            //    chromeOptions.Proxy = _proxy;
            //    chromeOptions.AddArgument("ignore-certificate-errors");
            //}
            ChromeOptions chromeOptions = new ChromeOptions();
            _options = chromeOptions;
            _version = GetChromeVersion();
            if (_version == null)
                _version = "89";
            else
            {
                _version = _version.Split('.').FirstOrDefault();
                if (_version == null)
                    _version = "89";
            }
            if (_connectToRunningBrowser)
                chromeOptions.DebuggerAddress = $"{_remoteAddress}:{_port}";
            _driver = new ChromeDriver($"{_driverPath}/chrome/{_version}", chromeOptions);

            _driver.Manage().Timeouts().PageLoad.Add(_pageTimeout);
            return _driver;
            //  chrome.exe --remote-debugging-port=9222 --user-data-dir=C:\\A --load-extension=C:\\E --load-extension="F:\SOUR

        }

        public IWait<IWebDriver> CreateDriverWait(TimeSpan? timeout = null, TimeSpan? pollingInterval = null)
        {
            _wait = new WebDriverWait(_driver, timeout != null ? timeout.Value : TimeSpan.FromMinutes(5))
            {
                PollingInterval = pollingInterval != null ? pollingInterval.Value : TimeSpan.FromSeconds(2)
            };
            _wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            return _wait;
        }
        public void CreateDevToolsSession()
        {
            if (_browser == BrowserType.Chrome)
            {
                var devToolsSession = (_driver as IDevTools).GetDevToolsSession();
                SetUserAgentOverrideCommandSettings a = new SetUserAgentOverrideCommandSettings()
                {
                    UserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.101 Mobile Safari/537.36"
                };
                devToolsSession.SendCommand(a);
            }
        }


        public void Dispose()
        {
            try
            {
                if (_driver != null)
                {
                    try
                    {
                        if (!_deleteProfileAfterUse)
                            _driver.Close();
                        _driver.Quit();
                        _driver.Dispose();
                    }
                    catch (Exception ex) { }
                 
                }
                if (_wait != null)
                    _wait = null;
                Process[] AllProcesses = Process.GetProcesses();
                foreach (var process in AllProcesses)
                {
                    string s = process.ProcessName.ToLower();
                    if (s.StartsWith("chromedriver") || s.StartsWith("geckodriver"))
                        switch (_browser)
                        {
                            case BrowserType.Chrome:
                                if (s.StartsWith("chromedriver") && process.GetMainModuleFileName().Contains(Environment.CurrentDirectory))
                                    process.Kill();
                                break;
                            case BrowserType.Firefox:
                                if (s.StartsWith("geckodriver") && process.GetMainModuleFileName().Contains(Environment.CurrentDirectory))
                                    process.Kill();
                                break;
                            default:
                                break;
                        }
                }
            }
            catch (Exception e)
            {
                Logs.Error(e);
            }
            finally
            {
                if (_browser == BrowserType.Chrome && _chromeProfileDir != null && _deleteProfileAfterUse && Directory.Exists(_chromeProfileDir))
                {
                    try
                    {
                        Directory.Delete(_chromeProfileDir, true);
                        Directory.CreateDirectory(_chromeProfileDir);
                    }
                    catch (Exception ex)
                    {
                    }

                }
            }
        }


        public void ChangeBrowerStartUpLocation()
        {

        }
        public bool CheckInstalled(string browserName)
        {
            string registryKey = Environment.Is64BitOperatingSystem ? @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\App Paths" : @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey);
            if (key != null)
            {
                foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName)))
                {
                    if (subkey.Name != null && subkey.Name.Contains(browserName))
                    {
                        return true;
                    }
                }
                key.Close();
            }
            return false;
        }
        public string GetChromeVersion()
        {


            var path = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe", "", null);
            if (path != null)
                return FileVersionInfo.GetVersionInfo(path.ToString()).FileVersion;
            return null;
        }
        public string GetBrowserPath(string browserName)
        {
            RegistryKey browserKeys;
            //on 64bit the browsers are in a different location
            browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Clients\StartMenuInternet");
            if (browserKeys == null)
                browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");
            string[] browserNames = browserKeys.GetSubKeyNames();
            string browserNameUpper = browserName.ToUpper();
            foreach (var name in browserNames)
            {
                if (name.ToUpper().Contains(browserNameUpper))
                {
                    RegistryKey browserKey = browserKeys.OpenSubKey(name);
                    RegistryKey browserKeyPath = browserKey.OpenSubKey(@"shell\open\command");
                    return (string)browserKeyPath.GetValue(null).ToString().StripQuotes();
                }
            }
            return null;
        }

    }
}
