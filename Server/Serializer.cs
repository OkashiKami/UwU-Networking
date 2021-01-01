using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace UServer
{
	public static class Serializer
	{
		public static byte[] ToByteArray<T>(this T graph)
		{
			using (var ms = new MemoryStream())
			{
				graph.Serialize(ms);

				return ms.ToArray();
			}
		}

		public static T FromByteArray<T>(this byte[] serialized)
		{
			using (var ms = new MemoryStream(serialized))
			{
				return ms.DeSerialize<T>();
			}
		}

		public static void Serialize<T>(this T graph, Stream target)
		{
			// create the formatter:
			IFormatter formatter = new BinaryFormatter();

			// set the binder to the custom binder:
			formatter.Binder = TypeOnlyBinder.Default;

			// serialize the object into the stream:
			formatter.Serialize(target, graph);

		}

		public static T DeSerialize<T>(this Stream source)
		{
			// create the formatter:
			IFormatter formatter = new BinaryFormatter();

			// set the binder to the custom binder:
			formatter.Binder = TypeOnlyBinder.Default;

			// serialize the object into the stream:
			return (T)formatter.Deserialize(source);
		}

		/// <summary>
		/// removes assembly name from type resolution
		/// </summary>
		public class TypeOnlyBinder : SerializationBinder
		{
			/// <summary>
			/// look up the type locally if the assembly-name is "NA"
			/// </summary>
			/// <param name="assemblyName"></param>
			/// <param name="typeName"></param>
			/// <returns></returns>
			public override Type BindToType(string assemblyName, string typeName)
			{
				if (assemblyName.Equals("NA"))
					return Type.GetType(typeName);
				else
					return defaultBinder.BindToType(assemblyName, typeName);
			}

			/// <summary>
			/// override BindToName in order to strip the assembly name. Setting assembly name to null does nothing.
			/// </summary>
			/// <param name="serializedType"></param>
			/// <param name="assemblyName"></param>
			/// <param name="typeName"></param>
			public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
			{
				// specify a neutral code for the assembly name to be recognized by the BindToType method.
				assemblyName = "NA";
				typeName = serializedType.FullName;
			}

			private static SerializationBinder defaultBinder = new BinaryFormatter().Binder;

			private static object locker = new object();
			private static TypeOnlyBinder _default = null;

			public static TypeOnlyBinder Default
			{
				get
				{
					lock (locker)
					{
						if (_default == null)
							_default = new TypeOnlyBinder();
					}
					return _default;
				}
			}
		}


	}
}
