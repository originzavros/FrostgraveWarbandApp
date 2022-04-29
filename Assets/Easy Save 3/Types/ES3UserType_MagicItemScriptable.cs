using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("itemName", "itemDescription", "itemPurchasePrice", "itemSalePrice", "itemBook", "itemType")]
	public class ES3UserType_MagicItemScriptable : ES3ScriptableObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_MagicItemScriptable() : base(typeof(MagicItemScriptable)){ Instance = this; priority = 1; }


		protected override void WriteScriptableObject(object obj, ES3Writer writer)
		{
			var instance = (MagicItemScriptable)obj;
			
			writer.WriteProperty("itemName", instance.itemName, ES3Type_string.Instance);
			writer.WriteProperty("itemDescription", instance.itemDescription, ES3Type_string.Instance);
			writer.WriteProperty("itemPurchasePrice", instance.itemPurchasePrice, ES3Type_int.Instance);
			writer.WriteProperty("itemSalePrice", instance.itemSalePrice, ES3Type_int.Instance);
			writer.WriteProperty("itemBook", instance.itemBook, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(FrostgraveBook)));
			writer.WriteProperty("itemType", instance.itemType, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(MagicItemType)));
		}

		protected override void ReadScriptableObject<T>(ES3Reader reader, object obj)
		{
			var instance = (MagicItemScriptable)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "itemName":
						instance.itemName = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "itemDescription":
						instance.itemDescription = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "itemPurchasePrice":
						instance.itemPurchasePrice = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "itemSalePrice":
						instance.itemSalePrice = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "itemBook":
						instance.itemBook = reader.Read<FrostgraveBook>();
						break;
					case "itemType":
						instance.itemType = reader.Read<MagicItemType>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_MagicItemScriptableArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_MagicItemScriptableArray() : base(typeof(MagicItemScriptable[]), ES3UserType_MagicItemScriptable.Instance)
		{
			Instance = this;
		}
	}
}