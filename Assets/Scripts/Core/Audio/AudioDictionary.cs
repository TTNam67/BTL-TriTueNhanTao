using System.Collections.Generic;

public static class AudioDictionary
{
	public static List<string> names = new List<string> {
		"808KickBoomBassSnare",
		"CompressedButton",
		"DNB",
		"UncompressedButton",
		"Null",
	};

	public static List<float> lengths = new List<float> {
		27.06286f,
		0.2002083f,
		208.1959f,
		0.1364583f,
		0f,
	};
}

public enum AudioEnum
{
	_808KickBoomBassSnare,
	CompressedButton,
	DNB,
	UncompressedButton,
	Null,
}