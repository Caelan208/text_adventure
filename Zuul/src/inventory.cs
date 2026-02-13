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
    int total = 0;
    foreach (Item item in items.Values)
    {
        total += item.Weight;
    }
    return total;
}

public bool Put(string itemName, Item item) 
{ 
// Check het gewicht van het Item 
int totalWeight = GetTotalWeight() + item.Weight;

// Is er genoeg ruimte in de Inventory? 
// Past het Item? 
if (totalWeight > maxWeight)
{
    return false;
}

// Controleer of item al bestaat
if (items.ContainsKey(itemName))
{
    return false;
}

// Zet Item in de Dictionary 
items.Add(itemName, item);

// Return true/false voor succes/mislukt 
return true; 
} 
public Item Get(string itemName) 
{ 
// Zoek Item in de Dictionary 
if (!items.ContainsKey(itemName))
{
    return null;
}

// Haal item op
Item item = items[itemName];

// Verwijder Item uit Dictionary (als gevonden)
items.Remove(itemName);

// Return Item of null 
return item;
} 
}