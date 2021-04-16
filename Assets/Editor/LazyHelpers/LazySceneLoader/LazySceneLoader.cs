#region NameSpaces

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

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

        public static LazySceneLoader _window;
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
        Texture2D headerBackground;
        Texture2D headerSeperator;

        Texture2D submenuBackground;

        Texture2D itemBackground;

        Texture2D itemOddBackground;
        Texture2D itemEvenBackground;
        #endregion

        #region Styles
        //Heading Style
        public GUIStyle headerStyleText = new GUIStyle();
        public GUIStyle subHeaderStyle = new GUIStyle();
        public GUIStyle headerSecondaryStyleText = new GUIStyle();
        public GUIStyle generalStyle = new GUIStyle();
        
        //Padding style
        public GUIStyle stylePadding = new GUIStyle();

        //Background Styles
        public GUIStyle evenBoxStyle = new GUIStyle();
        public GUIStyle oddBoxStyle = new GUIStyle();

        //Font Styles
        public GUIStyle itemTitleStyle = new GUIStyle();
        public GUIStyle categoryStyleButton = new GUIStyle();
        #endregion

        #region Sections

        Rect headerSection;
        Rect subMenuSection;
        Rect itemSection; 
        Rect categorySection;

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
            CreateResources();

            if (LazySceneLoaderCategoryWindow._window != null)
            {
                LazySceneLoaderCategoryWindow._window.Close();
            }
        }

        //Draw the textures and get images
        private void InitTextures()
        {
            headerBackground = new Texture2D(1, 1);
            headerBackground.SetPixel(0, 0, new Color32(26, 26, 26, 255));
            headerBackground.Apply();

            headerSeperator = new Texture2D(1, 1);
            headerSeperator.SetPixel(0, 0, new Color32(242, 242, 242, 255));
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

            itemTitleStyle.normal.textColor = new Color32(218, 124, 40, 255);
            itemTitleStyle.fontSize = 14;
            itemTitleStyle.fontStyle = FontStyle.Bold;
            itemTitleStyle.alignment = TextAnchor.MiddleLeft;

            string path = "Assets/Editor/LazyHelpers/Resources/HeaderFont.ttf";
            Font headerFont = EditorGUIUtility.Load(path) as Font;
            headerStyleText.normal.textColor = new Color32(218, 124, 40, 255);
            headerStyleText.fontSize = 16;
            headerStyleText.alignment = TextAnchor.LowerCenter;
            headerStyleText.font = headerFont;
            
            subHeaderStyle.normal.textColor = new Color32(218, 124, 40, 255);
            subHeaderStyle.fontSize = 14;
            subHeaderStyle.alignment = TextAnchor.LowerCenter;
            subHeaderStyle.font = headerFont;
            
            generalStyle.normal.textColor = new Color32(255, 255, 255, 255);
            generalStyle.fontSize = 11;



            categoryStyleButton = new GUIStyle("Button");
            categoryStyleButton.fontSize = 12;
            categoryStyleButton.alignment = TextAnchor.MiddleCenter;
            categoryStyleButton.fontStyle = FontStyle.Bold;

            stylePadding.margin = new RectOffset(2, 2, 4, 4);
        }

        #endregion
        #region Drawing Functions
        private void OnGUI()
        {
            InitStyle();
            
            if (headerBackground == null)
            {
                OnEnable();
            }

            DrawLayout();
            DrawHeader();
            DrawSubHeading();
            DrawItemAreas();

            if (_Items.allCategorys.Count > 0)
            {
                for (int i = 0; i < _Items.allCategorys.Count; i++)
                {
                    _Items.allCategorys.RemoveAll(LazySceneCategorys => LazySceneCategorys == null);
                }
                
            }
            
            if (_Items.activeCategorys.Count > 0)
            {
                for (int i = 0; i < _Items.activeCategorys.Count; i++)
                {
                    _Items.activeCategorys.RemoveAll(LazySceneCategorys => LazySceneCategorys == null);
                }
                
            }

            if (_Items.activeCategorys.Count > 0)
            {
                foreach (var s in _Items.activeCategorys)
                {
                    s.Scenes.RemoveAll(SceneAsset => SceneAsset == null);
                }
            }
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
            subMenuSection.height = 27;

            itemSection.x = 50;
            itemSection.y = headerSection.height + subMenuSection.height;
            itemSection.width = Screen.width - itemSection.x;
            itemSection.height = Screen.height;
            
            categorySection.x = 0;
            categorySection.y = headerSection.height + subMenuSection.height;
            categorySection.width = itemSection.x;
            categorySection.height = Screen.height;

            GUI.DrawTexture(headerSection, headerBackground);
            GUI.DrawTexture(subMenuSection, submenuBackground);
            GUI.DrawTexture(categorySection, submenuBackground);
            GUI.DrawTexture(itemSection, headerBackground);

            //Draw Seperators
            GUI.DrawTexture(new Rect(categorySection.width - 2, headerSection.height + subMenuSection.height, 2, categorySection.height), headerSeperator);
            GUI.DrawTexture(new Rect(headerSection.x, headerSection.height - 2, headerSection.width, 2), headerSeperator);
            GUI.DrawTexture(new Rect(subMenuSection.x, (subMenuSection.height + headerSection.height) - 2, subMenuSection.width, 2), headerSeperator);
        }

        private void DrawHeader()
        {
            GUILayout.BeginArea(headerSection);
           // Rect centerRect = LazyEditorHelperUtils.CenterRect(headerSection, logoHeader);
           GUILayout.Space(7);
           GUILayout.BeginHorizontal();
           GUILayout.Label("LAZY FRIDAY STUDIO", headerStyleText);
           GUILayout.EndHorizontal();
            //GUI.Label(new Rect(centerRect.x + 13, centerRect.y - 2, centerRect.width, centerRect.height), logoHeader);
            GUILayout.EndArea();
        }

        private void DrawSubHeading()
        {
            GUILayout.BeginArea(subMenuSection);
            GUILayout.BeginHorizontal(stylePadding);
            
            if (GUILayout.Button("Edit Categories", GUILayout.MaxWidth(100)))
            {
                LazySceneLoaderCategoryWindow.Init();
            }
            
            if (GUILayout.Button("Add Scenes", GUILayout.MaxWidth(100)))
            {
                LazySceneLoaderSceneAdderWindow.Init();
            }
            
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("Help", GUILayout.MaxWidth(100)))
            {
                Application.OpenURL("https://www.lazyfridaystudio.com/lazysceneloader");
            }
            
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private bool isAllCategorys;
        private void DrawItemAreas()
        {
            _Items.activeCategorys.RemoveAll(LazySceneCategorys => LazySceneCategorys == null);
            _Items.allCategorys.RemoveAll(LazySceneCategorys => LazySceneCategorys == null);

            for (int i = 0; i < _Items.allCategorys.Count; i++)
            {
                if (_Items.allCategorys[i].Scenes.Count <= 0)
                {
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_Items.allCategorys[i]));
                    _Items.allCategorys.RemoveAt(i);
                }
            }
            
            //CATEGORY SECTION
            Rect buttonArea = new Rect(categorySection.x + 2, categorySection.y + 2,categorySection.width - 6,categorySection.height);
            GUILayout.BeginArea(buttonArea);
            GUILayout.BeginVertical();
            
            //Set First button to show all the categorys
            if (GUILayout.Button("ALL", categoryStyleButton,GUILayout.Height(30)))
            {
                SetAllCategorysActive();
                isAllCategorys = true;
            }
            
            //Items for each category
            if (_Items.allCategorys.Count > 0)
            {
                for (int i = 0; i < _Items.allCategorys.Count; i++)
                {
                    GUILayout.Space(3);
                    string buttonName = string.Empty;
                    if (_Items.allCategorys[i].categoryTitle.Length >= 3)
                    {
                        if (GUILayout.Button(_Items.allCategorys[i].categoryTitle.Substring(0,3) , categoryStyleButton , GUILayout.Height(30)))
                        {
                            SetActiveCategory(i);
                            isAllCategorys = false;
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(_Items.allCategorys[i].categoryTitle.Substring(0,1) , categoryStyleButton , GUILayout.Height(30)))
                        {
                            SetActiveCategory(i);
                            isAllCategorys = false;
                        }
                    }
                }
            }
            
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.EndArea();
            
            //ITEM AREA
            GUILayout.Space(headerSection.height + subMenuSection.height);
            GUILayout.BeginArea(itemSection);
            _scrollArea = GUILayout.BeginScrollView(_scrollArea);
            GUILayout.BeginVertical();
            
            if (_Items.activeCategorys.Count > 0)
            {
                foreach (var s in _Items.activeCategorys)
                {
                    s.Scenes.RemoveAll(SceneAsset => SceneAsset == null);
                }
            }
            
            GUILayout.Space(5);

            if (_Items.activeCategorys.Count == 1)
            {
                GUILayout.Label(_Items.activeCategorys[0].categoryTitle, subHeaderStyle);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(_Items.activeCategorys[0].categoryDescription, generalStyle);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            else if (_Items.activeCategorys.Count >= 1)
            {
                GUILayout.Label("All", subHeaderStyle);
            }
            
            
            List<SceneAsset> allSceneAssets = new List<SceneAsset>();
            allSceneAssets.Clear();
            
            if (_Items.activeCategorys.Count > 0)
            {
                for (int i = 0; i < _Items.activeCategorys.Count; i++)
                {
                    for (int j = 0; j < _Items.activeCategorys[i].Scenes.Count; j++)
                    {
                        if (!allSceneAssets.Contains(_Items.activeCategorys[i].Scenes[j]))
                        {
                            allSceneAssets.Add(_Items.activeCategorys[i].Scenes[j]);
                        }
                    }
                }   
            }
            
            if (_Items == null)
            {
                Debug.LogWarning("Scene resource file is NULL, generating new file");
            }
            else
            {
                for (int j = 0; j < allSceneAssets.Count; j++)
                    {
                        SceneAsset T = allSceneAssets[j];
                        bool isEven = j % 2 == 0;
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

                        if (GUILayout.Button("Select File", GUILayout.MaxHeight(20), GUILayout.MaxWidth(80)))
                        {
                            Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GetAssetPath(T));
                        }

                        if (GUILayout.Button("Load Additive", GUILayout.MaxHeight(20), GUILayout.MaxWidth(90)))
                        {
                            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                            {
                                Debug.LogWarning("New Scene Loaded");
                                EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(T), OpenSceneMode.Additive);
                            }
                            else
                            {
                                Debug.LogWarning("Scene Load Cancelled");
                            }
                        }
                        
                        if (GUILayout.Button("Load", GUILayout.MaxHeight(20), GUILayout.MaxWidth(50)))
                        {
                            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                            {
                                Debug.LogWarning("New Scene Loaded");
                                EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(T));
                            }
                            else
                            {
                                Debug.LogWarning("Scene Load Cancelled");
                            }
                        }

                        if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            EditorUtility.SetDirty(_Items);
                            for (int i = 0; i < _Items.activeCategorys.Count; i++)
                            {
                                for (int k = 0; k < _Items.activeCategorys[i].Scenes.Count; k++)
                                {
                                    if (_Items.activeCategorys[i].Scenes.Contains(allSceneAssets[j]))
                                    {
                                        _Items.activeCategorys[i].Scenes.Remove(allSceneAssets[j]);
                                    }
                                }
                            }


                            AssetDatabase.SaveAssets();
                        }

                        GUILayout.EndHorizontal();

                        #endregion
                    }
                
            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
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

            if (!AssetDatabase.IsValidFolder("Assets/Editor/LazyHelpers/LazySceneLoader/Resources/Categories"))
            {
                AssetDatabase.CreateFolder("Assets/Editor/LazyHelpers/LazySceneLoader/Resources", "Categories");
            }
        }
        
        public void SetAllCategorysActive()
        {
            _Items.activeCategorys.Clear();
            
            for (int i = 0; i < _Items.allCategorys.Count; i++)
            {
                _Items.activeCategorys.Add(_Items.allCategorys[i]);
            }
        }

        public void SetActiveCategory(int listNum)
        {
            _Items.activeCategorys.Clear();
            _Items.activeCategorys.Add(_Items.allCategorys[listNum]);
        }
        
        public void AddNewCategory()
        {
            EditorUtility.SetDirty(_Items);
            
            LazySceneCategorys tempItem = CreateInstance(typeof(LazySceneCategorys)) as LazySceneCategorys;
            AssetDatabase.CreateAsset(tempItem, "Assets/Editor/LazyHelpers/LazySceneLoader/Resources/Categories/"+ Random.Range(0,99999) +".asset");
            _Items.allCategorys.Add(tempItem);
            
            AssetDatabase.SaveAssets();
        }

        #endregion
    }

    public class LazySceneLoaderCategoryWindow : EditorWindow
    {
        #region Editor Values

        public static LazySceneLoaderCategoryWindow _window;
        
        public static void Init()
        {
            // Get existing open window or if none, make a new one:
            _window = (LazySceneLoaderCategoryWindow) GetWindow(typeof(LazySceneLoaderCategoryWindow));
            _window.titleContent.text = "Category Editor";
            _window.position = new Rect(GUIUtility.GUIToScreenPoint(Event.current.mousePosition).x, GUIUtility.GUIToScreenPoint(Event.current.mousePosition).y, 600, 300);
            _window.autoRepaintOnSceneChange = false;
        }
        
        #endregion

        #region General Values

        private bool isCreating = false;
        private bool hasSelectedAnOption = false;
        
        #endregion
        
        #region Sections
        Rect headerSection;
        Rect subMenuSection;
        Rect itemSection; 
        Rect categorySection;
        #endregion
        
        #region Textures
        private Texture2D mainBackground;
        private Texture2D secondaryBackground;
        private Texture2D seperator;

        private Texture2D oddBackground;
        private Texture2D evenBackground;
        #endregion
        
        #region Styles
        public GUIStyle mainHeaderStyle = new GUIStyle();
        public GUIStyle secondaryHeaderStyle = new GUIStyle();
        public GUIStyle generalStyle = new GUIStyle();
        
        public GUIStyle paddingStyle = new GUIStyle();
        
        //Background Styles
        public GUIStyle evenBoxStyle = new GUIStyle();
        public GUIStyle oddBoxStyle = new GUIStyle();
        
        #endregion
        
        #region Generation Functions
        //On SceneChange
        private void OnHierarchyChange()
        {
            OnEnable();
            Repaint();
        }
        //Start Function
        private void OnEnable()
        {
            GenerateSections();
            GenerateStyle();
            GenerateTextures();
            _window = (LazySceneLoaderCategoryWindow) GetWindow(typeof(LazySceneLoaderCategoryWindow));
        }

        private void GenerateStyle()
        {
            string path = "Assets/Editor/LazyHelpers/Resources/HeaderFont.ttf";
            Font headerFont = EditorGUIUtility.Load(path) as Font;
            
            //MAIN HEADER
            mainHeaderStyle.normal.textColor = new Color32(218, 124, 40, 255);
            mainHeaderStyle.fontSize = 16;
            mainHeaderStyle.alignment = TextAnchor.LowerCenter;
            mainHeaderStyle.font = headerFont; 
            
            //SECONDARY HEADER
            secondaryHeaderStyle.normal.textColor = new Color32(218, 124, 40, 255);
            secondaryHeaderStyle.fontSize = 12;
            secondaryHeaderStyle.fontStyle = FontStyle.Bold;
            secondaryHeaderStyle.alignment = TextAnchor.MiddleCenter;

            //General Chat
            //General Text
            generalStyle.normal.textColor = new Color32(255, 255, 255, 255);
            generalStyle.fontSize = 12;
            generalStyle.alignment = TextAnchor.MiddleLeft;
            
            //ITEM STYLES
            oddBoxStyle.normal.background = oddBackground;
            oddBoxStyle.padding = new RectOffset(3, 3, 3, 3);
            oddBoxStyle.border = new RectOffset(0, 0, 5, 5);
            oddBoxStyle.normal.textColor = new Color32(255, 255, 255, 255);

            evenBoxStyle.normal.background = evenBackground;
            evenBoxStyle.border = new RectOffset(0, 0, 5, 5);
            evenBoxStyle.padding = new RectOffset(3, 3, 3, 3);
            evenBoxStyle.normal.textColor = new Color32(255, 255, 255, 255);


            paddingStyle.margin = new RectOffset(2, 2, 4, 4);
        }
        private void GenerateTextures()
        {
            mainBackground = new Texture2D(1, 1);
            mainBackground.SetPixel(0, 0, new Color32(26, 26, 26, 255));
            mainBackground.Apply();
            
            secondaryBackground = new Texture2D(1, 1);
            secondaryBackground.SetPixel(0, 0, new Color32(33, 33, 33, 255));
            secondaryBackground.Apply();

            seperator = new Texture2D(1, 1);
            seperator.SetPixel(0, 0, new Color32(242, 242, 242, 255));
            seperator.Apply();
            
            //Item areas
            evenBackground = new Texture2D(1, 1);
            evenBackground.SetPixel(0, 0, new Color32(44, 44, 44, 255));
            evenBackground.Apply();

            oddBackground = new Texture2D(1, 1);
            oddBackground.SetPixel(0, 0, new Color32(33, 33, 33, 255));
            oddBackground.Apply();
        }
        #endregion
        
        private void OnGUI()
        {
            GenerateStyle();
            //Safe guard against random Generation issues.
            if (mainBackground == null)
            {
                OnEnable();

            }
            
            //Draw window main panels
            GUI.DrawTexture(headerSection, mainBackground);
            GUI.DrawTexture(subMenuSection, secondaryBackground);
            GUI.DrawTexture(itemSection, mainBackground);
            
            GUI.DrawTexture(new Rect(headerSection.x, headerSection.height - 2, headerSection.width, 2), seperator);
            GUI.DrawTexture(new Rect(subMenuSection.x, (subMenuSection.height + headerSection.height) - 2, subMenuSection.width, 2), seperator);
            
            //General Drawing functions
            GenerateSections();
            DrawHeader();
            DrawSubHeading();
            DrawItemArea();
        }
        
        #region Drawing GUI
        private void GenerateSections()
        {
            headerSection.x = 0;
            headerSection.y = 0;
            headerSection.width = Screen.width;
            headerSection.height = 25;

            subMenuSection.x = 0;
            subMenuSection.y = headerSection.height;
            subMenuSection.width = Screen.width;
            subMenuSection.height = 27;
            
            itemSection.x = 0;
            itemSection.y = headerSection.height + subMenuSection.height;
            itemSection.width = Screen.width;
            itemSection.height = Screen.height;
        }
        
        public void DrawHeader()
        {
            GUILayout.BeginArea(headerSection);
            GUILayout.Space(7);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Category Editor", mainHeaderStyle);
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        public void DrawSubHeading()
        {
            GUILayout.BeginArea(subMenuSection);
            GUILayout.BeginHorizontal(paddingStyle);
            if (GUILayout.Button("Create New Category"))
            {
                isCreating = true;
                hasSelectedAnOption = true;
            }
            if (GUILayout.Button("Edit Existing Categories"))
            {
                isCreating = false;
                hasSelectedAnOption = true;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        public void DrawItemArea()
        {
            if (hasSelectedAnOption)
            {
                if (isCreating)
                {
                    CreateCategoryDisplay();
                }
                else
                {
                    EditCategoryDisplay();
                }
            }
        }

        #region Which Area

        public void EditCategoryDisplay()
        {
            GUILayout.BeginArea(itemSection);
            GUILayout.Space(2);
            
            LazyScene allItems = null;
            if (allItems == null)
            { 
                allItems = AssetDatabase.LoadAssetAtPath("Assets/Editor/LazyHelpers/LazySceneLoader/Resources/Scenes.asset", typeof(LazyScene)) as LazyScene;
            }
            
            for (int i = 0; i < allItems.allCategorys.Count; i++)
            {
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
                
                GUILayout.BeginHorizontal(itemStyle);
                allItems.allCategorys[i].categoryTitle = GUILayout.TextField(allItems.allCategorys[i].categoryTitle, 16, GUILayout.MaxWidth(120),GUILayout.MinWidth(120));
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Delete Category"))
                {
                    EditorUtility.SetDirty(allItems);
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(allItems.allCategorys[i]));
                    allItems.allCategorys.RemoveAt(i);
                    AssetDatabase.SaveAssets();
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.EndArea();
        }

        private Object source;
        private string categoryName = "Name";
        private string categoryDescription = "Basic description";
        public void CreateCategoryDisplay()
        {
            LazyScene allItems = null;
            if (allItems == null)
            { 
                allItems = AssetDatabase.LoadAssetAtPath("Assets/Editor/LazyHelpers/LazySceneLoader/Resources/Scenes.asset", typeof(LazyScene)) as LazyScene;
            }
            
            GUILayout.BeginArea(itemSection);
            GUILayout.Space(2);
            GUILayout.BeginVertical();
            
            GUILayout.BeginVertical(oddBoxStyle);
            //First Scene object
            GUILayout.Label("First Scene", secondaryHeaderStyle);
            GUILayout.Label("First Scene of the category", generalStyle);
            source = EditorGUILayout.ObjectField(source, typeof(SceneAsset), false);
            GUILayout.EndVertical();
            
            GUILayout.Space(5);
            
            //Category Name
            GUILayout.BeginVertical(evenBoxStyle);
            GUILayout.Label("Category Name", secondaryHeaderStyle);
            GUILayout.Label("Only the 3 first letters of the name will appear on the buttons", generalStyle);
            categoryName = GUILayout.TextField(categoryName,16);
            GUILayout.EndVertical();
            
            GUILayout.Space(5);
            
            //Text Area Name
            GUILayout.BeginVertical(oddBoxStyle);
            GUILayout.Label("Category Description", secondaryHeaderStyle);
            GUILayout.Label("A small description of the category", generalStyle);
            categoryDescription = GUILayout.TextArea(categoryDescription,256);
            GUILayout.EndVertical();
            
            GUILayout.BeginVertical(evenBoxStyle);

            
            
            if (source == null)
            {
                EditorGUILayout.HelpBox("First Scene File Cannot Be Null", MessageType.Error);
            }
            else
            {
                if (categoryName == string.Empty)
                {
                    EditorGUILayout.HelpBox("Name Cannot Be Empty", MessageType.Error);
                }
                else
                {
                    if (categoryDescription == string.Empty)
                    {
                        EditorGUILayout.HelpBox("Description Cannot Be Empty", MessageType.Error);
                    }
                    else
                    {
                        bool nameAlreadyTaken = false;
                        for (int i = 0; i < allItems.allCategorys.Count; i++)
                        {
                            if (allItems.allCategorys[i].categoryTitle == categoryName)
                            {
                                nameAlreadyTaken = true;
                            }
                        }

                        if (nameAlreadyTaken)
                        {
                            EditorGUILayout.HelpBox("Name Already Taken Please Choose A Different Name", MessageType.Error);
                        }
                        else
                        {
                            if (GUILayout.Button("Create Catagory"))
                            {
                                CreateCategory(categoryName, categoryDescription,(SceneAsset)source);
                            } 
                        }
                    }
                }
            }
            GUILayout.EndVertical();
            GUILayout.Space(5);
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        #endregion

        #region General Functions

        public void CreateCategory(string _categoryName, string _categoryDescription, SceneAsset _firstScene)
        {
            LazyScene allItems = null;
            if (allItems == null)
            { 
                allItems = AssetDatabase.LoadAssetAtPath("Assets/Editor/LazyHelpers/LazySceneLoader/Resources/Scenes.asset", typeof(LazyScene)) as LazyScene;
            }
            
            EditorUtility.SetDirty(allItems);
            
            LazySceneCategorys tempItem = CreateInstance(typeof(LazySceneCategorys)) as LazySceneCategorys;
            tempItem.categoryDescription = _categoryDescription;
            tempItem.categoryTitle = _categoryName;
            tempItem.Scenes.Add(_firstScene);

            string assetName;

            if (string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID("Assets/Editor/LazyHelpers/LazySceneLoader/Resources/Categories/"+ _categoryName +".asset")))
            {
                assetName = _categoryName;
            }
            else
            {
                assetName = _categoryName + Random.Range(0,9999);
            }
            
            AssetDatabase.CreateAsset(tempItem, "Assets/Editor/LazyHelpers/LazySceneLoader/Resources/Categories/"+ assetName +".asset");
            allItems.allCategorys.Add(tempItem);
            
            AssetDatabase.SaveAssets();
            LazySceneLoaderCategoryWindow._window.Close();
        }
        
        #endregion
        #endregion
    }

    public class LazySceneLoaderSceneAdderWindow : EditorWindow
    {
        #region Editor Values

        public static LazySceneLoaderSceneAdderWindow _window;
        
        Object source = null;
        public static void Init()
        {
            // Get existing open window or if none, make a new one:
            _window = (LazySceneLoaderSceneAdderWindow) GetWindow(typeof(LazySceneLoaderSceneAdderWindow));
            _window.titleContent.text = "Scene Editor Adder";
            _window.position = new Rect(GUIUtility.GUIToScreenPoint(Event.current.mousePosition).x, GUIUtility.GUIToScreenPoint(Event.current.mousePosition).y, 600, 300);
            _window.autoRepaintOnSceneChange = false;

        }
        
        #endregion

        #region Sections
        Rect headerSection;
        Rect itemSection; 
        Rect categorySection;
        #endregion
        
        #region Textures
        private Texture2D mainBackground;
        private Texture2D secondaryBackground;
        private Texture2D seperator;

        private Texture2D oddBackground;
        private Texture2D evenBackground;
        #endregion
        
        #region Styles
        public GUIStyle mainHeaderStyle = new GUIStyle();
        public GUIStyle secondaryHeaderStyle = new GUIStyle();
        public GUIStyle generalStyle = new GUIStyle();
        public GUIStyle paddingStyle = new GUIStyle();
        
        //Background Styles
        public GUIStyle evenBoxStyle = new GUIStyle();
        public GUIStyle oddBoxStyle = new GUIStyle();
        
        #endregion
        
        #region Generation Functions
        //On SceneChange
        private void OnHierarchyChange()
        {
            OnEnable();
            Repaint();
        }
        //Start Function
        private void OnEnable()
        {
            GenerateSections();
            GenerateStyle();
            GenerateTextures();
            _window = (LazySceneLoaderSceneAdderWindow) GetWindow(typeof(LazySceneLoaderSceneAdderWindow));
        }

        private void GenerateStyle()
        {
            string path = "Assets/Editor/LazyHelpers/Resources/HeaderFont.ttf";
            Font headerFont = EditorGUIUtility.Load(path) as Font;
            
            //MAIN HEADER
            mainHeaderStyle.normal.textColor = new Color32(218, 124, 40, 255);
            mainHeaderStyle.fontSize = 16;
            mainHeaderStyle.alignment = TextAnchor.LowerCenter;
            mainHeaderStyle.font = headerFont; 
            
            //SECONDARY HEADER
            secondaryHeaderStyle.normal.textColor = new Color32(218, 124, 40, 255);
            secondaryHeaderStyle.fontSize = 12;
            secondaryHeaderStyle.fontStyle = FontStyle.Bold;
            secondaryHeaderStyle.alignment = TextAnchor.MiddleCenter;

            //General Text
            generalStyle.normal.textColor = new Color32(255, 255, 255, 255);
            generalStyle.fontSize = 12;
            generalStyle.alignment = TextAnchor.MiddleLeft;
            
            //ITEM STYLES
            oddBoxStyle.normal.background = oddBackground;
            oddBoxStyle.padding = new RectOffset(3, 3, 3, 3);
            oddBoxStyle.border = new RectOffset(0, 0, 5, 5);
            oddBoxStyle.normal.textColor = new Color32(255, 255, 255, 255);

            evenBoxStyle.normal.background = evenBackground;
            evenBoxStyle.border = new RectOffset(0, 0, 5, 5);
            evenBoxStyle.padding = new RectOffset(3, 3, 3, 3);
            evenBoxStyle.normal.textColor = new Color32(255, 255, 255, 255);


            paddingStyle.margin = new RectOffset(2, 2, 4, 4);
        }
        private void GenerateTextures()
        {
            mainBackground = new Texture2D(1, 1);
            mainBackground.SetPixel(0, 0, new Color32(26, 26, 26, 255));
            mainBackground.Apply();
            
            secondaryBackground = new Texture2D(1, 1);
            secondaryBackground.SetPixel(0, 0, new Color32(33, 33, 33, 255));
            secondaryBackground.Apply();

            seperator = new Texture2D(1, 1);
            seperator.SetPixel(0, 0, new Color32(242, 242, 242, 255));
            seperator.Apply();
            
            //Item areas
            evenBackground = new Texture2D(1, 1);
            evenBackground.SetPixel(0, 0, new Color32(44, 44, 44, 255));
            evenBackground.Apply();

            oddBackground = new Texture2D(1, 1);
            oddBackground.SetPixel(0, 0, new Color32(33, 33, 33, 255));
            oddBackground.Apply();
        }
        private void GenerateSections()
        {
            headerSection.x = 0;
            headerSection.y = 0;
            headerSection.width = Screen.width;
            headerSection.height = 25;
            
            itemSection.x = 0;
            itemSection.y = headerSection.height;
            itemSection.width = Screen.width;
            itemSection.height = Screen.height;
        }

        #endregion

        private void OnGUI()
        {
            GenerateStyle();
            //Safe guard against random Generation issues.
            if (mainBackground == null)
            {
                OnEnable();

            }
            
            //Draw window main panels
            GUI.DrawTexture(headerSection, mainBackground);
            GUI.DrawTexture(itemSection, mainBackground);
            
            GUI.DrawTexture(new Rect(headerSection.x, headerSection.height - 2, headerSection.width, 2), seperator);
            //General Drawing functions
            GenerateSections();
            DrawHeader();
            DrawItemArea();
        }
        
        public void DrawHeader()
        {
            GUILayout.BeginArea(headerSection);
            GUILayout.Space(7);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Scene Editor Adder", mainHeaderStyle);
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private int _selected = 0;
        string[] _options = new string[]
        {
            "Default1", "Default2", "Default3"
        };
        public void DrawItemArea()
        {
            GUILayout.BeginArea(itemSection);
            LazyScene allItems = null;
            if (allItems == null)
            { 
                allItems = AssetDatabase.LoadAssetAtPath("Assets/Editor/LazyHelpers/LazySceneLoader/Resources/Scenes.asset", typeof(LazyScene)) as LazyScene;
            }
            
            List<string> categoryOptions = new List<string>();
            for (int i = 0; i < allItems.allCategorys.Count; i++)
            {
                categoryOptions.Add(allItems.allCategorys[i].categoryTitle);
            }

            _options = categoryOptions.ToArray();
            GUILayout.BeginVertical(evenBoxStyle);
            EditorGUILayout.HelpBox("Select the Category that the scene will be added to", MessageType.Info);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Category", generalStyle, GUILayout.MaxWidth(70),GUILayout.MinWidth(70));
            _selected = EditorGUILayout.Popup(_selected, _options);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            
            GUILayout.Space(5);
            
            GUILayout.BeginVertical(oddBoxStyle);
            EditorGUILayout.HelpBox("Scene to add", MessageType.Info);
            source = EditorGUILayout.ObjectField(source, typeof(SceneAsset), false);
            GUILayout.EndVertical();
            
            GUILayout.Space(5);
            
            GUILayout.BeginVertical(evenBoxStyle);
            if (source != null)
            {
                if (GUILayout.Button("Add Scene", GUILayout.MaxHeight(20), GUILayout.MaxWidth(120)))
                {
                    AddSceneToCategory((SceneAsset)source , _selected);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Scene cannot be null", MessageType.Error);
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        private void AddSceneToCategory(SceneAsset scene, int categoryNum)
        {
            LazyScene allItems = null;
            if (allItems == null)
            { 
                allItems = AssetDatabase.LoadAssetAtPath("Assets/Editor/LazyHelpers/LazySceneLoader/Resources/Scenes.asset", typeof(LazyScene)) as LazyScene;
            }
            
            EditorUtility.SetDirty(allItems.allCategorys[categoryNum]);

            if (!allItems.allCategorys[categoryNum].Scenes.Contains(scene))
            {
                allItems.allCategorys[categoryNum].Scenes.Add(scene); 
            }
            source = null;
            AssetDatabase.SaveAssets();
            LazySceneLoaderSceneAdderWindow._window.Close();
        }
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}