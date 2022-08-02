using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("referenceSpell", "currentWizardLevelMod", "wizardSchoolMod")]
	public class ES3UserType_WizardRuntimeSpell : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_WizardRuntimeSpell() : base(typeof(WizardRuntimeSpell)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (WizardRuntimeSpell)obj;
			
			writer.WriteProperty("referenceSpell", instance.referenceSpell, ES3UserType_SaveSpellRuntime.Instance);
			writer.WriteProperty("currentWizardLevelMod", instance.currentWizardLevelMod, ES3Type_int.Instance);
			writer.WriteProperty("wizardSchoolMod", instance.wizardSchoolMod, ES3Type_int.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (WizardRuntimeSpell)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "referenceSpell":
						instance.referenceSpell = reader.Read<SaveSpellRuntime>(ES3UserType_SaveSpellRuntime.Instance);
						break;
					case "currentWizardLevelMod":
						instance.currentWizardLevelMod = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "wizardSchoolMod":
						instance.wizardSchoolMod = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new WizardRuntimeSpell();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_WizardRuntimeSpellArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_WizardRuntimeSpellArray() : base(typeof(WizardRuntimeSpell[]), ES3UserType_WizardRuntimeSpell.Instance)
		{
			Instance = this;
		}
	}
}