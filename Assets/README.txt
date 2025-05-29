Level Streaming for Unity WebGL
===============================

Description
===========

This package implements basic support level streaming for Unity WebGL, like the "Streamed" build option in the Unity web player. 

Unity has streaming support in the web player, where the game can start loading as soon as the first level(s) have been loaded, and additional levels will be loaded while the game is playing, without requiring the developer to set up AssetBundles to implement this. On WebGL this is not currently supported. This package adds support for that feature to Unity WebGL, by replacing the packed .data file in the WebGL build with a sequence of data files for each level to be streamed, and by patching the data loading code to load these in sequence instead.

Disclaimer
==========

This is an unsupported package provided by Unity Technologies for demonstration purposes.

Specifically, this package relies heavily on knowledge of internals of Unity's and emscripten's build pipelines, which might change future versions, potentially causing this package to break.

Manual
======

Add this package to your project to enable streaming for your WebGL builds.

Before loading any level from your project you will need to call:

static bool WebGLLevelStreaming.CanStreamedLevelBeLoaded(int levelIndex)

which will return true if the level has already been loaded, and false otherwise.


