using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("wizardSpellbookSpells", "wizardSchool")]
	public class ES3UserType_WizardSpellbook : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_WizardSpellbook() : base(typeof(WizardSpellbook)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (WizardSpellbook)obj;
			
			writer.WriteProperty("wizardSpellbookSpells", instance.wizardSpellbookSpells, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<WizardRuntimeSpell>)));
			writer.WritePropertyByRef("wizardSchool", instance.wizardSchool);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (WizardSpellbook)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "wizardSpellbookSpells":
						instance.wizardSpellbookSpells = reader.Read<System.Collections.Generic.List<WizardRuntimeSpell>>();
						break;
					case "wizardSchool":
						instance.wizardSchool = reader.Read<WizardSchoolScriptable>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new WizardSpellbook();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_WizardSpellbookArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_WizardSpellbookArray() : base(typeof(WizardSpellbook[]), ES3UserType_WizardSpellbook.Instance)
		{
			Instance = this;
		}
	}
}