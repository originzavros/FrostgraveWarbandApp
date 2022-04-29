using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("playerWizardLevel", "playerWizardExperience", "playerWizardSpellbook", "soldierName", "inventoryLimit", "move", "fight", "shoot", "armor", "will", "health", "cost", "hiringName", "soldierType", "isHired", "description", "bookEdition", "baseSoldierEquipment", "soldierInventory")]
	public class ES3UserType_PlayerWizard : ES3ScriptableObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_PlayerWizard() : base(typeof(PlayerWizard)){ Instance = this; priority = 1; }


		protected override void WriteScriptableObject(object obj, ES3Writer writer)
		{
			var instance = (PlayerWizard)obj;
			
			writer.WriteProperty("playerWizardLevel", instance.playerWizardLevel, ES3Type_int.Instance);
			writer.WriteProperty("playerWizardExperience", instance.playerWizardExperience, ES3Type_int.Instance);
			writer.WriteProperty("playerWizardSpellbook", instance.playerWizardSpellbook, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(WizardSpellbook)));
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

		protected override void ReadScriptableObject<T>(ES3Reader reader, object obj)
		{
			var instance = (PlayerWizard)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "playerWizardLevel":
						instance.playerWizardLevel = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "playerWizardExperience":
						instance.playerWizardExperience = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "playerWizardSpellbook":
						instance.playerWizardSpellbook = reader.Read<WizardSpellbook>();
						break;
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
	}


	public class ES3UserType_PlayerWizardArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_PlayerWizardArray() : base(typeof(PlayerWizard[]), ES3UserType_PlayerWizard.Instance)
		{
			Instance = this;
		}
	}
}