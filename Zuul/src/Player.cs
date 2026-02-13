class Player
{
	// fields
	private int health;
	private Inventory inventory;
	// auto property
	public Room CurrentRoom { get; set; }
	// constructor
	public Player()
	{
		health = 100;
		CurrentRoom = null;
		inventory = new Inventory(20);
	}
	
	// methods
	public void Damage(int amount)
	{
		health -= amount;
	}
	
	public void Heal(int amount)
	{
		health += amount;
	}
	
	public bool IsAlive()
	{
		return health > 0;
	}

	public int GetHealth()
	{
		return health;
	}

	public Inventory GetInventory()
	{
		return inventory;
	}
}