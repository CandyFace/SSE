using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StrumpyShaderEditor
{
	[DataContract(Namespace = "http://strumpy.net/ShaderEditor/")]
	public abstract class FunctionThreeInput : Node, IFunction {
		[DataMember] private Float4OutputChannel _result = new Float4OutputChannel( 0, "Result" );
		[DataMember] private Float4InputChannel _arg1;
		[DataMember] private Float4InputChannel _arg2;
		[DataMember] private Float4InputChannel _arg3;

        string resultDisplayName = "Result";
        string arg1DisplayName = "Arg1";
        string arg2DisplayName = "Arg2";
        string arg3DisplayName = "Arg2";

        protected FunctionThreeInput()
	    {
			Initialize();
		}

        protected FunctionThreeInput(string resultDisplayName, string arg1DisplayName, string arg2DisplayName, string arg3DisplayName)
        {
            this.resultDisplayName = resultDisplayName;
            this.arg1DisplayName = arg1DisplayName;
            this.arg2DisplayName = arg2DisplayName;
            this.arg3DisplayName = arg3DisplayName;
            Initialize();
        }

        public override sealed void Initialize ()
		{
			_result = _result ?? new Float4OutputChannel( 0, resultDisplayName);
			_arg1 = _arg1 ?? new Float4InputChannel( 0, arg1DisplayName, Vector4.zero );
			_arg2 = _arg2 ?? new Float4InputChannel( 1, arg2DisplayName, Vector4.zero );
			_arg3 = _arg3 ?? new Float4InputChannel( 2, arg3DisplayName, Vector4.zero );
		}
		
		public abstract string FunctionName
		{
			get;
		}
		
		
		public string GetFunctionDefinition()
		{
			return "";
		}

		protected override IEnumerable<OutputChannel> GetOutputChannels()
		{
			var ret = new List<OutputChannel> {_result};
		    return ret;
		}
		
		public override IEnumerable<InputChannel> GetInputChannels()
		{
			var ret = new List<InputChannel> {_arg1, _arg2, _arg3};
		    return ret;
		}
		
		public string GetAdditionalFields()
		{
			var arg1Input = _arg1.ChannelInput( this );
			var arg2Input = _arg2.ChannelInput( this );
			var arg3Input = _arg3.ChannelInput( this );
			
			var ret = arg1Input.AdditionalFields;
			ret += arg2Input.AdditionalFields;
			ret += arg3Input.AdditionalFields;
			return ret;
		}
		
		public string GetUsage()
		{
			var arg1Input = _arg1.ChannelInput( this );
			var arg2Input = _arg2.ChannelInput( this );
			var arg3Input = _arg3.ChannelInput( this );
			
			string ret = "float4 ";
			ret += UniqueNodeIdentifier;
			ret += "=";
			ret += FunctionName + "(" + arg1Input.QueryResult + "," + arg2Input.QueryResult + "," + arg3Input.QueryResult + ");\n";
			return ret;
		}
		
		public override string GetExpression( uint channelId )
		{
			AssertOutputChannelExists( channelId );
			return UniqueNodeIdentifier;
		}
	}
}