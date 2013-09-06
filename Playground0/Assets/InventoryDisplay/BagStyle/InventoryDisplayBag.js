//This type of inventory display will be a bag. similair to WoW.
var  itemBeingDragged:Item;//This refers to the "Item" script. you WILL need
//this script in your game to use this line of code.
var draggedItemPosition:Vector2;//Where on the screen we are dragging our item.
var draggedItemSize:Vector2;//The size of the item icon we are dragging.

var backDrop:Texture2D;
private var windowPosition:Vector2=Vector2(200,200);//where on the screen the window will appear.
//this can easily be and should be updated on the fly incase the screen size changes or what not.
private var windowSize:Vector2=Vector2(108.0,130.0);//the size of the window the bag will be displayed.
var itemIconSize:Vector2=Vector2(32.0,32.0);//The size of the item icons
var updateListDelay=1.0;//This will be used to updated the inventory on screen, rather then
//updating it every time OnGUI is called. if you prefer you can directly get what in the list. but i
//dont like having multiple GetComponents >.<.
var lastUpdate=0.0;//last time we updated the display.
var UpdatedList:Transform[];//The inv list with a late delay.
var associatedInventory:Inventory;//var to keep track of your inv
var displayInventory=false;//if inv is opened
var invSkin:GUISkin;//the GUI skin.
var windowRect=Rect(200,200,108,130);//keeping track of our window.
var bagIcon:Texture2D;//this texture will be used for our switch
var offsetX=6;//This will leave so many pixels between the edge of the window. left and right
var offsetY=6;//This will leave so many pixels between the edge of the window. top and bottom

function Awake(){
	//On this line im hard coding the position. So the bag is displayed Above the button.
	//You can remove this to have it appear where you want it.
	windowRect=Rect(Screen.width-windowSize.x,Screen.height-40-windowSize.y,windowSize.x,windowSize.y);
	associatedInventory=GetComponent(Inventory);//keepin track of the inventory script
}
function UpdateInventoryList(){//update the delayed inv list
	UpdatedList=associatedInventory.Contents;
}
function Update(){
	if(Input.GetKeyDown(KeyCode.Escape)){//Pressed escape
		ClearDraggedItem();//get rid of the dragged item.
	}
}

function OnGUI(){	
	GUI.skin=invSkin;
	if(itemBeingDragged!=null){//if we are in faCt dragging an item!
		//Once again Im going to use a button to show where the item is being dragged around.
		//The reason for this is because buttons are easier to see and will still show on screen if
		//no inventoryIcon is assigned! If youd like you can change this to GUI.DrawTexture() !
		GUI.Button(Rect(draggedItemPosition.x,draggedItemPosition.y,draggedItemSize.x,draggedItemSize.y),itemBeingDragged.inventoryIcon);
	}
	if(bagIcon!=null){
		if(GUI.Button(Rect(Screen.width-40,Screen.height-40,40,40),bagIcon)){
			//make a bag icon button that will open and close the inventory.
			if(displayInventory){
				displayInventory=false;//inv off
				//since we close inventory we will clear the dragged item
				ClearDraggedItem();
			}
			else{
				displayInventory=true;//inv on
			}
		}
	}
	if(displayInventory){//If the inventory is opened up.
		//we will make a window that shows whats in the inventory.
		windowRect=GUI.Window (0, windowRect, DisplayInventoryWindow, "Inventory");
		//and update the position and size variables from the windows variables.
		//We will use these variables later on to determine where the inventory icons belong
		//and for later uses. This could be done by directly pulling the variabled from windowRect
		//but i prefer storing the variables as vector2's.
		windowPosition=Vector2(windowRect.x,windowRect.y);
		windowSize=Vector2(windowRect.width,windowRect.height);
	}
}

function FixedUpdate(){//I will update the display inventory here at a set delay to limit cpu usage
	if(itemBeingDragged!=null){//Making the dragged icon update its position
		//as you can see im giving a 15 pixel space away from the mouse pointer to allow me to be able
		//to click stuff and not hit the button we are dragging
		draggedItemPosition.y=Screen.height-Input.mousePosition.y+15;
		draggedItemPosition.x=Input.mousePosition.x+15;
	}
	if(Time.time>lastUpdate){
		lastUpdate=Time.time+updateListDelay;
		UpdateInventoryList();
	}
}



function DisplayInventoryWindow(windowID:int){
	//GUI.DragWindow (Rect (0,0, 10000, 20));  //Do we want the window to be able to be dragged?
	var currentX=0+offsetX;//where to put the first items
	var currentY=18+offsetY;//Im setting the start y position to 18 to give room for the title bar on the window
	//Draw the backdrop in the windowposition and the size of the windowsize.
	if(backDrop!=null){//if we have a backDrop to display, we display it!
		GUI.DrawTexture(Rect(0,0,windowSize.x,windowSize.y),backDrop,ScaleMode.StretchToFill);
	}
	for(var i:Transform in UpdatedList){//we start a loop for whats in our list.
		var item=i.GetComponent(Item);//we know that all objects in this list are items, cus we
		//will make sure nothing else can go in here, RIGHT? :P
		//directly call accocialtedInventory.Contents but i prefer not to since its more work for you and the pc.
		//I use a button since its easier to be able to click it and then make a drop down menu to delete or move
		if(GUI.Button(Rect(currentX,currentY,itemIconSize.x,itemIconSize.y),item.inventoryIcon)){
			//THIS HERE IS A DEMO ELEMENT! IF YOU DECIDE NOT TO USE THE DEMO ASSETS
			//YOU WILL NEED TO REMOVE THIS OR REPLACE IT WITH YOUR OWN STUFF.
			var dragitem=true;//Incase we stop dragging an item we dont wanna redrag a new one.
			if(itemBeingDragged==item){//Basically. we clicked the item, then clicked it again
				GetComponent(Player).UseItem(item,0,true);//so we use it. if its equipment we autoequip it (true)
				ClearDraggedItem();//stop dragging what we are dragging
				dragitem=false;//dont redrag what we stopped dragging
			}
			if(dragitem){
				itemBeingDragged=item;//set the item being dragged.
				draggedItemSize=itemIconSize;//We set the dragged icon size to our item button size
				//and the position it should be at (mouseposition)
				draggedItemPosition.y=Screen.height-Input.mousePosition.y-15;//im taking away 15 pixels so the button isnt directly on the top of the cursor
				draggedItemPosition.x=Input.mousePosition.x+15;
			}
		}
		if(item.stackable){
			GUI.Label(Rect(currentX,currentY,itemIconSize.x,itemIconSize.y),""+item.stack,"Stacks");
			//This will show a number showing how many items a stack has IF it has a stack
			//Im using a GUISkin with this and its going to use the custom "Stacks"
			//this way i can choose where to put the number at ex: Top left top right bottom right.
		}
		currentX+=itemIconSize.x;
		if(currentX+itemIconSize.x+offsetX>windowSize.x){
		//if the next item icon will be to large for the window..... and the offsetX (to keep it looking neat)
			currentX=offsetX;//we move it back to its startpoint wich is 0 + our offsetX.
			currentY+=itemIconSize.y;//and down a row.
			if(currentY+itemIconSize.y+offsetY>windowSize.y){//if the row is down to far. we quit the loop
			//I also am going to unclude a offset here. So items wont be crouding the window line.
				return;
			}
		}
	}
}


function ClearDraggedItem(){//If we are dragging an item, we will clear it and not be anymore.
	itemBeingDragged=null;
}