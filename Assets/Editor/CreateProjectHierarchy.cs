using System.Collections.Generic;
using System.IO;

using UnityEditor;

using UnityEngine;

public class CreateProjectHierarchy : EditorWindow {
  
  private static string projectName = "PROJECT_NAME";

  [ MenuItem("Assets/Create Default Folder Hierarchy") ]
  private static void SetUpFolderHierarchy() {
    CreateProjectHierarchy window = ScriptableObject.CreateInstance<CreateProjectHierarchy>();
    window.position = new Rect(Screen.width / 2, Screen.height / 2, 400, 110);
    window.ShowPopup();
  }

  private static void CreateAllFolders() {
    if(!Directory.Exists("Assets/ThirdParty"))
      Directory.CreateDirectory("Assets/ThirdParty");

    // List of all top level folders
    List<string> folders = new List<string> {
      "Art",
      "Audio",
      "Code",
      "Design",
      "Doc",
      "UI"
    };

    // Creating top level folders
    foreach(string folder in folders)
      if(!Directory.Exists("Assets/" + projectName + "/" + folder))
        Directory.CreateDirectory("Assets/" + projectName + "/" + folder);

    // List of Design subfolders
    List<string> designSubFolders = new List<string> {"Scene", "Prefab"};

    // Creating Design subfolders
    foreach(string folder in designSubFolders)
      if(!Directory.Exists("Assets/" + projectName + "/Design/" + folder))
        Directory.CreateDirectory("Assets/" + projectName + "/Design/" + folder);

    // List of UI subfolders
    List<string> uiSubFolders = new List<string> {"Asset", "Font", "Sprite"};

    // Creating UI subfolders
    foreach(string subFolder in uiSubFolders)
      if(!Directory.Exists("Assets/" + projectName + "/UI/" + subFolder))
        Directory.CreateDirectory("Assets/" + projectName + "/UI/" + subFolder);

    // Refresh AssetDatabase
    AssetDatabase.Refresh();
  }

  // Draw GUI
  private void OnGUI() {
    EditorGUILayout.LabelField("Insert the Project name used as the root folder");
    projectName = EditorGUILayout.TextField("Project Name: ", projectName);
    this.Repaint();
    GUILayout.Space(10);
    if(GUILayout.Button("Close"))
      this.Close();
    GUILayout.Space(5);
    if(!GUILayout.Button("Generate")) return;
    CreateAllFolders();
    this.Close();
  }
}
