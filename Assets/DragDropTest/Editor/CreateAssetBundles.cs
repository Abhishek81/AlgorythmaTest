using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
	[MenuItem("Assets/Build AssetBundles")]
	static void BuildAllAssetBundles()
	{
		string assetBundleDirectory = "Assets/AssetBundles/iOS";
		if(!Directory.Exists(assetBundleDirectory)){																		//
			Directory.CreateDirectory(assetBundleDirectory);																//	Building Asset Bundle for iOS
		}																													//
		BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.iOS);				//

		assetBundleDirectory = "Assets/AssetBundles/Android";																//
		if(!Directory.Exists(assetBundleDirectory)){																		//	Building Asset Bundle for Android
			Directory.CreateDirectory(assetBundleDirectory);																//
		}																													//
		BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.Android);			//

		assetBundleDirectory = "Assets/AssetBundles/Mac";																	//
		if(!Directory.Exists(assetBundleDirectory)){																		//	Building Asset Bundle for Mac
			Directory.CreateDirectory(assetBundleDirectory);																//
		}																													//
		BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);		//

		assetBundleDirectory = "Assets/AssetBundles/Windows";																//
		if(!Directory.Exists(assetBundleDirectory)){																		//	Building Asset Bundle for Windows
			Directory.CreateDirectory(assetBundleDirectory);																//
		}																													//
		BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);	//
	}
}