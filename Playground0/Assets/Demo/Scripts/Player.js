var WeaponSlots:Transform[];//For weapons i am going to make this list, wich will contain the
//slots of each "Slot" that weapons can be equipped to, Right Hand, Left Hand. etc. I will not do this
//with armor because armor will have its own skeleton and animation so will just need to be placed
//over the players primary skeleton.
var ArmorSlot:Item[]; //I made these 2 lists to keep track of whats what. Basically.
var ArmorSlotName:String[];//if you want 10 different types of armor, youd set this list to the
//int you want. And then go ahead and name all the slots. And im going to keep these 2 lists synced together
//so if [0] is to equal "Head" Then the only type of item that can be placed in the armor slot is armor
//of the type "Head". I will make a demonstration on how to set up the visuals as well.
//But if you would like to add more then what i have. You will need to code them in yourself.
private var csheet=false;
var positionsForCS:Rect[];//This list will contain where all buttons, equipped or not
//will be stored. THIS SHOULD HAVE THE SAME NUMBER OF cells as the ArmorSlot list
var cbutton:Texture2D;
var windowRect=Rect(100,100,200,300);//keeping track of our window.

var BoneList:Transform[];//this is used for parenting items to the player

function Awake(){
	var Bones=GetComponentsInChildren(Transform);//We are collecting all the bones in the children
	var newarray=new Array(Bones);
	BoneList=newarray.ToBuiltin(Transform);
}



function PlayAnim(anim:String){//This is going to play an animation for the player, and all of his armor.

	if(animation!=null){//If this gameObject has an animation component!
		animation.Play(anim);//We play the animation!
	}
}

function PairBones(i:Item){//Basically its going to take all the bones in the items skeleton
	//and parent them to the corresponding bone within the players skele.
	//This is good to do because it will animate the object WITHOUT having to literally play an animation
	//and it will keep track of where the objects belong for a ragdoll effect And, for the locomotion lib!
	for(var mybone:Transform in BoneList){//for all the bones in the player.
		for(var b:Transform in i.BoneList){//and for all the bones in the item
			if(b.name==mybone.name){//if the bone in the item has the same name as the bone in the player
				b.parent=mybone;//make the items bone parented to the players bone
				b.rotation=mybone.rotation;//and give it the same attributes.
				b.localPosition=Vector3.zero;
				//Remember, I have the bones that belong to the item already stored in a list.
				//so it will be easy to unequip the item and reparent all the bones back to the item when
			}
		}
	}
}

function CheckSlot(tocheck:int){//just checking if we already have somthing equipped
	var toreturn=false;
	if(ArmorSlot[tocheck]!=null){
		toreturn=true;
	}
	return toreturn;
}

function UseItem(i:Item,slot:int,autoequip:boolean){//Using the item. If we assign a slot, we already know where to equip it.
	if(i.isEquipment){
		if(autoequip){//This is incase we.. dbl click the item, it will auto equip it.
			var index=0;//Keeping track of where we are in the list.
			var equipto=0;//and where we wanna be at the end
			for(var a in ArmorSlotName){//for all the named slots on the armorslots list
				if(a==i.itemType){//if the name is the same as the armor type
					equipto=index;//we will want to be aimed at that slot!
				}
				index++;//We move on to the next slot.
			}
			EquipItem(i,equipto);
		}
		else{//if we dont auto equip it then it means we must of tried to equip it to a slot
		//so we make sure the item can be equipped to that slot!
			if(i.itemType==ArmorSlotName[slot]){//if the equipment is the same as the slot
				EquipItem(i,slot);
			}
		}
	}
}



function EquipItem(i:Item,slot:int){
	if(i.itemType==ArmorSlotName[slot]){//if it can be equipped there
		if(CheckSlot(slot)){//if theres an item equipped to that slot
			UnequipItem(ArmorSlot[slot]);
			ArmorSlot[slot]=null;
		}
		ArmorSlot[slot]=i;//when we find out slot we set it to the item
		var rends=i.GetComponentsInChildren(Renderer);//make it visible
		var rend:Renderer=i.GetComponent(Renderer);//make it visible
		if(rend){
			i.enabled=true;
		}
		for(var r:Renderer in rends){
			r.enabled=true;
		}
		i.transform.position=transform.position;//set its position
		i.transform.rotation=transform.rotation;//and rotation were it belongs
		PairBones(i);//and pair all of its bones to the players bones.
	}
}

function UnequipItem(i:Item){
	for(var b:Transform in i.BoneList){//we take all the bones we parented to the player
		b.parent=i.transform;//and unparent them!
	}
	var rends=i.GetComponentsInChildren(Renderer);//make it invisible
	var rend:Renderer=i.GetComponent(Renderer);//make it invisible
	if(rend){
		i.enabled=false;
	}
	for(var r:Renderer in rends){
		r.enabled=false;
	}
}



//THIS WILL DISPLAY THE CHARACTER SHEET (Inventory). This is part of the demo and not the
//tutorial/guide, so may need to be replace when making your own game. You may use this if youd like though.


function OnGUI(){	
	if(cbutton!=null){
		if(GUI.Button(Rect(0,Screen.height-40,40,40),cbutton)){
			//make a icon button that will open and close the character sheet.
			if(csheet){
				csheet=false;//csheet off
			}
			else{
				csheet=true;//csheet on
			}
		}
	}
	if(csheet){//If the csheet is opened up.
		//we will make a window that shows whats in the csheet.
		windowRect=GUI.Window (1, windowRect, DisplayCSheetWindow, "Character Sheet");
		//and update the position and size variables from the windows variables.
		//We will use these variables later on to determine where the inventory icons belong
		//and for later uses. This could be done by directly pulling the variabled from windowRect
		//but i prefer storing the variables as vector2's.
	}
}





function DisplayCSheetWindow(windowID:int){
	GUI.DragWindow (Rect (0,0, 10000, 20));  //the window to be able to be dragged
	//Im going to create the character sheet. This will display the objects equipped.
	//*WARNING* I CAN ASSIGN THESE VAR's Without Errors because i know what
	//slot is wich. If your going to re-use this script. Your going to need to re-Hard Code this
	//yourself!
	//UPDATED: Im gonna change around this lot of code! Sorry about changing it all around on ya
	//USING a loop and another list (where its equipped) to make it more efficient
	//^____^
	
	var index=0;
	for(var a in ArmorSlot){
		if(a==null){
			if(GUI.Button(positionsForCS[index],"")){//if we click this button (That has no item equipped)
				var id=GetComponent(InventoryDisplayBag);
				if(id.itemBeingDragged!=null){//if we are dragging an item
					EquipItem(id.itemBeingDragged,index);
					id.ClearDraggedItem();//Stop dragging the item.
				}
			}
		}
		else{
			if(GUI.Button(positionsForCS[index],ArmorSlot[index].inventoryIcon)){
				var id2=GetComponent(InventoryDisplayBag);
				if(id2.itemBeingDragged!=null){
					EquipItem(id2.itemBeingDragged,index);
					id2.ClearDraggedItem();
				}
				else{
					UnequipItem(ArmorSlot[index]);
					ArmorSlot[index]=null;
					id2.ClearDraggedItem();
				}
			}
		}
		index++;
	}
}
