#if UNITY_EDITOR

public partial class BuildReport
{
	static FileFilters[] CreateFileFilters()
	{
		return new FileFilters[]
		{
			new FileFilters("Textures",
				new string[]{
				".psd",
				".jpg",
				".jpeg",
				".gif",
				".png",
				".tiff",
				".tif",
				".tga",
				".bmp",
				".dds",
				".exr",
				".iff",
				".pict",
			}),
			new FileFilters("Models",
				new string[]{
				".fbx",
				".dae",
				".mb",
				".ma",
				".max",
				".blend",
				".obj",
				".3ds",
				".dxf",
			}),
			new FileFilters("Prefabs",
				new string[]{
				".prefab",
			}),
			new FileFilters("Animation",
				new string[]{
				".anim",
				".controller",
				".mask",
			}),
			new FileFilters("Movies",
				new string[]{
				".mov",
				".mpg",
				".mpeg",
				".mp4",
				".avi",
				".asf",
			}),
			new FileFilters("Materials",
				new string[]{
				".mat",
				".sbsar",
				".cubemap",
				".flare",
			}),
			new FileFilters("Shaders",
				new string[]{
				".shader",
				".compute",
				".cginc",
			}),
			new FileFilters("GUI",
				new string[]{
				".guiskin",
				".fontsettings",
				".ttf",
				".dfont",
				".otf",
			}),
			new FileFilters("Sounds",
				new string[]{
				".wav",
				".mp3",
				".ogg",
				".aif",
				".xm",
				".mod",
				".it",
				".s3m",
			}),
			new FileFilters("Scripts",
				new string[]{
				".cs",
				".js",
				".boo",
				".dll",
				".py",
			}),
			new FileFilters("Text",
				new string[]{
				".txt",
				".bytes",
				".html",
				".htm",
				".xml",
			}),
			new FileFilters("Misc",
				new string[]{
				".asset",
				".physicmaterial",
				".unity",
			}),
			new FileFilters("Standard Assets",
				new string[]{
				"/Standard Assets/",
			}),
			new FileFilters("Editor",
				new string[]{
				"/Editor/",
			}),
			new FileFilters("Version Control",
				new string[]{
				"/.svn/",
				"/.git/",
				"/.cvs/",
			}),
		};

	}
}

#endif