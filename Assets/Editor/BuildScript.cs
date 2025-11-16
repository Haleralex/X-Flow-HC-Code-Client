using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

public class BuildScript
{
    static string[] GetScenes()
    {
        return EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();
    }

    [MenuItem("Jenkins/Run All Tests")]
    public static void RunAllTests()
    {
        Debug.Log("🧪 Running all tests...");

        var testRunner = ScriptableObject.CreateInstance<TestRunnerApi>();

        // Subscribe to test events
        testRunner.RegisterCallbacks(new TestCallbacks());

        var filter = new Filter()
        {
            testMode = TestMode.PlayMode  // or TestMode.EditMode
        };

        testRunner.Execute(new ExecutionSettings
        {
            filters = new[] { filter }
        });
    }

    private class TestCallbacks : ICallbacks
    {
        public void RunStarted(ITestAdaptor testsToRun)
        {
            Debug.Log($"▶️ Test Run Started: {testsToRun.TestCaseCount} tests detected");
        }

        public void RunFinished(ITestResultAdaptor result)
        {
            Debug.Log($"✅ Test Run Finished: {result.PassCount} passed, {result.FailCount} failed, {result.SkipCount} skipped");
            if (result.FailCount > 0)
            {
                Debug.LogError("❌ Some tests failed!");
            }
        }

        public void TestStarted(ITestAdaptor test)
        {
            Debug.Log($"⏳ Running Test: {test.Name}");
        }

        public void TestFinished(ITestResultAdaptor result)
        {
            if (result.HasChildren) return; // only log leaf tests
            if (result.ResultState == "Passed")
                Debug.Log($"✅ {result.Name} Passed");
            else if (result.ResultState == "Failed")
                Debug.LogError($"❌ {result.Name} Failed: {result.Message}");
            else
                Debug.LogWarning($"⚠️ {result.Name} {result.ResultState}");
        }
    }

    [MenuItem("Jenkins/Build Addressables")]
    public static void BuildAddressables()
    {
        if (IsAddressablesAvailable())
        {
            Debug.Log("🔧 Building Addressables...");
            var type = System.Type.GetType("UnityEditor.AddressableAssets.Settings.AddressableAssetSettings");
            var method = type?.GetMethod("BuildPlayerContent", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            method?.Invoke(null, null);
            Debug.Log("✅ Addressables build completed.");
        }
        else
        {
            Debug.LogWarning("⚠️ Addressables package not found. Skipping Addressables build.");
        }
    }

    private static bool IsAddressablesAvailable()
    {
        return System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Any(t => t.FullName == "UnityEditor.AddressableAssets.Settings.AddressableAssetSettings");
    }

    private static BuildOptions GetBuildOptions()
    {
        string val = (System.Environment.GetEnvironmentVariable("DEVELOPMENT_BUILD") ?? "false").ToLowerInvariant();
        bool isDev = val == "on" || val == "true" || val == "yes" || val == "1";

        BuildOptions opts = BuildOptions.None;
        if (isDev)
        {
            opts |= BuildOptions.Development;
            opts |= BuildOptions.ConnectWithProfiler;
            Debug.Log("🛠️ DEVELOPMENT_BUILD = ON → Development, Deep Profiling, Autoconnect Profiler enabled.");
        }
        else
        {
            Debug.Log("🧱 DEVELOPMENT_BUILD = OFF → Building in Release mode.");
        }

        return opts;
    }

    private static void ApplyScriptingDefineSymbols(BuildTargetGroup targetGroup)
    {
        string symbolsEnv = System.Environment.GetEnvironmentVariable("SCRIPTING_DEFINE_SYMBOLS");
        
        if (string.IsNullOrEmpty(symbolsEnv))
        {
            Debug.Log("ℹ️ SCRIPTING_DEFINE_SYMBOLS is empty, skipping scripting define symbols update.");
            return;
        }

        // Get current scripting define symbols
        string currentSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
        string[] currentSymbolsArray = string.IsNullOrEmpty(currentSymbols) 
            ? new string[0] 
            : currentSymbols.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

        // Parse new symbols from environment variable (comma or semicolon separated)
        string[] newSymbolsArray = symbolsEnv
            .Split(new char[] { ',', ';' }, System.StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrEmpty(s))
            .ToArray();

        if (newSymbolsArray.Length == 0)
        {
            Debug.Log("ℹ️ No valid scripting define symbols found in SCRIPTING_DEFINE_SYMBOLS.");
            return;
        }

        // Merge current and new symbols, removing duplicates
        var allSymbols = currentSymbolsArray.Union(newSymbolsArray).Distinct().OrderBy(s => s).ToArray();
        string mergedSymbols = string.Join(";", allSymbols);

        // Apply the merged symbols
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, mergedSymbols);
        
        Debug.Log($"✅ Applied Scripting Define Symbols for {targetGroup}:");
        Debug.Log($"   Current: {(string.IsNullOrEmpty(currentSymbols) ? "(none)" : currentSymbols)}");
        Debug.Log($"   New: {string.Join(", ", newSymbolsArray)}");
        Debug.Log($"   Merged: {mergedSymbols}");
    }

    

    [MenuItem("Jenkins/Build Windows")]
    public static void BuildWindows()
    {
        ApplyScriptingDefineSymbols(BuildTargetGroup.Standalone);
        BuildAddressables();

        string buildPath = "Builds/Windows/" + Application.productName + ".exe";
        var commonOptions = GetBuildOptions();

        Debug.Log("🔨 Starting Windows build to " + buildPath);

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = GetScenes(),
            locationPathName = buildPath,
            target = BuildTarget.StandaloneWindows,
            options = commonOptions
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("✅ Windows build succeeded");
        }
        else
        {
            Debug.LogError("❌ Windows build failed");
            EditorApplication.Exit(1);
        }
    }

    public static string GetSanitizedProductName()
    {
        string rawName = Application.productName;

        string sanitized = Regex.Replace(rawName, @"[^a-zA-Z0-9]", "");

        return sanitized;
    }
}
