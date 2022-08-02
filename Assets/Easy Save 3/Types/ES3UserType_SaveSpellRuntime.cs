using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("Name", "CastingNumber", "School", "Restriction", "Description", "bookEdition")]
	public class ES3UserType_SaveSpellRuntime : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_SaveSpellRuntime() : base(typeof(SaveSpellRuntime)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (SaveSpellRuntime)obj;
			
			writer.WriteProperty("Name", instance.Name, ES3Type_string.Instance);
			writer.WriteProperty("CastingNumber", instance.CastingNumber, ES3Type_int.Instance);
			writer.WriteProperty("School", instance.School, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(WizardSchools)));
			writer.WriteProperty("Restriction", instance.Restriction, ES3Type_string.Instance);
			writer.WriteProperty("Description", instance.Description, ES3Type_string.Instance);
			writer.WriteProperty("bookEdition", instance.bookEdition, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(FrostgraveBook)));
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (SaveSpellRuntime)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "Name":
						instance.Name = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "CastingNumber":
						instance.CastingNumber = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "School":
						instance.School = reader.Read<WizardSchools>();
						break;
					case "Restriction":
						instance.Restriction = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "Description":
						instance.Description = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "bookEdition":
						instance.bookEdition = reader.Read<FrostgraveBook>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new SaveSpellRuntime();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_SaveSpellRuntimeArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_SaveSpellRuntimeArray() : base(typeof(SaveSpellRuntime[]), ES3UserType_SaveSpellRuntime.Instance)
		{
			Instance = this;
		}
	}
}