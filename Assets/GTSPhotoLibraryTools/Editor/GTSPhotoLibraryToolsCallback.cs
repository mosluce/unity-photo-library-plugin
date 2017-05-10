using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace GTSPhotoLibraryTools
{
    class Callback
    {
        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target == BuildTarget.iOS)
            {
                ModifyProj(pathToBuiltProject);
                ModifyInfoPlist(pathToBuiltProject);
            }
        }

        private static void ModifyProj(string pathToBuiltProject)
        {
            var pbxProjectPath = Path.Combine(pathToBuiltProject, "./Unity-iPhone.xcodeproj/project.pbxproj");
            PBXProject pbxProject = new PBXProject();
            pbxProject.ReadFromFile(pbxProjectPath);

            var targetGUID = pbxProject.TargetGuidByName("Unity-iPhone");

            pbxProject.AddFrameworkToProject(targetGUID, "Photos.framework", weak: false);
            File.WriteAllText(pbxProjectPath, pbxProject.WriteToString());
        }

        private static void ModifyInfoPlist(string pathToBuiltProject)
        {
            var infoPlistPath = Path.Combine(pathToBuiltProject, "./Info.plist");
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(infoPlistPath));

            PlistElementDict rootDict = plist.root;
            // 相簿權限描述 (iOS10 必須設定)
            rootDict.SetString("NSPhotoLibraryUsageDescription", "");
            // 全螢幕
            rootDict.SetBoolean("UIRequiresFullScreen", true);

            File.WriteAllText(infoPlistPath, plist.WriteToString());
        }
    }
}
