# MindScape VR - AI Molecule Visualizer

This is a Unity VR project that uses AI to convert text descriptions of molecules (like methane and butane) into interactive 3D models. Users can explore these molecules in virtual reality.

## Features
- **AI-Powered:** Uses the Google Gemini API to parse text and generate a scene script.
- **Procedural Generation:** A C# script in Unity reads the AI's output and builds the molecule in real-time.
- **VR Interaction:** Built with Unity's XR Interaction Toolkit, allowing users to move around the scene via teleportation.

## Setup
1. Clone this repository.
2. Open the project in Unity (Version 2022.3.x or newer recommended).
3. The necessary packages should be included, but if not, install the XR Interaction Toolkit and OpenXR Plug-in from the Package Manager.