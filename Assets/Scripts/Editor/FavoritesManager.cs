using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;

namespace EditorTools
{
    public class FavoritesManager : EditorWindow
    {
        private Vector2 scrollPosition;
        private List<FavoriteItem> favorites = new List<FavoriteItem>();
        private const string PREFS_KEY = "FavoritesManager_Data";
        private static GUIStyle headerStyle;
        private static GUIStyle favoriteBoxStyle;
        private Rect dropArea;
        private bool isDraggingOver = false;

        [Serializable]
        private class FavoriteItem
        {
            public string path;
            public string guid;
            public string name;
            public FavoriteType type;

            public FavoriteItem(UnityEngine.Object obj)
            {
                path = AssetDatabase.GetAssetPath(obj);
                guid = AssetDatabase.AssetPathToGUID(path);
                name = obj.name;
                type = DetermineType(obj);
            }

            private FavoriteType DetermineType(UnityEngine.Object obj)
            {
                if (obj is SceneAsset) return FavoriteType.Scene;
                if (obj is MonoScript) return FavoriteType.Script;
                if (obj is ScriptableObject) return FavoriteType.ScriptableObject;
                if (obj is Material) return FavoriteType.Material;
                if (obj is Texture2D || obj is Sprite) return FavoriteType.Texture;
                if (obj is AudioClip) return FavoriteType.Audio;
                if (obj is GameObject) return FavoriteType.Prefab;
                if (obj is AnimationClip) return FavoriteType.Animation;
                return FavoriteType.Other;
            }

            public UnityEngine.Object GetObject()
            {
                return AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            }
        }

        private enum FavoriteType
        {
            Scene,
            Script,
            ScriptableObject,
            Material,
            Texture,
            Audio,
            Prefab,
            Animation,
            Other
        }

        [Serializable]
        private class FavoritesData
        {
            public List<FavoriteItem> items = new List<FavoriteItem>();
        }

        [MenuItem("Tools/Favorites Manager")]
        public static void ShowWindow()
        {
            FavoritesManager window = GetWindow<FavoritesManager>("Favorites");
            window.minSize = new Vector2(300, 200);
            window.Show();
        }

        private void OnEnable()
        {
            LoadFavorites();
        }

        private void OnDisable()
        {
            SaveFavorites();
        }

        private void InitStyles()
        {
            if (headerStyle == null)
            {
                headerStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    fontSize = 14,
                    alignment = TextAnchor.MiddleLeft,
                    padding = new RectOffset(10, 10, 5, 5)
                };
            }

            if (favoriteBoxStyle == null)
            {
                favoriteBoxStyle = new GUIStyle(GUI.skin.box)
                {
                    padding = new RectOffset(5, 5, 5, 5),
                    margin = new RectOffset(5, 5, 2, 2)
                };
            }
        }

        private void OnGUI()
        {
            InitStyles();

            // Header
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Favorites Manager", headerStyle);
            EditorGUILayout.LabelField($"Total Items: {favorites.Count}", EditorStyles.miniLabel);
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(5);

            // Drop Area
            DrawDropArea();

            EditorGUILayout.Space(5);

            // Buttons
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear All", GUILayout.Height(25)))
            {
                if (EditorUtility.DisplayDialog("Clear All Favorites",
                    "Are you sure you want to remove all favorites?", "Yes", "No"))
                {
                    favorites.Clear();
                    SaveFavorites();
                }
            }
            if (GUILayout.Button("Refresh", GUILayout.Height(25)))
            {
                RemoveInvalidFavorites();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            // Favorites List
            DrawFavoritesList();
        }

        private void DrawDropArea()
        {
            Event evt = Event.current;
            dropArea = GUILayoutUtility.GetRect(0f, 60f, GUILayout.ExpandWidth(true));

            Color originalColor = GUI.backgroundColor;
            GUI.backgroundColor = isDraggingOver ? new Color(0.5f, 0.8f, 1f, 0.5f) : new Color(0.3f, 0.3f, 0.3f, 0.3f);

            GUI.Box(dropArea, "", favoriteBoxStyle);

            GUI.backgroundColor = originalColor;

            // Draw text in the center
            GUIStyle labelStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                normal = { textColor = isDraggingOver ? Color.cyan : Color.gray }
            };

            GUI.Label(dropArea, isDraggingOver ? "Release to Add" : "Drag & Drop Assets Here", labelStyle);

            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(evt.mousePosition))
                    {
                        isDraggingOver = false;
                        break;
                    }

                    isDraggingOver = true;
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        foreach (UnityEngine.Object draggedObject in DragAndDrop.objectReferences)
                        {
                            AddFavorite(draggedObject);
                        }
                        isDraggingOver = false;
                    }
                    evt.Use();
                    break;

                case EventType.DragExited:
                    isDraggingOver = false;
                    break;
            }
        }

        private void DrawFavoritesList()
        {
            if (favorites.Count == 0)
            {
                EditorGUILayout.HelpBox("No favorites yet. Drag and drop assets above to add them.", MessageType.Info);
                return;
            }

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            FavoriteItem itemToRemove = null;

            for (int i = 0; i < favorites.Count; i++)
            {
                FavoriteItem item = favorites[i];
                UnityEngine.Object obj = item.GetObject();

                if (obj == null)
                {
                    itemToRemove = item;
                    continue;
                }

                EditorGUILayout.BeginHorizontal(favoriteBoxStyle);

                // Type Icon
                Texture2D icon = AssetDatabase.GetCachedIcon(item.path) as Texture2D;
                if (icon != null)
                {
                    GUILayout.Label(icon, GUILayout.Width(20), GUILayout.Height(20));
                }

                // Object Field (clickable)
                EditorGUI.BeginChangeCheck();
                UnityEngine.Object newObj = EditorGUILayout.ObjectField(obj, typeof(UnityEngine.Object), false);
                if (EditorGUI.EndChangeCheck() && newObj != obj)
                {
                    // If user changed the object, update it
                    favorites[i] = new FavoriteItem(newObj);
                    SaveFavorites();
                }

                // Type Label
                EditorGUILayout.LabelField($"[{item.type}]", EditorStyles.miniLabel, GUILayout.Width(80));

                // Ping button
                if (GUILayout.Button("Ping", GUILayout.Width(45), GUILayout.Height(18)))
                {
                    EditorGUIUtility.PingObject(obj);
                    Selection.activeObject = obj;
                }

                // Remove button
                if (GUILayout.Button("✕", GUILayout.Width(25), GUILayout.Height(18)))
                {
                    itemToRemove = item;
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

            if (itemToRemove != null)
            {
                favorites.Remove(itemToRemove);
                SaveFavorites();
                Repaint();
            }
        }

        private void AddFavorite(UnityEngine.Object obj)
        {
            if (obj == null) return;

            string path = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(path)) return;

            // Check if already exists
            if (favorites.Any(f => f.path == path))
            {
                Debug.LogWarning($"'{obj.name}' is already in favorites.");
                return;
            }

            favorites.Add(new FavoriteItem(obj));
            SaveFavorites();
            Repaint();
        }

        private void RemoveInvalidFavorites()
        {
            int removedCount = favorites.RemoveAll(f => f.GetObject() == null);
            if (removedCount > 0)
            {
                SaveFavorites();
                Debug.Log($"Removed {removedCount} invalid favorite(s).");
                Repaint();
            }
        }

        private void SaveFavorites()
        {
            FavoritesData data = new FavoritesData { items = favorites };
            string json = JsonUtility.ToJson(data, true);
            EditorPrefs.SetString(PREFS_KEY, json);
        }

        private void LoadFavorites()
        {
            if (EditorPrefs.HasKey(PREFS_KEY))
            {
                string json = EditorPrefs.GetString(PREFS_KEY);
                try
                {
                    FavoritesData data = JsonUtility.FromJson<FavoritesData>(json);
                    favorites = data.items ?? new List<FavoriteItem>();
                    RemoveInvalidFavorites();
                }
                catch
                {
                    favorites = new List<FavoriteItem>();
                }
            }
        }
    }
}
