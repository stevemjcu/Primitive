using System.ComponentModel;

namespace Primitive.Cli
{
    public class ResourceDescriptionAttribute(string key)
        : DescriptionAttribute(Resources.ResourceManager.GetString(key)!)
    { }
}
