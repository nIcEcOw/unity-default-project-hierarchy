using System.Collections.Generic;
using System.IO;

using UnityEditor;

using UnityEngine;

public class CreateProjectHierarchy : EditorWindow {
  
  private static string projectName = "PROJECT_NAME";

  [ MenuItem("Assets/Setup Folder Hierarchy") ]
  private static void SetUpFolderHierarchy() {
    CreateProjectHierarchy window = ScriptableObject.CreateInstance<CreateProjectHierarchy>();
    window.position = new Rect(Screen.width / 2, Screen.height / 2, 400, 100);
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
    
    // List of Code subfolders
    List<string> codeSubFolders = new List<string> {"Script", "Shader"};
    // Creating Code subfolders
    foreach(string folder in codeSubFolders)
      if(!Directory.Exists("Assets/" + projectName + "/Code/" + folder))
        Directory.CreateDirectory("Assets/" + projectName + "/Code/" + folder);

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
    // Creating TextField to input Project Name
    EditorGUILayout.LabelField("Please enter Project Name to be used as Root folder");
    projectName = EditorGUILayout.TextField("Project Name: ", projectName);
    
    // Generate Button to create project hierarchy
    GUILayout.Space(5);
    if(GUILayout.Button("Generate")) {
      CreateAllFolders();
      this.Close();
    }
    
    // Close Button to close the dialog
    GUILayout.Space(5);
    if(GUILayout.Button("Close"))
      this.Close();
    this.Repaint();
  }
}
