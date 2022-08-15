using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("soldierName", "inventoryLimit", "move", "fight", "shoot", "armor", "will", "health", "cost", "hiringName", "soldierType", "isHired", "description", "status", "bookEdition", "baseSoldierEquipment", "soldierInventory", "monsterKeywordList")]
	public class ES3UserType_SoldierScriptable : ES3ScriptableObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_SoldierScriptable() : base(typeof(SoldierScriptable)){ Instance = this; priority = 1; }


		protected override void WriteScriptableObject(object obj, ES3Writer writer)
		{
			var instance = (SoldierScriptable)obj;
			
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
			writer.WriteProperty("status", instance.status, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(SoldierStatus)));
			writer.WriteProperty("bookEdition", instance.bookEdition, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(FrostgraveBook)));
			writer.WriteProperty("baseSoldierEquipment", instance.baseSoldierEquipment, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<EquipmentScriptable>)));
			writer.WriteProperty("soldierInventory", instance.soldierInventory, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<MagicItemScriptable>)));
			writer.WriteProperty("monsterKeywordList", instance.monsterKeywordList, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<MonsterKeywordScriptable>)));
		}

		protected override void ReadScriptableObject<T>(ES3Reader reader, object obj)
		{
			var instance = (SoldierScriptable)obj;
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
					case "status":
						instance.status = reader.Read<SoldierStatus>();
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
					case "monsterKeywordList":
						instance.monsterKeywordList = reader.Read<System.Collections.Generic.List<MonsterKeywordScriptable>>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_SoldierScriptableArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_SoldierScriptableArray() : base(typeof(SoldierScriptable[]), ES3UserType_SoldierScriptable.Instance)
		{
			Instance = this;
		}
	}
}