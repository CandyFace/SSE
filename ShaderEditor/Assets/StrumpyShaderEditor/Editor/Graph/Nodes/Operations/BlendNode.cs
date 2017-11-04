using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEditor;

namespace StrumpyShaderEditor
{
    [DataContract(Namespace = "http://strumpy.net/ShaderEditor/")]
    public enum BlendFunction
    {
        [EnumMember] Linear,
        [EnumMember] WhiteOut,
        [EnumMember] Overlay,
        
    }
    interface IBlendFunction
    {
        string Exec(string node, string value1, string value2);
    }

    class BlendFunctionLinear : IBlendFunction
    {
        public string Exec(string node, string value1, string value2)
        {
            return String.Format("float4 {0} = ({1} + {2})*2 - 2;\n", node, value1, value2);
        }
    }
    class BlendFunctionWhiteOut : IBlendFunction
    {
        public string Exec(string node, string value1, string value2)
        {
            var n1 = node + "_1";
            var n2 = node + "_2";

            var res = "";
            res += String.Format("float4 {0} = {1};\n", n1, value1);
            res += String.Format("float4 {0} = {1};\n", n2, value2);

            res += String.Format("{0} = {0} * 2 - 1;\n", n1);
            res += String.Format("{0} = {0} * 2 - 1;\n", n2);
            res += String.Format("float4 {0} = float4({1}.xy + {2}.xy, {1}.z * {2}.z, 0);\n", node, n1, n2);
            return res;
        }
    }
    class BlendFunctionOverlay : IBlendFunction
    {
        public string Exec(string node, string value1, string value2)
        {
            var n1 = node + "_1";
            var n2 = node + "_2";
            var a = node + "_3";
            var b = node + "_4";

            var res = "";
            res += String.Format("float4 {0} = {1};\n", n1, value1);
            res += String.Format("float4 {0} = {1};\n", n2, value2);

            res += String.Format("{0} = {0} * 4 - 2;\n", n1);                   // n1 = n1 * 4 - 2;
            res += String.Format("float4 {0} = {1} >= 0 ? -1 : 1;\n", a, n1);   // float4 a = n1 >= 0 ? -1 : 1;
            res += String.Format("float4 {0} = {1} >= 0 ? 1 : 0;\n", b, n1);    // float4 b = n1 >= 0 ? 1 : 0;
            res += String.Format("{0} = 2 * {1} + {0};\n", n1, a);              // n1 = 2 * a + n1;
            res += String.Format("{0} = {0} * {1} + {2};\n", n2, a, b);         // n2 = n2 * a + b;

            res += String.Format("float4 {0} = {1} * {2} - {3};\n", node, n1, n2, a);  // float3 r = n1 * n2 - a;
            return res;
        }
    }

    [DataContract(Namespace = "http://strumpy.net/ShaderEditor/")]
	[NodeMetaData("Blend", "Function", typeof(BlendNode),"")]
	public class BlendNode : Node, IResultCacheNode
    {
		private const string NodeName = "Blend";

        [DataMember] private BlendFunction blendFunc;
        [DataMember] private Float4InputChannel _arg1;
        [DataMember] private Float4InputChannel _arg2;
        [DataMember] private Float4OutputChannel _result;

        [DataMember] private EditorBool _normalize;

        public BlendNode()
        {
            Initialize();
        }

        public override void Initialize()
        {
            _result = _result ?? new Float4OutputChannel(0, "Result");
            _arg1 = _arg1 ?? new Float4InputChannel(0, "A", Vector4.zero);
            _arg2 = _arg2 ?? new Float4InputChannel(1, "B", Vector4.zero);
        }

        public override string NodeTypeName
		{
			get{ return NodeName; }
		}

        public override string DisplayName
        {
            get { return NodeName + "(" + blendFunc.ToString() + ")"; }
        }

        public override void DrawProperties()
        {
            var blendContent = new GUIContent("Blend Func", "Select the blend function");
            blendFunc = (BlendFunction)EditorGUILayout.EnumPopup(blendContent, blendFunc);
            _normalize = EditorGUILayout.Toggle("Normalize Result", _normalize);
            base.DrawProperties();
        }

        public string GetAdditionalFields()
        {
            var arg1Input = _arg1.ChannelInput(this);
            var arg2Input = _arg2.ChannelInput(this);

            var ret = arg1Input.AdditionalFields;
            ret += arg2Input.AdditionalFields;
            return ret;
        }

        public override string GetExpression(uint channelId)
        {
            AssertOutputChannelExists(channelId);
            return UniqueNodeIdentifier;
        }

        public override IEnumerable<InputChannel> GetInputChannels()
        {
            return new List<InputChannel> { _arg1, _arg2 };
        }


        protected override IEnumerable<OutputChannel> GetOutputChannels()
        {
            return new List<OutputChannel> { _result };
        }

        public string GetUsage()
        {
            var arg1 = _arg1.ChannelInput(this);
            var arg2 = _arg2.ChannelInput(this);

            IBlendFunction func;
            switch (blendFunc)
            {
                default:
                case BlendFunction.Linear:
                    func = new BlendFunctionLinear();
                    break;
                case BlendFunction.WhiteOut:
                    func = new BlendFunctionWhiteOut();
                    break;
                case BlendFunction.Overlay:
                    func = new BlendFunctionOverlay();
                    break;
            }
            string result = func.Exec(UniqueNodeIdentifier, arg1.QueryResult, arg2.QueryResult);
            if (_normalize) result += String.Format("{0} = normalize({0});\n", UniqueNodeIdentifier);
            return result;
        }


    }
}

