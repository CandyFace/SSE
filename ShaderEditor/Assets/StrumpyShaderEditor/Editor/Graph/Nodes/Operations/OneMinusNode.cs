using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StrumpyShaderEditor
{
	[DataContract(Namespace = "http://strumpy.net/ShaderEditor/")]
	[NodeMetaData("1-v", "Operation", typeof(OneMinus),"Invert via the one minus approach, Used to find the inverse for numbers in the [0,1] range, in particular textures. Used often with blending. To sanitize input, combine with saturate. Shorthand for subtract, no performance difference over simply subtracting.")]
	public class OneMinus : FunctionOneInput {
		private const string NodeName = "Invert";

        protected override void OnInitialized()
        {
            _result.DisplayName = "1-Value";
            _arg1.DisplayName = "Value";
            base.OnInitialized();
        }

        public override string NodeTypeName
		{
			get{ return NodeName; }
		}

        public override string FunctionName
        {
            get { return ""; }
        }

        public override string GetUsage()
		{
			var arg1 = _arg1.ChannelInput( this );
			var result = "float4 ";
			result += UniqueNodeIdentifier;
			result += "=";
			result += " float4(1.0, 1.0, 1.0, 1.0) - " + arg1.QueryResult + ";\n";
			return result;
		}
	}
}