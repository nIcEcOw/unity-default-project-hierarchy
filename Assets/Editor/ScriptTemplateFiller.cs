using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEngine;

public class ScriptTemplateFiller : UnityEditor.AssetModificationProcessor {

  private static char[] delimiters = new char[] {'/', '\\', '.'};
  private static List<string> foldersToIgnore = new List<string> {"Code", "Scripts"};

  public static void OnWillCreateAsset(string _assetName) {
    _assetName = _assetName.Replace(".meta", string.Empty);
    
    if(!IsFileWithExtension(_assetName, out int index)) return;

    if(!IsCSharpFile(_assetName, index)) return;

    string namespaceString = GetNamespaceName(_assetName, out string projectName);

    FillTemplateFields(_assetName, namespaceString, projectName);

    AssetDatabase.Refresh();
  }

  private static bool IsFileWithExtension(string assetName, out int index) {
    index = assetName.LastIndexOf('.');
    return index > -1;
  }

  private static bool IsCSharpFile(string assetName, int index) {
    assetName = assetName.Substring(index);
    return assetName.Equals(".cs");
  }

  private static string GetNamespaceName(string assetName, out string projectName) {
    string namespaceString = string.Empty;
    projectName = string.Empty;

    List<string> folders = assetName.Split(delimiters).ToList();
    folders = folders.Except(foldersToIgnore).ToList();

    if(folders.Count < 1) {
      namespaceString = "Globals";
      return namespaceString;
    }
    
    for(int i = 1; i < folders.Count - 2; ++i) {
      namespaceString += folders[i];
      if(i < folders.Count - 3)
        namespaceString += ".";
    }

    projectName = folders[1];

    return namespaceString;
  }

  private static void FillTemplateFields(string assetName, string namespaceString, string projectName) {
    string fileContent = System.IO.File.ReadAllText(assetName);
    fileContent = fileContent.Replace("#DATETIME#", System.DateTime.UtcNow.ToLongDateString());
    fileContent = fileContent.Replace("#DEVELOPER#", System.Environment.UserName);
    fileContent = fileContent.Replace("#PROJECTNAME#", 
      string.IsNullOrEmpty(namespaceString) ? Application.productName : projectName);
    fileContent = fileContent.Replace("#ROOTNAMESPACE#", namespaceString);
    System.IO.File.WriteAllText(assetName, fileContent);
  }
}
