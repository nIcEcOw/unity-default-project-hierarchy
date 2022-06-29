using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEngine;

public class ScriptTemplateFiller : UnityEditor.AssetModificationProcessor {
  
  // Delimiters to be used for folder separation
  private static char[] delimiters = new char[] {'/', '\\', '.'};
  
  // Folders to be ignored 
  private static List<string> foldersToIgnore = new List<string> {"Assets", "Code", "Script"};

  public static void OnWillCreateAsset(string _assetName) {
    // Ignoring all .meta files
    _assetName = _assetName.Replace(".meta", string.Empty);
    if(!IsFileWithExtension(_assetName, out int index)) return;
    // Check to see if C# script
    if(!IsCSharpFile(_assetName, index)) return;
    // Create namespace Value
    string namespaceString = GetNamespaceValue(_assetName);
    // Fill in the template fields
    FillTemplateFields(_assetName, namespaceString);
    // Refresh AssetDatabase
    AssetDatabase.Refresh();
  }
  
  // Check to test if the assetName has file extension associated with it
  private static bool IsFileWithExtension(string assetName, out int index) {
    index = assetName.LastIndexOf('.');
    return index > -1;
  }
  
  // Check to test if the assetName belongs to a C# file
  private static bool IsCSharpFile(string assetName, int index) {
    assetName = assetName.Substring(index);
    return assetName.Equals(".cs");
  }
  
  // Create namespace value
  private static string GetNamespaceValue(string assetName) {
    string namespaceString = string.Empty;

    List<string> folders = assetName.Split(delimiters).ToList();
    folders = folders.Except(foldersToIgnore).ToList();

    if(folders.Count < 3) {
      namespaceString = "Globals";
      return namespaceString;
    } else {
      for(int i = 0; i < folders.Count - 2; ++i) {
        namespaceString += folders[i];
        if(i < folders.Count - 3)
          namespaceString += ".";
      }
    }
    
    return namespaceString;
  }
  
  // Fill Template Fields
  private static void FillTemplateFields(string assetName, string namespaceString) {
    string fileContent = System.IO.File.ReadAllText(assetName);
    fileContent = fileContent.Replace("#DATETIME#", System.DateTime.UtcNow.ToLongDateString());
    fileContent = fileContent.Replace("#DEVELOPER#", System.Environment.UserName);
    fileContent = fileContent.Replace("#ROOTNAMESPACE#", namespaceString);
    System.IO.File.WriteAllText(assetName, fileContent);
  }
}
