using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("creaturesKilled", "spellsPassed", "spellsFailed", "treasuresCaptured", "monstersInGame")]
	public class ES3UserType_RuntimeGameInfo : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_RuntimeGameInfo() : base(typeof(RuntimeGameInfo)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (RuntimeGameInfo)obj;
			
			writer.WritePrivateField("creaturesKilled", instance);
			writer.WritePrivateField("spellsPassed", instance);
			writer.WritePrivateField("spellsFailed", instance);
			writer.WritePrivateField("treasuresCaptured", instance);
			writer.WriteProperty("monstersInGame", instance.monstersInGame, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<RuntimeSoldierData>)));
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (RuntimeGameInfo)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "creaturesKilled":
					reader.SetPrivateField("creaturesKilled", reader.Read<System.Int32>(), instance);
					break;
					case "spellsPassed":
					reader.SetPrivateField("spellsPassed", reader.Read<System.Int32>(), instance);
					break;
					case "spellsFailed":
					reader.SetPrivateField("spellsFailed", reader.Read<System.Int32>(), instance);
					break;
					case "treasuresCaptured":
					reader.SetPrivateField("treasuresCaptured", reader.Read<System.Int32>(), instance);
					break;
					case "monstersInGame":
						instance.monstersInGame = reader.Read<System.Collections.Generic.List<RuntimeSoldierData>>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new RuntimeGameInfo();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_RuntimeGameInfoArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_RuntimeGameInfoArray() : base(typeof(RuntimeGameInfo[]), ES3UserType_RuntimeGameInfo.Instance)
		{
			Instance = this;
		}
	}
}