using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

// Place this script in an Editor folder
public class FolderColorizer : EditorWindow
{
    private static Dictionary<string, Color> folderColors = new Dictionary<string, Color>();
    private static Texture2D folderIcon;
    
    [MenuItem("Tools/Folder Colorizer")]
    public static void ShowWindow()
    {
        GetWindow<FolderColorizer>("Folder Colorizer");
    }
    
    void OnEnable()
    {
        // Load saved folder colors
        LoadFolderColors();
        
        // Subscribe to project window GUI event
        EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
        
        // Cache the default folder icon
        folderIcon = AssetDatabase.GetCachedIcon("Assets") as Texture2D;
    }
    
    void OnDisable()
    {
        // Unsubscribe when window is closed
        EditorApplication.projectWindowItemOnGUI -= OnProjectWindowItemGUI;
    }
    
    void OnGUI()
    {
        GUILayout.Label("Folder Color Settings", EditorStyles.boldLabel);
        
        EditorGUILayout.Space();
        
        // Example: Set color for a specific folder
        GUILayout.Label("Add New Folder Color:", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        string folderPath = EditorGUILayout.TextField("Folder Path", "Assets/YourFolder");
        Color folderColor = EditorGUILayout.ColorField("Color", Color.blue);
        EditorGUILayout.EndHorizontal();
        
        if (GUILayout.Button("Add/Update Folder"))
        {
            folderColors[folderPath] = folderColor;
            SaveFolderColors();
            EditorApplication.RepaintProjectWindow();
        }
        
        EditorGUILayout.Space();
        
        // Display and edit existing folder colors
        GUILayout.Label("Existing Folder Colors:", EditorStyles.boldLabel);
        List<string> keysToRemove = new List<string>();
        
        foreach (var kvp in folderColors)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(kvp.Key, GUILayout.Width(200));
            folderColors[kvp.Key] = EditorGUILayout.ColorField(kvp.Value);
            if (GUILayout.Button("Remove", GUILayout.Width(80)))
            {
                keysToRemove.Add(kvp.Key);
            }
            EditorGUILayout.EndHorizontal();
        }
        
        // Remove deleted entries
        foreach (var key in keysToRemove)
        {
            folderColors.Remove(key);
        }
        
        if (keysToRemove.Count > 0)
        {
            SaveFolderColors();
            EditorApplication.RepaintProjectWindow();
        }
    }
    
    private static void OnProjectWindowItemGUI(string guid, Rect selectionRect)
    {
        string assetPath = AssetDatabase.GUIDToAssetPath(guid);
        
        // Check if this is a folder and if we have a custom color for it
        if (System.IO.Directory.Exists(assetPath) && folderColors.ContainsKey(assetPath))
        {
            // Draw the colored folder icon
            Color color = folderColors[assetPath];
            DrawColoredIcon(selectionRect, color);
        }
    }
    
    private static void DrawColoredIcon(Rect rect, Color color)
    {
        // This is a simplified version - in a full implementation,
        // you would create a colored version of the folder icon
        Color originalColor = GUI.color;
        GUI.color = color;
        
        // Adjust rect for icon size
        rect.width = 16;
        rect.height = 16;
        
        if (folderIcon != null)
        {
            GUI.DrawTexture(rect, folderIcon);
        }
        
        GUI.color = originalColor;
    }
    
    private void SaveFolderColors()
    {
        // Convert dictionary to JSON and save to EditorPrefs
        string data = JsonUtility.ToJson(new SerializableDictionary<string, Color>(folderColors));
        EditorPrefs.SetString("FolderColorizer_Data", data);
    }
    
    private void LoadFolderColors()
    {
        // Load from EditorPrefs and convert from JSON
        if (EditorPrefs.HasKey("FolderColorizer_Data"))
        {
            string data = EditorPrefs.GetString("FolderColorizer_Data");
            SerializableDictionary<string, Color> loadedData = JsonUtility.FromJson<SerializableDictionary<string, Color>>(data);
            folderColors = loadedData.ToDictionary();
        }
    }
}

// Helper class for serialization (Unity can't serialize dictionaries directly)
[System.Serializable]
public class SerializableDictionary<TKey, TValue>
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();
    
    [SerializeField]
    private List<TValue> values = new List<TValue>();
    
    public SerializableDictionary() { }
    
    public SerializableDictionary(Dictionary<TKey, TValue> dictionary)
    {
        foreach (var kvp in dictionary)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }
    
    public Dictionary<TKey, TValue> ToDictionary()
    {
        Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
        for (int i = 0; i < keys.Count; i++)
        {
            if (i < values.Count)
            {
                dict[keys[i]] = values[i];
            }
        }
        return dict;
    }
}