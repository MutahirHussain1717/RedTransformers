using System;

public static class GlobalScripts
{
	public static int mainmenu_checkbt;

	public static GlobalScripts.GameState gameState;

	public static float damage_player;

	public static bool HeadShot;

	public static int DimndCount;

	public static int CurrLevelIndex;

	public static bool currIndex;

	public enum GameState
	{
		GameReady,
		GamePlaying
	}
}
