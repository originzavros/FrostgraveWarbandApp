using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("Instance", "type", "isPrimitive", "isValueType", "isCollection", "isDictionary", "isEnum", "isES3TypeUnityObject", "isReflectedType", "isUnsupported", "priority")]
	public class ES3UserType_ES3UserType_RuntimeSoldierData : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_ES3UserType_RuntimeSoldierData() : base(typeof(ES3Types.ES3UserType_RuntimeSoldierData)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (ES3Types.ES3UserType_RuntimeSoldierData)obj;
			
			writer.WriteProperty("Instance", ES3Types.ES3UserType_RuntimeSoldierData.Instance, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(ES3Types.ES3Type)));
			writer.WriteProperty("type", instance.type, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Type)));
			writer.WriteProperty("isPrimitive", instance.isPrimitive, ES3Type_bool.Instance);
			writer.WriteProperty("isValueType", instance.isValueType, ES3Type_bool.Instance);
			writer.WriteProperty("isCollection", instance.isCollection, ES3Type_bool.Instance);
			writer.WriteProperty("isDictionary", instance.isDictionary, ES3Type_bool.Instance);
			writer.WriteProperty("isEnum", instance.isEnum, ES3Type_bool.Instance);
			writer.WriteProperty("isES3TypeUnityObject", instance.isES3TypeUnityObject, ES3Type_bool.Instance);
			writer.WriteProperty("isReflectedType", instance.isReflectedType, ES3Type_bool.Instance);
			writer.WriteProperty("isUnsupported", instance.isUnsupported, ES3Type_bool.Instance);
			writer.WriteProperty("priority", instance.priority, ES3Type_int.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (ES3Types.ES3UserType_RuntimeSoldierData)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "Instance":
						ES3Types.ES3UserType_RuntimeSoldierData.Instance = reader.Read<ES3Types.ES3Type>();
						break;
					case "type":
						instance.type = reader.Read<System.Type>();
						break;
					case "isPrimitive":
						instance.isPrimitive = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "isValueType":
						instance.isValueType = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "isCollection":
						instance.isCollection = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "isDictionary":
						instance.isDictionary = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "isEnum":
						instance.isEnum = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "isES3TypeUnityObject":
						instance.isES3TypeUnityObject = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "isReflectedType":
						instance.isReflectedType = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "isUnsupported":
						instance.isUnsupported = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "priority":
						instance.priority = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new ES3Types.ES3UserType_RuntimeSoldierData();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_ES3UserType_RuntimeSoldierDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_ES3UserType_RuntimeSoldierDataArray() : base(typeof(ES3Types.ES3UserType_RuntimeSoldierData[]), ES3UserType_ES3UserType_RuntimeSoldierData.Instance)
		{
			Instance = this;
		}
	}
}