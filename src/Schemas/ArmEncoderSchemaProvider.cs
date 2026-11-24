using System.Reflection;

namespace StdUnit.Tags.ArmEncoder.Schemas;

/// <summary>
/// ArmEncoder XSD Schema Provider
/// </summary>
public sealed class ArmEncoderSchemaProvider : ITagsProjectSchemaProvider
{

    /// <inheritdoc/>
    public IEnumerable<(string LogicalName, string Content)> GetSchemaContents()
    {
        using var stream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("StdUnit.Tags.ArmEncoder.Schemas.ArmEncoder.xsd");
        if (stream is null)
        {
            throw new FileNotFoundException("ArmEncoder.xsd not Embedded");
        }
        using var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();
        yield return ("ArmEncoder", content);
    }

}
