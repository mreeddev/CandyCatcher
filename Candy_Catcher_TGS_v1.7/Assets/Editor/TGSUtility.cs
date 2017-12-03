using System;
using UnityEngine;
using UnityEditor;


public class TGSUtility {

	[MenuItem( "TGS Menu/iOS Build" )]
	static void iOSBuild()
	{
		//Debug.Log (GetProjectFolderPath ());
		FileUtil.DeleteFileOrDirectory( "Assets/GooglePlayGames/" );
		FileUtil.DeleteFileOrDirectory( "Assets/Plugins/iOS/GPGSAppController.mm" );
		FileUtil.DeleteFileOrDirectory( "Assets/Plugins/iOS/GPGSAppController.h" );
		AssetDatabase.Refresh();
	}

	[MenuItem( "TGS Menu/Android Build" )]
	static void AndroidBuild()
	{
		//Debug.Log (GetProjectFolderPath ());
		FileUtil.CopyFileOrDirectory( "TGS_Plugin/GooglePlayGames/","Assets/GooglePlayGames/" );
		FileUtil.CopyFileOrDirectory( "TGS_Plugin/GPGSAppController.mm", "Assets/Plugins/iOS/GPGSAppController.mm" );
		FileUtil.CopyFileOrDirectory( "TGS_Plugin/GPGSAppController.h", "Assets/Plugins/iOS/GPGSAppController.h" );
		AssetDatabase.Refresh();
	}


}
