using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("warbandName", "warbandWizard", "warbandSoldiers", "warbandGold", "warbandMaxSoldiers", "warbandVault")]
	public class ES3UserType_PlayerWarband : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_PlayerWarband() : base(typeof(PlayerWarband)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (PlayerWarband)obj;
			
			writer.WriteProperty("warbandName", instance.warbandName, ES3Type_string.Instance);
			writer.WritePropertyByRef("warbandWizard", instance.warbandWizard);
			writer.WriteProperty("warbandSoldiers", instance.warbandSoldiers, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<SoldierScriptable>)));
			writer.WriteProperty("warbandGold", instance.warbandGold, ES3Type_int.Instance);
			writer.WriteProperty("warbandMaxSoldiers", instance.warbandMaxSoldiers, ES3Type_int.Instance);
			writer.WriteProperty("warbandVault", instance.warbandVault, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<MagicItemScriptable>)));
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (PlayerWarband)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "warbandName":
						instance.warbandName = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "warbandWizard":
						instance.warbandWizard = reader.Read<PlayerWizard>(ES3UserType_PlayerWizard.Instance);
						break;
					case "warbandSoldiers":
						instance.warbandSoldiers = reader.Read<System.Collections.Generic.List<SoldierScriptable>>();
						break;
					case "warbandGold":
						instance.warbandGold = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "warbandMaxSoldiers":
						instance.warbandMaxSoldiers = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "warbandVault":
						instance.warbandVault = reader.Read<System.Collections.Generic.List<MagicItemScriptable>>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new PlayerWarband();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_PlayerWarbandArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_PlayerWarbandArray() : base(typeof(PlayerWarband[]), ES3UserType_PlayerWarband.Instance)
		{
			Instance = this;
		}
	}
}