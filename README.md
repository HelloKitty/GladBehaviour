# GladBehaviour
Library that uses that Unity3D MonoBehaviour class as a vector to deliver additional functionality to behaviours.

##Features

Current Features:
* Draws interface fields to the inspector
* Draws collections of interface fields to the inspector

##Tests

#### Linux/Mono - Unit Tests
||Debug x86|Debug x64|Release x86|Release x64|
|:--:|:--:|:--:|:--:|:--:|:--:|
|**master**| N/A | N/A | N/A | [![Build Status](https://travis-ci.org/HelloKitty/GladBehaviour.svg?branch=master)](https://travis-ci.org/HelloKitty/GladBehaviour) |
|**dev**| N/A | N/A | N/A | [![Build Status](https://travis-ci.org/HelloKitty/GladBehaviour.svg?branch=dev)](https://travis-ci.org/HelloKitty/GladBehaviour)|

#### Windows - Unit Tests

(Done locally)

##How to Use

Build the VS solution, import the built DLLs, mark the xxx.Editor.dll file as an editor only file and then inherit from GladMonoBehaviour instead of MonoBehaviour to gain the additional functionality.

##Builds

Linux: [![Build Status](https://travis-ci.org/HelloKitty/GladBehaviour.svg?branch=master)](https://travis-ci.org/HelloKitty/GladBehaviour)

Windows: (Done Locally)
