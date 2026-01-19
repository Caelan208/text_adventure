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
}