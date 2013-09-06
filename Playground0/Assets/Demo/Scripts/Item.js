/*
this script is made to show you one way picking up items could possibly be done.
you can find others ways to do this but using the same techniques i do to add item :P.
*/

var inventoryIcon:Texture2D;//this is what will be displayed in the demos On Screen GUI.
var canGet=true;
var itemType:String;//This will let us equip the item to specific slots. Ex: Head, Shoulder, or whatever we set up.
var BoneList:Transform[];
var stackable=false;//is it stackable?
var maxStack=99;//each stack can have 99 items stacked on it!
var stack=1;//i will set the stack to 1. since itself counts as 1.
var isEquipment=true;

function Awake(){
	var Bones=GetComponentsInChildren(Transform);//keeping track of all the bones
	var newarray=new Array(Bones);//that this item has. so we can parent them to the player.
	BoneList=newarray.ToBuiltin(Transform);
}


/*Update-----Stackables-
Basically when you get an item, If its stackable (Usually applies to stuff like potions and what not
If you already have one in your inventory we are just going to Destroy the current one and add +1
to the stack in your inventory. Simple as that! :P.*/

function OnMouseDown(){//When you click an item
	var getit=true;
	var playersinv=FindObjectOfType(Inventory);//finding the players inv. I suggest when making
		//a game, you make a function that picks up an item within the player script. and then have the inventory
		//referneced from its own variable. OR since the playerscript would be attached to the inv i suggest you
		//do GetComponent(Inventory).AddItem, This way multiple players can have inventorys.
	if(canGet){//if its getable or hasnt been gotten.
		if(stackable){
			var locatedit:Item;
			for(var t:Transform in playersinv.Contents){
				if(t.name==this.transform.name){//if the item we wanna stack this on has the same name
					var i:Item=t.GetComponent(Item);
					if(i.stack<i.maxStack){
						locatedit=i;
					}
				}
			}
			if(locatedit!=null){//if we have a stack to stack it to!
				getit=false;
				locatedit.stack+=1;
				Destroy(this.gameObject);
			}
			else{
				getit=true;
			}
		}
		if(getit){
			playersinv.AddItem(this.transform);
			MoveMeToThePlayer(playersinv.transform);//moves the object, to the player
		}
	}
}






function MoveMeToThePlayer(theplayer:Transform){//This will basically make the item stay where the player is
//as long as its in the players inventory. This can also be done multiple ways, but ill stick with an easy one.
	canGet=false;//now that we have it no need to be able to get it again.
	transform.collider.isTrigger=true;//makes it undence.
	var renderers=GetComponentsInChildren(Renderer);//get all the renderers in the object
	for(var rend:Renderer in renderers){
		rend.enabled=false;//turn off all renderers in this object.
	}
	if(transform.renderer!=null){
		transform.renderer.enabled=false;//makes it invisible
	}
	transform.parent=theplayer;//makes the object parented to the player.
	transform.localPosition=Vector3.zero;//now that the item is parented to the player
	//i can set the localPosition to 0,0,0 and it will be ontop of the player. and move around with him/her
}


/*
Alternate ways to make objects stay with a player. is just move it to an
unused part of the map and deactivate it like i did. Its invisible so it will be unseen
and its untouchable :P.
If you are switching scenes. for world areas or somthing. you can keep inventory objects
alive by doing "DontDestroyOnLoad(this)". These are just some tips. Design however youd
like to :P.
*/