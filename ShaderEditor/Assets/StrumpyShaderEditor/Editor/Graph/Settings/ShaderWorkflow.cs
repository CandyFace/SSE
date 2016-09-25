using UnityEngine;
using System;
using System.Collections;
using System.Runtime.Serialization;

namespace StrumpyShaderEditor
{
    [DataContract(Namespace = "http://strumpy.net/ShaderEditor/")]
    public enum ShaderWorkflow
    {
        [EnumMember] Metallic,
        [EnumMember] Specular,
    }

    public static class ShaderWorkflowExtensions
    {
        public static string WorkflowString(this ShaderWorkflow targetEnum)
        {
            switch (targetEnum)
            {
                case ShaderWorkflow.Metallic:
                    return "Standard";
                case ShaderWorkflow.Specular:
                    return "StandardSpecular";
                default:
                    throw new Exception("Unsupported Type");
            }
        }
    }

}
