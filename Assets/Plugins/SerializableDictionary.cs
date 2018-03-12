using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

namespace Utility
{
	[Serializable]
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
	{
		public SerializableDictionary () { }
		public void WriteXml (XmlWriter write)       // Serializer  
		{
			XmlSerializer KeySerializer = new XmlSerializer (typeof (TKey));
			XmlSerializer ValueSerializer = new XmlSerializer (typeof (TValue));

			foreach (KeyValuePair<TKey, TValue> kv in this) {
				write.WriteStartElement ("SerializableDictionary");
				write.WriteStartElement ("key");
				KeySerializer.Serialize (write, kv.Key);
				write.WriteEndElement ();
				write.WriteStartElement ("value");
				ValueSerializer.Serialize (write, kv.Value);
				write.WriteEndElement ();
				write.WriteEndElement ();
			}
		}
		public void ReadXml (XmlReader reader)       // Deserializer  
		{
			reader.Read ();
			XmlSerializer KeySerializer = new XmlSerializer (typeof (TKey));
			XmlSerializer ValueSerializer = new XmlSerializer (typeof (TValue));

			while (reader.NodeType != XmlNodeType.EndElement) {
				reader.ReadStartElement ("SerializableDictionary");
				reader.ReadStartElement ("key");
				TKey tk = (TKey)KeySerializer.Deserialize (reader);
				reader.ReadEndElement ();
				reader.ReadStartElement ("value");
				TValue vl = (TValue)ValueSerializer.Deserialize (reader);
				reader.ReadEndElement ();
				reader.ReadEndElement ();
				this.Add (tk, vl);
				reader.MoveToContent ();
			}
			reader.ReadEndElement ();

		}
		public XmlSchema GetSchema ()
		{
			return null;
		}
	}
}

//3.使用

//  a.定义SerializableDictionary对象，这里以存储<string, string> 键对为例：

//[csharp] view plaincopy

//SerializableDictionary<string, string> serializableDictionary = new SerializableDictionary<string, string> ();
//b.添加元素

//[csharp] view plaincopy

//serializableDictionary.Add ("Key1", “Value1”);  
//......  
//  c.序列化

//[csharp] view plaincopy

//using (FileStream fileStream = new FileStream (fileName, FileMode.Create))  
//{  
//    XmlSerializer xmlFormatter = new XmlSerializer (typeof (SerializableDictionary<string, string>));
//xmlFormatter.Serialize(fileStream, this.serializableDictionary);
//}
//注：文件名fileName自己定义，如“file.xml”
//  d.反序列化

//[csharp] view plaincopy

//using (FileStream fileStream = new FileStream (fileName, FileMode.Open))  
//{  
//    XmlSerializer xmlFormatter = new XmlSerializer (typeof (SerializableDictionary<string, string>));

//	this.serializableDictionary = (SerializableDictionary<string, string>)xmlFormatter.Deserialize (fileStream);  
//}
