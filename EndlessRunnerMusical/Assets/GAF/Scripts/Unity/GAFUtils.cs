/*
 * File:           GAFUtils.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

#define GAF_ASSERT
#define GAF_PRINT_ERROR

using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Text;

public class GAFUtils
{
	public static void Assert(bool _statemant, string _Message)
	{
#if GAF_ASSERT
		if (!_statemant)
		{
			Debug.LogError(_Message);
			Debug.Break();
		}
#endif // GAF_ASSERT
	}
	
	public static void Error(string _Message)
	{
#if GAF_PRINT_ERROR

		Debug.LogError(_Message);

#endif // GAF_PRINT_ERROR
	}

	public static string PropertyDump(object variable)
	{
		var stringPropertyNamesAndValues = variable.GetType().GetProperties();
		
		System.Text.StringBuilder dump = new System.Text.StringBuilder();
		foreach (PropertyInfo pair in stringPropertyNamesAndValues)
		{
			if (pair.PropertyType == typeof(string) && pair.GetGetMethod() != null)
			{
				var Name 	= pair.Name;
				var Value	= pair.GetGetMethod().Invoke(variable, null);

				string pairAsString = string.Format("Name: {0} Value: {1}{2}", Name, Value, System.Environment.NewLine);
				dump.Append(pairAsString);
			}
		}

		return dump.ToString();
	}

	public static readonly Vector3 InvalidVector = new Vector3(int.MinValue, int.MinValue, int.MinValue);
}
