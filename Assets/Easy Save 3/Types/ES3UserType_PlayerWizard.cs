using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("playerWizardLevel", "playerWizardExperience", "wizardProfilekey", "playerWizardSpellbook")]
	public class ES3UserType_PlayerWizard : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_PlayerWizard() : base(typeof(PlayerWizard)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (PlayerWizard)obj;
			
			writer.WriteProperty("playerWizardLevel", instance.playerWizardLevel, ES3Type_int.Instance);
			writer.WriteProperty("playerWizardExperience", instance.playerWizardExperience, ES3Type_int.Instance);
			writer.WriteProperty("wizardProfilekey", instance.wizardProfilekey, ES3Type_string.Instance);
			writer.WriteProperty("playerWizardSpellbook", instance.playerWizardSpellbook, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(WizardSpellbook)));
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
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
					case "wizardProfilekey":
						instance.wizardProfilekey = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "playerWizardSpellbook":
						instance.playerWizardSpellbook = reader.Read<WizardSpellbook>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new PlayerWizard();
			ReadObject<T>(reader, instance);
			return instance;
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