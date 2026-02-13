class Item 
{ 
// fields 
public string Name { get; }
public int Weight { get; } 
public string Description { get; } 
// constructor 
public Item(string name, int weight, string description)
{ 
    Name = name;
    Weight = weight; 
    Description = description;
}
}