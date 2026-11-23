using StdUnit.Tags;
using System.Reflection;

namespace StdUnit.ArmEncoder.Schemas;

/// <summary>
/// UsbKeyboardCodeScanner XSD Schema Provider
/// </summary>
public sealed class ArmEncoderSchemaProvider : ITagsProjectSchemaProvider
{

    /// <inheritdoc/>
    public IEnumerable<(string LogicalName, string Content)> GetSchemaContents()
    {
        using var stream =Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("StdUnit.ArmEncoder.Schemas.ArmEncoder.xsd");
        if(stream is null)
        {
            throw new FileNotFoundException("ArmEncoder.xsd not Embedded");
        }
        using var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();
        yield return ("ArmEncoder", content);
    }

}
