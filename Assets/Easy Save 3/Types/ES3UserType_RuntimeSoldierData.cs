using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("soldierName", "inventoryLimit", "move", "fight", "shoot", "armor", "will", "health", "cost", "hiringName", "soldierType", "isHired", "description", "bookEdition", "baseSoldierEquipment", "soldierInventory")]
	public class ES3UserType_RuntimeSoldierData : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_RuntimeSoldierData() : base(typeof(RuntimeSoldierData)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (RuntimeSoldierData)obj;
			
			writer.WriteProperty("soldierName", instance.soldierName, ES3Type_string.Instance);
			writer.WriteProperty("inventoryLimit", instance.inventoryLimit, ES3Type_int.Instance);
			writer.WriteProperty("move", instance.move, ES3Type_int.Instance);
			writer.WriteProperty("fight", instance.fight, ES3Type_int.Instance);
			writer.WriteProperty("shoot", instance.shoot, ES3Type_int.Instance);
			writer.WriteProperty("armor", instance.armor, ES3Type_int.Instance);
			writer.WriteProperty("will", instance.will, ES3Type_int.Instance);
			writer.WriteProperty("health", instance.health, ES3Type_int.Instance);
			writer.WriteProperty("cost", instance.cost, ES3Type_int.Instance);
			writer.WriteProperty("hiringName", instance.hiringName, ES3Type_string.Instance);
			writer.WriteProperty("soldierType", instance.soldierType, ES3Type_string.Instance);
			writer.WriteProperty("isHired", instance.isHired, ES3Type_bool.Instance);
			writer.WriteProperty("description", instance.description, ES3Type_string.Instance);
			writer.WriteProperty("bookEdition", instance.bookEdition, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(FrostgraveBook)));
			writer.WriteProperty("baseSoldierEquipment", instance.baseSoldierEquipment, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<EquipmentScriptable>)));
			writer.WriteProperty("soldierInventory", instance.soldierInventory, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<MagicItemScriptable>)));
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (RuntimeSoldierData)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "soldierName":
						instance.soldierName = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "inventoryLimit":
						instance.inventoryLimit = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "move":
						instance.move = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "fight":
						instance.fight = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "shoot":
						instance.shoot = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "armor":
						instance.armor = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "will":
						instance.will = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "health":
						instance.health = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "cost":
						instance.cost = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "hiringName":
						instance.hiringName = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "soldierType":
						instance.soldierType = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "isHired":
						instance.isHired = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "description":
						instance.description = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "bookEdition":
						instance.bookEdition = reader.Read<FrostgraveBook>();
						break;
					case "baseSoldierEquipment":
						instance.baseSoldierEquipment = reader.Read<System.Collections.Generic.List<EquipmentScriptable>>();
						break;
					case "soldierInventory":
						instance.soldierInventory = reader.Read<System.Collections.Generic.List<MagicItemScriptable>>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new RuntimeSoldierData();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_RuntimeSoldierDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_RuntimeSoldierDataArray() : base(typeof(RuntimeSoldierData[]), ES3UserType_RuntimeSoldierData.Instance)
		{
			Instance = this;
		}
	}
}