# MocartAssignment

Instructions:
Clone repository and either:
1. In unity project select run and build and use the local host port in the browser that opens and copy to desired broswer for testing if default brower is not desired one
2. Build into target folder and use html in a website project or just use files inside build folder inside your own index

it is important to notice that depending on project you might need to change unity project settings such as compression.

External Packages:

Newtonsoft.Json: Add by name "com.unity.nuget.newtonsoft-json", better json repository
Better json conversion library

DoTween: Asset Store
For ui animation

Code Overview:

Managers: spawned on scene load and responsible for main flow and utilities. 
Events: each componenet is responsible for registering and unregistering to events. enables independance of componenets from each other. 
since one cant use async functions in webgl build we only have limited use of events in project. 
NetworkMessager: uses the httprequesttool to send desired requests over network and handle responses.
Scene: ShelfLogic is responsible for activating NoteLogic which is reponsoble for displaying product information individualy. in future might
be responsible also for displaying productson the shelf itself.
CameraPositioner: makes sure that displayed image is alwyas of the shelf at optimal proportions.
UI: EditFrontLogic is responsible for EnterEditButtonLogic which start edit mode and the EditPanelLogic which is responsible for changing names and prices of products. 
