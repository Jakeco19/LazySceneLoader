#region NameSpaces

using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;

#endregion

//================================================================
//							IMPORTANT
//================================================================
//				Copyright LazyFridayStudio
//DO NOT SELL THIS CODE OR REDISTRIBUTE WITH INTENT TO SELL.
//
//Send an email to our support line for any questions or inquirys
//CONTACT: Lazyfridaystudio@gmail.com
//
//Alternatively join our Discord
//DISCORD: https://discord.gg/Z3tpMG
//
//Hope you enjoy the simple Scene loader 
//designed and made by lazyFridayStudio
//================================================================
namespace LazyHelper.LazySceneLoader
{
    public class LazySceneLoader : EditorWindow
    {
        #region Editor Values

        private static LazySceneLoader _window;
        private Vector2 _scrollArea = Vector2.zero;
        public LazyScene _Items;
        private UnityEngine.Object source;

        [MenuItem("Window/LazyHelper/Lazy Scene Loader")]
        public static void Init()
        {
            // Get existing open window or if none, make a new one:
            _window = (LazySceneLoader) GetWindow(typeof(LazySceneLoader));
            _window.titleContent.text = "Lazy Scene Loader";
            _window.position = new Rect(0, 0, 600, 800);
            _window.autoRepaintOnSceneChange = false;
        }

        #region Textures

        Texture logoHeader;

        Texture2D headerBackground;
        Texture2D headerSeperator;

        Texture2D submenuBackground;

        Texture2D itemBackground;

        Texture2D itemOddBackground;
        Texture2D itemEvenBackground;

        #endregion

        #region Styles

        //Padding style
        public GUIStyle stylePadding = new GUIStyle();

        //Background Styles
        public GUIStyle evenBoxStyle = new GUIStyle();
        public GUIStyle oddBoxStyle = new GUIStyle();

        //Font Styles
        public GUIStyle itemTitleStyle = new GUIStyle();

        #endregion

        #region Sections

        Rect headerSection;
        Rect subMenuSection;
        Rect ItemSection;

        #endregion

        #endregion

        #region On Enable Functions

        //On SceneChange
        private void OnHierarchyChange()
        {
            OnEnable();
            Repaint();
        }

        //Start Function
        private void OnEnable()
        {
            InitTextures();
            InitStyle();
            CreateResources();
        }

        //Draw the textures and get images
        private void InitTextures()
        {
            string path = "Assets/Editor/LazyHelpers/Resources/Logo.png";
            logoHeader = EditorGUIUtility.Load(path) as Texture;

            headerBackground = new Texture2D(1, 1);
            headerBackground.SetPixel(0, 0, new Color32(22, 22, 22, 255));
            headerBackground.Apply();

            headerSeperator = new Texture2D(1, 1);
            headerSeperator.SetPixel(0, 0, new Color32(239, 143, 29, 255));
            headerSeperator.Apply();

            submenuBackground = new Texture2D(1, 1);
            submenuBackground.SetPixel(0, 0, new Color32(33, 33, 33, 255));
            submenuBackground.Apply();

            itemBackground = new Texture2D(1, 1);
            itemBackground.SetPixel(0, 0, new Color32(22, 22, 22, 255));
            itemBackground.Apply();

            itemEvenBackground = new Texture2D(1, 1);
            itemEvenBackground.SetPixel(0, 0, new Color32(44, 44, 44, 255));
            itemEvenBackground.Apply();

            itemOddBackground = new Texture2D(1, 1);
            itemOddBackground.SetPixel(0, 0, new Color32(33, 33, 33, 255));
            itemOddBackground.Apply();
        }

        //Create the styles
        private void InitStyle()
        {
            oddBoxStyle.normal.background = itemOddBackground;
            oddBoxStyle.padding = new RectOffset(3, 3, 3, 3);
            evenBoxStyle.border = new RectOffset(0, 0, 5, 5);
            oddBoxStyle.normal.textColor = new Color32(255, 255, 255, 255);

            evenBoxStyle.normal.background = itemEvenBackground;
            evenBoxStyle.border = new RectOffset(0, 0, 5, 5);
            evenBoxStyle.padding = new RectOffset(3, 3, 3, 3);
            evenBoxStyle.normal.textColor = new Color32(255, 255, 255, 255);

            itemTitleStyle.normal.textColor = new Color32(239, 143, 29, 255);
            itemTitleStyle.fontSize = 14;
            itemTitleStyle.fontStyle = FontStyle.Bold;
            itemTitleStyle.alignment = TextAnchor.MiddleLeft;

            stylePadding.margin = new RectOffset(2, 2, 4, 4);
        }

        #endregion

        #region Drawing Functions

        private void OnGUI()
        {
            if (headerBackground == null)
            {
                OnEnable();
            }

            DrawLayout();
            DrawHeader();
            DrawSubHeading();
            DrawItems();
        }

        private void DrawLayout()
        {
            headerSection.x = 0;
            headerSection.y = 0;
            headerSection.width = Screen.width;
            headerSection.height = 25;

            subMenuSection.x = 0;
            subMenuSection.y = headerSection.height;
            subMenuSection.width = Screen.width;
            subMenuSection.height = 70;

            ItemSection.x = 0;
            ItemSection.y = headerSection.height + subMenuSection.height;
            ItemSection.width = Screen.width;
            ItemSection.height = Screen.height;

            GUI.DrawTexture(headerSection, headerBackground);
            GUI.DrawTexture(subMenuSection, submenuBackground);
            GUI.DrawTexture(ItemSection, headerBackground);

            //Draw Seperators
            GUI.DrawTexture(new Rect(headerSection.x, headerSection.height - 2, headerSection.width, 2), headerSeperator);
            GUI.DrawTexture(new Rect(subMenuSection.x, (subMenuSection.height + headerSection.height) - 2, subMenuSection.width, 2), headerSeperator);
        }

        private void DrawHeader()
        {
            GUILayout.BeginArea(headerSection);
            Rect centerRect = LazyEditorHelperUtils.CenterRect(headerSection, logoHeader);
            GUI.Label(new Rect(centerRect.x + 13, centerRect.y - 2, centerRect.width, centerRect.height), logoHeader);
            GUILayout.EndArea();
        }

        private void DrawSubHeading()
        {
            GUILayout.BeginArea(subMenuSection);


            GUILayout.BeginVertical(stylePadding);
            EditorGUILayout.HelpBox("Select the scene you want to add to the list", MessageType.Info, true);
            GUILayout.BeginHorizontal(stylePadding);

            source = EditorGUILayout.ObjectField(source, typeof(SceneAsset), false);
            if (GUILayout.Button("Add Scene", GUILayout.MaxWidth(100)))
            {
                AddScene();
                GUI.FocusControl(null);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        private void DrawItems()
        {
            //Vector2 itemAreaScroll = Vector2.zero;

            GUILayout.Space(headerSection.height + subMenuSection.height);
            //GUILayout.BeginArea(ItemSection);
            _scrollArea = GUILayout.BeginScrollView(_scrollArea);
            GUILayout.BeginVertical();
            if (_Items.Scenes.Count > 0)
            {
                _Items.Scenes.RemoveAll(SceneAsset => SceneAsset == null);
            }

            if (_Items == null)
            {
                Debug.LogWarning("Scene resource file is NULL, generating new file");
            }
            else
            {
                for (int i = 0; i < _Items.Scenes.Count; i++)
                {
                    SceneAsset T = _Items.Scenes[i];
                    bool isEven = i % 2 == 0;
                    GUIStyle itemStyle = new GUIStyle();

                    if (isEven)
                    {
                        itemStyle = evenBoxStyle;
                    }
                    else
                    {
                        itemStyle = oddBoxStyle;
                    }

                    #region Area

                    GUILayout.BeginHorizontal(itemStyle);
                    EditorGUILayout.LabelField(T.name, itemTitleStyle, GUILayout.MaxWidth(100));

                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button("Select Scene File", GUILayout.MaxHeight(20), GUILayout.MaxWidth(120)))
                    {
                        Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GetAssetPath(T));
                    }

                    if (GUILayout.Button("Load Scene", GUILayout.MaxHeight(20), GUILayout.MaxWidth(100)))
                    {
                        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        {
                            Debug.LogWarning("Scene Saved New Scene Loaded");
                            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(T));
                        }
                        else
                        {
                            Debug.LogWarning("Scene Save Cancelled");
                        }
                    }

                    if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        EditorUtility.SetDirty(_Items);
                        _Items.Scenes.RemoveAt(i);
                        AssetDatabase.SaveAssets();
                    }

                    GUILayout.EndHorizontal();

                    #endregion
                }
            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            //GUILayout.EndArea();
        }

        #endregion

        #region General Functions

        private void CreateResources()
        {
            if (AssetDatabase.IsValidFolder("Assets/Editor/LazyHelpers/LazySceneLoader/Resources"))
            {
                _Items = AssetDatabase.LoadAssetAtPath("Assets/Editor/LazyHelpers/LazySceneLoader/Resources/Scenes.asset", typeof(LazyScene)) as LazyScene;
                if (_Items == null)
                {
                    //Debug.Log("no asset file found, could not reload");	
                    _Items = CreateInstance(typeof(LazyScene)) as LazyScene;
                    AssetDatabase.CreateAsset(_Items, "Assets/Editor/LazyHelpers/LazySceneLoader/Resources/Scenes.asset");
                    GUI.changed = true;
                }
            }
            else
            {
                AssetDatabase.CreateFolder("Assets/Editor/LazyHelpers/LazySceneLoader", "Resources");

                _Items = AssetDatabase.LoadAssetAtPath("Assets/Editor/LazyHelpers/LazySceneLoader/Resources/Scenes.asset", typeof(LazyScene)) as LazyScene;
                if (_Items == null)
                {
                    //Debug.Log("no asset file found, could not reload");	
                    _Items = CreateInstance(typeof(LazyScene)) as LazyScene;
                    AssetDatabase.CreateAsset(_Items, "Assets/Editor/LazyHelpers/LazySceneLoader/Resources/Scenes.asset");
                    GUI.changed = true;
                }
            }
        }

        private void AddScene()
        {
            EditorUtility.SetDirty(_Items);
            _Items.Scenes.Add((SceneAsset) source);
            source = null;
            AssetDatabase.SaveAssets();
            //RefreshData();
        }

        #endregion
    }
}