class Player
{
	// fields
	private int health;
	// auto property
	public Room CurrentRoom { get; set; }
	// constructor
	public Player()
	{
		health = 100;
		CurrentRoom = null;
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
}