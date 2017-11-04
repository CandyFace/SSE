using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEditor;
using System;

namespace StrumpyShaderEditor
{
	[DataContract(Namespace = "http://strumpy.net/ShaderEditor/")]
	[NodeMetaData("Tex2D", "Function", typeof(Tex2DNode),"Read a texture sampler (Non-Cube) at the given UV-position. This operation is much more expensive if dependent on another texture read. Used with Sampler2D, primarily when reading offset UV's or reading multiple times from the same texture. To use the default UV, use Sampled2D instead.")]
	public class Tex2DNode : Node, IResultCacheNode{
		private const string NodeName = "Tex2D";

        [DataMember] private Float4OutputChannel _rgba;
        [DataMember] private Float4OutputChannel _r;
        [DataMember] private Float4OutputChannel _g;
        [DataMember] private Float4OutputChannel _b;
        [DataMember] private Float4OutputChannel _a;
		
		[DataMember] private Sampler2DInputChannel _sampler;
		[DataMember] private Float4InputChannel _uv;

        [DataMember] private EditorBool _split;

        public Tex2DNode()
		{
			Initialize();
		}
		
		public override sealed void Initialize ()
		{
			_rgba = _rgba ?? new Float4OutputChannel( 0, "RGBA" );
            _a = _a ?? new Float4OutputChannel(1, "A");
            _r = _r ?? new Float4OutputChannel(2, "R");
            _g = _g ?? new Float4OutputChannel(3, "G");
            _b = _b ?? new Float4OutputChannel(4, "B");

            _sampler = _sampler ?? new Sampler2DInputChannel( 0, "Sampler" );
			_uv = _uv ?? new Float4InputChannel( 1, "UV", Vector4.zero );
		}
		
		public override IEnumerable<string> IsValid ( SubGraphType graphType )
		{
			var errors = new List<string> ();
			if( graphType == SubGraphType.Vertex )
			{
				errors.Add( "Node not valid in graph type: " + graphType );
			}
			return errors;
		}
		
		public override string NodeTypeName
		{
			get{ return NodeName; }
		}

		protected override IEnumerable<OutputChannel> GetOutputChannels()
		{
            var ret = new List<OutputChannel> { _rgba };
            if (_split)
            {
                ret.Add(_r);
                ret.Add(_g);
                ret.Add(_b);
                ret.Add(_a);
            }
            return ret;
        }

        public override IEnumerable<InputChannel> GetInputChannels()
		{
			return new List<InputChannel> {_sampler, _uv};
		}

        public override void DrawProperties()
        {
            _split = EditorGUILayout.Toggle("Split", _split);
            base.DrawProperties();
        }

        public string GetAdditionalFields()
		{
			string result = "";
			if( IsInputChannelConnected( 0 ) )
			{
				var samplerInput = _sampler.ChannelInput( this );
				var uvInput = _uv.ChannelInput( this );
				
				result += samplerInput.AdditionalFields;
				result += uvInput.AdditionalFields;
			}
			return result;
		}
		
		public string GetUsage()
		{
			if( IsInputChannelConnected( 0 ) )
			{
				var samplerInput = _sampler.ChannelInput( this );
				var uvInput = _uv.ChannelInput( this );
				
				var result = "float4 ";
				result += UniqueNodeIdentifier;
				result += "=";
				result += "tex2D(" + samplerInput.QueryResult + "," + uvInput.QueryResult + ".xy);\n";
				return result;
			}
			else
			{
				var result = "";
				result += "float4 ";
				result += UniqueNodeIdentifier;
				result += "=float4(0.0,0.0,0.0,0.0);\n";
				return result;
			}
		}
		
		public override string GetExpression( uint channelId )
		{
			AssertOutputChannelExists( channelId );
            switch (channelId)
            {
                default:
                case 0: return UniqueNodeIdentifier;
                case 1: return String.Format("float4(0,0,0,{0}.a)", UniqueNodeIdentifier);
                case 2: return String.Format("float4({0}.r,0,0,0)", UniqueNodeIdentifier);
                case 3: return String.Format("float4(0,{0}.g,0,0)", UniqueNodeIdentifier);
                case 4: return String.Format("float4(0,0,{0}.b,0)", UniqueNodeIdentifier);
            }
        }
    }
}