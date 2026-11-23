
using System.Text.Json.Serialization;

namespace ArmEncoder.Sample.Tags;


record MyJson(int A, string S);

[JsonSerializable(typeof(MyJson))]
partial class MyJsonSerializerContext : JsonSerializerContext
{

}
