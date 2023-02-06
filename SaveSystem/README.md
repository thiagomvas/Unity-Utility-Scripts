Updating SaveData before saving and updating the variables after loading SaveData are not included since it varies on each case. To do that just make a OnLoadGame method to update variables in some way.

For saving lists, clean them before updating to save. If you have base building then update the save list every time something is built/editted. 
For variables just edit it straight on SaveData and use the SaveData to GET those variables during game

AKA instead of having ``int coins`` on Inventory, use ``SaveData.Current.coins``
