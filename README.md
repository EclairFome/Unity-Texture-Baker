Whilst working on my VRChat avatar, I needed to match my PC avatar's eye color on the Quest/Android version. The PC avatar used Poiyomi's hue shift to change the eye color at runtime, but Quest avatars can't use Poiyomi (since they're limited to VRChat's mobile shader). 
I needed a way to export the final shader output as a plain texture that could be used without any shader magic, this tool was the result.

# Installation

1. Download the Baker.cs
2. Move the Baker.cs into your unity project (preferably in Assets/Editor/)
3. Go to Tools in your top bar, and select the new option calles "Texture Baker"
4. Once the pop-up comes up, select or drag the material and click "Bake Texture"


