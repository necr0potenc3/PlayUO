namespace Client
{
    using Microsoft.DirectX.Direct3D;
    using System;

    public class VertexConstructor
    {
        public static CustomVertex.TransformedColoredTextured[] Create()
        {
            CustomVertex.TransformedColoredTextured[] texturedArray = new CustomVertex.TransformedColoredTextured[4];
            texturedArray[0].Rhw = 1f;
            texturedArray[1].Rhw = 1f;
            texturedArray[2].Rhw = 1f;
            texturedArray[3].Rhw = 1f;
            return texturedArray;
        }
    }
}

