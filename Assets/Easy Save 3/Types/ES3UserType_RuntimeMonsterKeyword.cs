using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("keywordName", "keywordDescription")]
	public class ES3UserType_RuntimeMonsterKeyword : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_RuntimeMonsterKeyword() : base(typeof(RuntimeMonsterKeyword)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (RuntimeMonsterKeyword)obj;
			
			writer.WriteProperty("keywordName", instance.keywordName, ES3Type_string.Instance);
			writer.WriteProperty("keywordDescription", instance.keywordDescription, ES3Type_string.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (RuntimeMonsterKeyword)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "keywordName":
						instance.keywordName = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "keywordDescription":
						instance.keywordDescription = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new RuntimeMonsterKeyword();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_RuntimeMonsterKeywordArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_RuntimeMonsterKeywordArray() : base(typeof(RuntimeMonsterKeyword[]), ES3UserType_RuntimeMonsterKeyword.Instance)
		{
			Instance = this;
		}
	}
}