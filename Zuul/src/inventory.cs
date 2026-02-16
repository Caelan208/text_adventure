class Inventory 
{ 
// fields 
private int maxWeight; 
private Dictionary<string, Item> items; 
// constructor 
public Inventory(int maxWeight) 
{ 
this.maxWeight = maxWeight; 
this.items = new Dictionary<string, Item>(); 
} 
// methods 
private int GetTotalWeight()
{
    int total = 20;
    foreach (Item item in items.Values)
    {
        total += item.Weight;
    }
    return total;
}

public bool Put(string itemName, Item item) 
{ 
// Zet Item in de Dictionary 
items.Add(itemName, item);

// Return true/false voor succes/mislukt 
return true; 
} 
public Item Get(string itemName) 
{ 
// Zoek Item in de Dictionary 
if (items.ContainsKey(itemName))
{
    Item item = items[itemName];
    // Verwijder Item uit Dictionary
    items.Remove(itemName);
    // Return Item of null 
    return item;
}
return null;
}

public int ItemCount()
{
    return items.Count;
}

public Dictionary<string, Item> GetItems()
{
    return items;
}

public bool HasItem(string itemName)
{
    return items.ContainsKey(itemName);
}
}