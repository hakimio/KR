Shader "Petes/Solid Transparent Color" {

Properties {
	_Color ("Solid Color (A = Opacity)", Color) = (0,0,0,1)	
}

Subshader {
	Tags {Queue = Transparent}
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha 
	
	Color [_Color]	
	Pass {}
}

}