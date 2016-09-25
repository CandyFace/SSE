using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace StrumpyShaderEditor
{
	[DataContract(Namespace = "http://strumpy.net/ShaderEditor/")]
	public class ShaderMasterNode : RootNode {
		private const string NodeName = "Master";
		
		[DataMember] private Float4InputChannel _albedo;
		[DataMember] private Float4InputChannel _normal;
        [DataMember] private Float4InputChannel _emission;
        [DataMember] private Float4InputChannel _alpha;

        [DataMember] private Float4InputChannel _metallic;
        [DataMember] private Float4InputChannel _specular;

        // Metallic
        [DataMember] private Float4InputChannel _smoothness;
        [DataMember] private Float4InputChannel _occlusion;
        [DataMember] private Float4InputChannel _clip;

        public ShaderMasterNode( )
		{
			Initialize(); 
		}
		
		public override sealed void Initialize ()
		{
			
			_albedo = _albedo ?? new Float4InputChannel( 0, "Diffuse", Vector4.zero );
			_normal = _normal ?? new Float4InputChannel( 1, "Normal", new Vector4( 0.0f, 0.0f, 1.0f, 1.0f ) );
			_emission = _emission ?? new Float4InputChannel( 2, "Emission", Vector4.zero );
            _specular = _specular ?? new Float4InputChannel(3, "Specular", Vector4.zero);
            _metallic = _metallic ?? new Float4InputChannel(4, "Metallic", Vector4.zero);
            _smoothness = _smoothness ?? new Float4InputChannel(5, "Smoothness", Vector4.zero);
            _occlusion = _occlusion ?? new Float4InputChannel(6, "Occlusion", Vector4.zero);
            _alpha = _alpha ?? new Float4InputChannel( 7, "Alpha", Vector4.one );
            _clip = _clip ?? new Float4InputChannel(8, "Clip", Vector4.one);

            _specular.DisplayName = "Specular";
            _albedo.DisplayName = "Diffuse";
		}

		protected override IEnumerable<OutputChannel> GetOutputChannels()
		{
			return new List<OutputChannel>();
		}

        public override IEnumerable<InputChannel> GetInputChannels()
        {
            return new List<InputChannel>
            {
                _albedo,
                _normal,
                _emission,
                _metallic,
                _specular,
                _smoothness,
                _occlusion,
                _alpha,
                _clip,
            };
        }
		
		public override string NodeTypeName
		{
			get{ return NodeName; }
		}
		
		public override string GetExpression( uint channelId )
		{
			Debug.LogError( "Can not get node based expression from the master node" );
			return "";
		}
		
		public string GetAdditionalFields()
		{
			var albedoInput = _albedo.ChannelInput( this );
			var normalInput = _normal.ChannelInput( this );
			var emissionInput = _emission.ChannelInput( this );
            var metallicInput = _metallic.ChannelInput(this);
            var specularInput = _specular.ChannelInput(this);
            var smoothnessInput = _smoothness.ChannelInput(this);
            var occlusionInput = _occlusion.ChannelInput(this);
            var alphaInput = _alpha.ChannelInput(this);
            var clipInput = _clip.ChannelInput(this);

            var result = albedoInput.AdditionalFields;
			result += normalInput.AdditionalFields;
            result += emissionInput.AdditionalFields;
            result += metallicInput.AdditionalFields;
            result += specularInput.AdditionalFields;
            result += smoothnessInput.AdditionalFields;
            result += occlusionInput.AdditionalFields;
            result += alphaInput.AdditionalFields;
            result += clipInput.AdditionalFields;

            return result;
		}
		
		public bool AlbedoConnected()
		{
			return _albedo.IncomingConnection != null;
		}
		public string GetAlbedoExpression()
		{
			return _albedo.ChannelInput( this ).QueryResult;
		}
		
		public bool NormalConnected()
		{
			return _normal.IncomingConnection != null;
		}
		public string GetNormalExpression()
		{
			return _normal.ChannelInput( this ).QueryResult;
		}
		
		public bool EmissionConnected()
		{
			return _emission.IncomingConnection != null;
		}
		public string GetEmissionExpression()
		{
			return _emission.ChannelInput( this ).QueryResult;
		}

        public bool MetallicConnected()
        {
            return _metallic.IncomingConnection != null;
        }
        public string GetMetallicExpression()
        {
            return _metallic.ChannelInput(this).QueryResult;
        }

        public bool SmoothnessConnected()
        {
            return _smoothness.IncomingConnection != null;
        }
        public string GetSmoothnessExpression()
        {
            return _smoothness.ChannelInput(this).QueryResult;
        }

        public bool OcclusionConnected()
        {
            return _occlusion.IncomingConnection != null;
        }
        public string GetOcclusionExpression()
        {
            return _occlusion.ChannelInput(this).QueryResult;
        }

        public bool SpecularConnected()
        {
            return _specular.IncomingConnection != null;
        }
        public string GetSpecularExpression()
        {
            return _specular.ChannelInput(this).QueryResult;
        }

        public bool AlphaConnected()
		{
			return _alpha.IncomingConnection != null;
		}
		public string GetAlphaExpression()
		{
			return _alpha.ChannelInput( this ).QueryResult;
		}
        public bool ClipConnected()
        {
            return _clip.IncomingConnection != null;
        }
        public string GetClipExpression()
        {
            return _clip.ChannelInput(this).QueryResult;
        }
    }
}
