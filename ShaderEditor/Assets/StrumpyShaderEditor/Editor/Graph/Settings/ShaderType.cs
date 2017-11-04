using UnityEngine;
using System;
using System.Collections;
using System.Runtime.Serialization;

namespace StrumpyShaderEditor
{
	[DataContract(Namespace = "http://strumpy.net/ShaderEditor/")]
	public enum ShaderType {
		[EnumMember] Standard,          // SurfaceOutput
        [EnumMember] PBR,               // SurfaceOutputStandard
        [EnumMember] PBR_Specular,      // SurfaceOutputStandardSpecular
    }

    public static class ShaderTypeExtensions
    {
		public static string TypeString(this ShaderType targetEnum)
		{
			switch (targetEnum)
			{
				case ShaderType.Standard:
					return "";
                case ShaderType.PBR:
                    return "Standard";
                case ShaderType.PBR_Specular:
                    return "StandardSpecular";
                default:
					throw new Exception("Unsupported Type");
			}
		}
	}
 
}
