using System;
using System.Diagnostics;

namespace ff.resource_editor.model
{
    internal enum resource_type
    {
        none,
        effect,
        music,
        animation,
        font_file,
        palette,
        shader,
        sprites,
        font,
        texture,
        input,
        file,
        resource_values,
    }

    internal static class resource_type_utility
    {
        public static string name_of(resource_type type)
        {
            switch (type)
            {
                case resource_type.none:
                    return string.Empty;

                case resource_type.effect:
                case resource_type.music:
                case resource_type.animation:
                case resource_type.font_file:
                case resource_type.palette:
                case resource_type.shader:
                case resource_type.sprites:
                case resource_type.font:
                case resource_type.texture:
                case resource_type.input:
                case resource_type.file:
                case resource_type.resource_values:
                    return Enum.GetName<resource_type>(type);

                default:
                    Debug.Fail("Unknown resource type");
                    return "<invalid>";
            }
        }
    }
}
