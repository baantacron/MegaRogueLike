//*******************************************//
//***************Created by Eddy*************//
//******************************************//

var Contents:Transform[];
//Although in the demo I am going to identify items as its own script *Item* I will still
//identify and add them by thier transform to allow more of a versitile of ways to program a game
//using this simple type of script :P.


function AddItem(Item:Transform){//Add an item to the inventory.
	var newContents=new Array(Contents);
	newContents.Add(Item);
	Debug.Log(Item.name+" Has been added to inventroy");
	Contents=newContents.ToBuiltin(Transform);// array to unity builtin array
}
function RemoveItem(Item:Transform){//Removed an item from the inventory.
	var newContents=new Array(Contents);
	var index=0;
	var shouldend=false;
	for(var i:Transform in newContents){
		if(i==Item){
			Debug.Log(Item.name+" Has been removed from inventroy");
			newContents.RemoveAt(index);
			shouldend=true;
			//No need to continue running through the loop since we found our item.
		}
		index++;//keep track of what index the item is and remove it.
		if(shouldend){
			Contents=newContents.ToBuiltin(Transform);
			return;
		}
	}
}



function DebugInfo(){  //A simple debug. Will tell you everything that is in the inventory.
	Debug.Log("Inventory Debug - Contents");
	items=0;
	for(var i:Transform in Contents){
		items++;
		Debug.Log(i.name);
	}
	Debug.Log("Inventory contains "+items+" Item(s)");
}