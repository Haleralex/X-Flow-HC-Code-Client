#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XFlow.Core;
using XFlow.Shop;

namespace XFlow.Shop.Editor
{
    public class BundleRandomGenerator : EditorWindow
    {
    private int bundlesCount = 5;
    private int minCosts = 0;
    private int maxCosts = 2;
    private int minRewards = 1;
    private int maxRewards = 3;
    private int seed = 0;
    private string outputFolder = "Assets/GeneratedBundles";

    private bool costHealth = true;
    private bool costGold = true;
    private bool costLocation = false;
    private bool costVIP = false;

    private bool rewardHealth = false;
    private bool rewardGold = false;
    private bool rewardLocation = true;
    private bool rewardVIP = true;

        [MenuItem("XFlow/Tools/Bundle Random Generator")]
        public static void ShowWindow()
        {
            var wnd = GetWindow<BundleRandomGenerator>("Bundle Random Generator");
            wnd.minSize = new Vector2(420, 220);
        }

        private void OnGUI()
        {
            GUILayout.Label("Generate random bundles", EditorStyles.boldLabel);

            bundlesCount = EditorGUILayout.IntField("Bundles count", bundlesCount);
            EditorGUILayout.BeginHorizontal();
            minCosts = EditorGUILayout.IntField("Min costs", minCosts);
            maxCosts = EditorGUILayout.IntField("Max costs", maxCosts);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            minRewards = EditorGUILayout.IntField("Min rewards", minRewards);
            maxRewards = EditorGUILayout.IntField("Max rewards", maxRewards);
            EditorGUILayout.EndHorizontal();

            seed = EditorGUILayout.IntField("Random seed (0 = time)", seed);
            outputFolder = EditorGUILayout.TextField("Output folder", outputFolder);

            GUILayout.Space(6);
            EditorGUILayout.LabelField("Cost domains (choose which domains to pick cost actions from)", EditorStyles.miniBoldLabel);
            EditorGUILayout.BeginHorizontal();
            costHealth = EditorGUILayout.ToggleLeft("Health", costHealth, GUILayout.Width(100));
            costGold = EditorGUILayout.ToggleLeft("Gold", costGold, GUILayout.Width(100));
            costLocation = EditorGUILayout.ToggleLeft("Location", costLocation, GUILayout.Width(100));
            costVIP = EditorGUILayout.ToggleLeft("VIP", costVIP, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Reward domains (choose which domains to pick reward actions from)", EditorStyles.miniBoldLabel);
            EditorGUILayout.BeginHorizontal();
            rewardHealth = EditorGUILayout.ToggleLeft("Health", rewardHealth, GUILayout.Width(100));
            rewardGold = EditorGUILayout.ToggleLeft("Gold", rewardGold, GUILayout.Width(100));
            rewardLocation = EditorGUILayout.ToggleLeft("Location", rewardLocation, GUILayout.Width(100));
            rewardVIP = EditorGUILayout.ToggleLeft("VIP", rewardVIP, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(8);
            if (GUILayout.Button("Generate"))
            {
                if (bundlesCount <= 0)
                {
                    EditorUtility.DisplayDialog("Error", "Bundles count must be > 0", "OK");
                }
                else
                {
                    GenerateBundles();
                }
            }

            GUILayout.Space(8);
            EditorGUILayout.HelpBox("This tool finds all GameActionScriptableObject assets in the project and uses them as building blocks for random bundles. Each bundle will contain a random number of cost actions and reward actions (within the specified ranges).\n\nNote: Run this tool in the Editor. After generation, open the 'Assets/GeneratedBundles' folder.", MessageType.Info);
        }

        private void GenerateBundles()
        {
            if (seed != 0)
                UnityEngine.Random.InitState(seed);
            else
                UnityEngine.Random.InitState(Environment.TickCount);

            var actionGuids = AssetDatabase.FindAssets("t:GameActionScriptableObject");
            var actions = new List<GameActionScriptableObject>();

            if (actionGuids == null || actionGuids.Length == 0)
            {
                var allGuids = AssetDatabase.FindAssets("t:ScriptableObject");
                foreach (var g in allGuids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(g);
                    var so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                    if (so is GameActionScriptableObject gas)
                        actions.Add(gas);
                }
            }
            else
            {
                foreach (var g in actionGuids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(g);
                    var so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                    if (so is GameActionScriptableObject gas)
                    {
                        string ns = gas.GetType().Namespace ?? string.Empty;
                        if (IsActionAllowedByDomain(ns, true) || IsActionAllowedByDomain(ns, false))
                            actions.Add(gas);
                    }
                }
            }

            if (actions.Count == 0)
            {
                EditorUtility.DisplayDialog("No actions found", "No GameActionScriptableObject assets found in project. Create some action ScriptableObjects first.", "OK");
                return;
            }

            if (!AssetDatabase.IsValidFolder(outputFolder))
            {
                var parent = "Assets";
                var parts = outputFolder.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 1; i < parts.Length; i++)
                {
                    string folder = string.Join("/", parts, 0, i + 1);
                    if (!AssetDatabase.IsValidFolder(folder))
                    {
                        AssetDatabase.CreateFolder(parent, parts[i]);
                    }
                    parent = folder;
                }
            }

            var createdPaths = new List<string>();

            for (int i = 0; i < bundlesCount; i++)
            {
                string bundleName = $"RandomBundle_{i + 1}";
                var bundle = ScriptableObject.CreateInstance<Bundle>();

                int costsCount = UnityEngine.Random.Range(minCosts, maxCosts + 1);
                int rewardsCount = UnityEngine.Random.Range(minRewards, maxRewards + 1);

                var costsList = new List<GameActionScriptableObject>();
                var rewardsList = new List<GameActionScriptableObject>();

                var costPool = new List<GameActionScriptableObject>();
                var rewardPool = new List<GameActionScriptableObject>();
                foreach (var a in actions)
                {
                    string ns = a.GetType().Namespace ?? string.Empty;
                    if (IsActionAllowedByDomain(ns, true)) costPool.Add(a);
                    if (IsActionAllowedByDomain(ns, false)) rewardPool.Add(a);
                }

                if (costPool.Count == 0) costPool.AddRange(actions);
                if (rewardPool.Count == 0) rewardPool.AddRange(actions);

                for (int c = 0; c < costsCount; c++)
                {
                    var pick = costPool[UnityEngine.Random.Range(0, costPool.Count)];
                    costsList.Add(pick);
                }

                for (int r = 0; r < rewardsCount; r++)
                {
                    var pick = rewardPool[UnityEngine.Random.Range(0, rewardPool.Count)];
                    rewardsList.Add(pick);
                }

                var so = new SerializedObject(bundle);
                var nameProp = so.FindProperty("bundleName");
                if (nameProp != null) nameProp.stringValue = bundleName;

                var costsProp = so.FindProperty("costs");
                if (costsProp != null)
                {
                    costsProp.arraySize = costsList.Count;
                    for (int idx = 0; idx < costsList.Count; idx++)
                        costsProp.GetArrayElementAtIndex(idx).objectReferenceValue = costsList[idx];
                }

                var rewardsProp = so.FindProperty("rewards");
                if (rewardsProp != null)
                {
                    rewardsProp.arraySize = rewardsList.Count;
                    for (int idx = 0; idx < rewardsList.Count; idx++)
                        rewardsProp.GetArrayElementAtIndex(idx).objectReferenceValue = rewardsList[idx];
                }

                so.ApplyModifiedProperties();

                string path = AssetDatabase.GenerateUniqueAssetPath(outputFolder + "/" + bundleName + ".asset");
                AssetDatabase.CreateAsset(bundle, path);
                createdPaths.Add(path);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Generated", $"Created {createdPaths.Count} bundles in {outputFolder}", "OK");
        }

        private void CreateFolderIfNotExists(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath)) return;
            if (AssetDatabase.IsValidFolder(folderPath)) return;

            var parts = folderPath.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            string current = parts[0];
            if (!current.Equals("Assets", StringComparison.OrdinalIgnoreCase))
                current = "Assets";

            for (int i = 1; i < parts.Length; i++)
            {
                string next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                }
                current = next;
            }
        }

        private bool IsActionAllowedByDomain(string @namespace, bool forCost)
        {
            if (string.IsNullOrEmpty(@namespace)) return false;
            @namespace = @namespace.ToLowerInvariant();
            if (forCost)
            {
                if (costHealth && @namespace.Contains("xflow.health")) return true;
                if (costGold && @namespace.Contains("xflow.gold")) return true;
                if (costLocation && @namespace.Contains("xflow.location")) return true;
                if (costVIP && @namespace.Contains("xflow.vip")) return true;
                return false;
            }
            else
            {
                if (rewardHealth && @namespace.Contains("xflow.health")) return true;
                if (rewardGold && @namespace.Contains("xflow.gold")) return true;
                if (rewardLocation && @namespace.Contains("xflow.location")) return true;
                if (rewardVIP && @namespace.Contains("xflow.vip")) return true;
                return false;
            }
        }
    }
}
#endif