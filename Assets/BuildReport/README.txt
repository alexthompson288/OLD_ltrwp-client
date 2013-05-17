

=== Important Notes ===

In case you get any build errors that you think might be caused by the Build Report Tool, try disabling the Build Report Window from showing automatically whenever you build. You can do this by searching for the OnPostprocessBuild function in BuildReport.cs and removing/commenting out its PostProcessBuild attribute or deleting that whole function.




=== Changelog ===

Version 1.6
- Added option to manually override what Editor.log to look at.
- Added option to select files in an asset list. All selected files have their total file size and percentage displayed.


Version 1.5
- Added options screen
  - Can now specify if Build Report Tool will collect information automatically or not.
  - Can now specify whether svn/git metadata get included in unused assets scan.
  - File filters can now be either a drop down box or the previous buttons.
- Build info can now be serialized (saved) into XML files for later viewing. Top row of buttons now have "Open" and "Save" for accessing and saving XML files, respectively.
  - Automatically gathered build info are also automatically saved, either in the user's personal folder (My Documents/Home) or beside the project folder. This can be changed in the options screen.


Version 1.4

- The Build Report Window will now show automatically after building your project.
- The Build Report Window is now a dockable window. Layout is changed, segregated in three major parts: Overview, Used Assets, and Unused Assets. The Asset Breakdown list is now in Used Assets.
- The Used Assets List (formerly Asset Breakdown List), can now be filtered. There are two types of filters: 1) by file type, and 2) by folder name.
  - By file type: the usual textures, models, scripts, sounds category. This is determined by the file type extension name of the file.
  - By folder name: you can filter assets based on what folder they reside in. For example, there's a filter to get all assets inside any folder named "Standard Assets". This will filter for any parent folder of the file.
  - Filters can be customized by editing FiltersUsed.cs (found in the BuildReport/Scripts/Editor/UserConfigurable folder) and adding/editing the entries. It's pretty much self-explenatory, and the entries there serve as samples on how to use it. Prepend your entries with a dot to indicate that it's a file type filter, or put a slash "/" before and after to indicate that it's a folder type filter.
  - Take note that upgrading to newer versions in the future may overwrite your FiltersUsed.cs. Backup first before updating the Build Report Tool to a new version.
- The Build Report Window can now show UNUSED assets in your project. This is for determining files that you no longer use. Same with the Used Assets List, this list can be filtered by file type or folder. The filters used in the Used Assets List is the same one used for this Unused Assets List.
- Note: Avatar Mask files (.mask files, used only in Unity 4) are not shown in the Unused Assets List. Right now, there is no reliable way of determining if an Avatar Mask file is used in the build or not.
- The Build Report Tool can now show prefabs that are not referenced as variables or residing in a Resources folder but nevertheless used in a build. See notes below on "Prefab Instances in Scenes" for details.
- Fixed a bug when unable to get DLL file size when using Unity 4.


Version 1.3

- The DLL list now shows the file size for each DLL, and is sorted by size, large to small. In particular, if you enable size stripping for mscorlib.dll, you'll see the file size of that DLL reflected properly.
- The DLL list is now separated into two lists: DLLs made from your scripts, and DLLs of standard Mono libraries that your project uses.
- The total compressed build size now shows for Android builds (this is simply the file size of the resulting .apk file).




=== How to use ===

To show the Build Report Window, simply go to Window > Show Build Report.

Make sure you build your project first. If not, the Build Report Tool will attempt to retrieve build info from your previous session.

However, it won't care if the project you built last time isn't the same one you currently have open. So it's always best to build your project before you look at the Build Report Window.




=== Parts ===

Overview: Shows Total Build Size, Size Breakdown, and DLLs included.

Size Breakdown: This will report how much your assets take space, per file type category (textures, meshes, sounds, scripts, etc.)

DLLs Included: A small list of all dynamically linked library (DLL) files your project uses, including the DLLs for your scripts, standard Mono DLLs, and other managed DLL plugins you use.


Used Assets: This is a more detailed look on how much your assets take space. Each asset included in your build will be shown here, including the storage space they take while not in their final compressed state yet.

You can click on the category buttons to filter out file types.

Clicking on an entry in the Asset Breakdown list will highlight that file in your Project window.




=== What's counted in the Build Report ===

The Build Report only takes into account your assets' size, plus managed DLLs. The real final size of your build may be larger or smaller depending on the platform.

In desktop and mobile builds, the build size reflects the size of the resulting sharedassets0.assets, sharedassets1.assets (and so on) files that are generated. Also counted are the files in the "Managed" folder (Mono DLLs, plus your scripts in compiled DLL form).

All other files are not counted in the report. That includes native DLLs (though I may try to find a way around that).


In desktop builds, you may find a "unity default resources" file in your build. Inside that are defaults, like the default GUI Skin, default font for the GUI, default shaders, the built-in cube, cylinder, or capsule, default white material, etc.

That file is also not counted in the Build Report.

See discussion here: http://forum.unity3d.com/threads/120081-unity-default-resources


In Windows, your resulting .exe file is also not considered in the Build Report as that is considered a "boilerplate" resource. The contents of your .exe file is largely standard among all Windows programs built in Unity, with some minor changes.


In web builds, your .unity3d file is a compressed archive of all your used assets, your Unity scene files, your scripts' resulting managed DLLs, any managed DLLs from the Mono standard library your build needs (take note there are certain Mono libraries that cannot be included due to considerations for web browser security), plus any managed DLLs that you explicitly included.


In iOS builds, the total build size only represents the size of the game before it gets compiled and packaged into an .app file in Xcode. The size may get smaller once it gets packaged into an .ipa file.


Note: Managed DLLs mean DLL files containing compiled .NET/Mono code. Native DLLs mean DLL files built out of standard C/C++ code.




=== Prefab Instances in Scenes ===

If you have prefab instances in a scene, they don't actually count in the editor log's build info. Why exactly, I'm not sure.

My guess is things work this way: Actually during runtime, the concept of prefabs do not exist anymore. As far as Unity is concerned, they are all just game objects. Instantiate actually merely duplicates/clones an existing game object (whether it is currently residing in the scene or not). (side note: you can actually use Instantiate to duplicate a game object, even if it is not a prefab)

As such, during runtime, prefab connections get lost. Prefab instances simply revert into normal game objects. So the prefab files in your assets folder are not included in the build. Instead, the prefab instances that are in your scenes are the ones counted (technically their file sizes are in the scene files).

Two exceptions:

1. If your prefab file is in a Resources folder, it gets included in the build.
2. If your prefab is referenced from a script via a variable/field, then it is included in the build info.

So you may find some prefab files in the Asset Breakdown list that do not show a size reading. Those are the prefabs that are used in scenes but not in a Resources folder nor referenced in a variable.




=== Size Readings (Why is the build size larger when shown in Windows Explorer?) ===

You may find that the sizes in the Build Report Tool are different from the sizes of the files on disk. This is normal. Here is a good explanation why:  http://superuser.com/questions/66825/what-is-the-difference-between-size-and-size-on-disk

Quote:

We know that a disk is made up of Tracks and Sectors. In Windows that means the OS allocates space for files in "clusters" or "allocation units".

The size of a cluster can vary, but typical ranges are from 512 bytes to 32K or more. For example, on my C:\ drive, the allocation unit is 4096 bytes. This means that Windows will allocate 4096 bytes for any file or portion of a file that is from 1 to 4096 bytes in length.

...

Another example would be if I have a file that is 2000 bytes in size. The file size on disk would be 4096 bytes. The reason is, because even though the entire file can fit inside one allocation unit, it still takes up 4096 of space (one allocation unit) on disk (only one file can use an allocation unit and cannot be shared with other files).

So the size on disk is the space of all those sectors in which the file is saved. That means, usually, the size on disk is always greater than the actual size.

End quote.


Suffice to say that when your game/app will be downloaded over the Internet, the amount that needs to be transferred is the size reading that you see in the Build Report Tool, not the size reading on your disk.

Side note: Size readings in the Build Report window are rounded up by two decimal places.




=== Additional Notes ===

Don't worry, the assets that the Build Report Tool itself uses won't be included in your build.
