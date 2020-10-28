using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazyEditorHelperUtils : MonoBehaviour
{
	#region Helper Functions
	/// <summary>
	/// Returns the center of a rect based on an texture and rect
	/// </summary>
	/// <param name="size"></param>
	/// <param name="textureToCenter"></param>
	/// <returns></returns>
	public static Rect CenterRect(Rect size, Texture textureToCenter)
	{
		return new Rect(((size.x + size.width * .5f) - (textureToCenter.width * .5f)), ((size.y + size.height / 2) - textureToCenter.height / 2), textureToCenter.width, textureToCenter.height);
	}

	public static string ClipedString(string text, int maxCharacter = 200)
	{
		if (text.Length > maxCharacter)
		{
			return text.Substring(0, maxCharacter) + ".....";
		}
		else
		{
			return text;
		}
	}
	#endregion
}
