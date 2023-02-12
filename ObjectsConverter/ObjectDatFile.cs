using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ObjectsConverter
{
    public class ObjectDatFile
    {
        public const string HEADER = "BULLFROG OBJECT DATA";
        public const int HEADER_LENGTH = 58;

        public struct Vertex
        {
            public float X;
            public float Y;
            public float Z;
        }
        public struct Triangle
        {
            public int A;
            public int B;
            public int C;
        }

        public static ObjectDatFile ReadFile(string filename)
        {
            ObjectDatFile file = new ObjectDatFile(filename);
            try
            {
                file = loadFromFile(filename, file);
            }
            catch(Exception e)
            {
                file.Error = e.Message;
            }

            return file;
        }

        private static ObjectDatFile loadFromFile(string filename, ObjectDatFile file)
        { 
            if (!File.Exists(filename))
            {
                file.Error = "File not found!";
                return file;
            }

            bool isAnimation = false;
            byte[] bytes = File.ReadAllBytes(filename);

            #region validate header
            byte[] header = Encoding.ASCII.GetBytes(HEADER);
            int offset = 0;
            for (offset = 0; offset < header.Length; offset++)
            {
                if (bytes[offset] != header[offset])
                {
                    file.Error = "Header not found!";
                    return file;
                }
            }
            #endregion

            #region 4 zerobytes follow
            file.Error = "Zero 1 after header not found!";
            if (bytes[offset] != 0) return file;
            offset++;

            file.Error = "Zero 2 after header not found!";
            if (bytes[offset] != 0) return file;
            offset++;

            file.Error = "Zero 3 after header not found!";
            if (bytes[offset] != 0) return file;
            offset++;

            file.Error = "Zero 4 after header not found!";
            if (bytes[offset] != 0) return file;
            offset++;
            #endregion

            #region numBytesTriDef
            file.Error = "Invalid numBytesTriDef found!";
            int numBytesTriDef = bytes[offset] | (bytes[offset+1] << 8);
            if (numBytesTriDef == 0) return file;
            offset++;
            offset++;

            // 2 zerobytes (or 2 more bytes of numBytesTriDef maybe)
            file.Error = "Zero 5 after header not found!";
            if (bytes[offset] != 0) return file;
            offset++;

            file.Error = "Zero 6 after header not found!";
            if (bytes[offset] != 0) return file;
            offset++;
            #endregion

            #region numBytesVertexData
            file.Error = "Invalid numBytesVertexData found!";
            int numBytesVertexData = bytes[offset] | (bytes[offset + 1] << 8);
            if (numBytesVertexData == 0) return file;
            offset++;
            offset++;

            // 2 zerobytes (or 2 more bytes of numBytesVertexData maybe)
            file.Error = "Zero 7 after header not found!";
            if (bytes[offset] != 0) return file;
            offset++;

            file.Error = "Zero 8 after header not found!";
            if (bytes[offset] != 0) return file;
            offset++;
            #endregion

            #region num 3, same as numVertices
            file.Error = "Invalid num3 found!";
            int num3 = bytes[offset] | (bytes[offset + 1] << 8);
            if (num3 == 0) return file;
            offset++;
            offset++;

            // 2 zerobytes
            file.Error = "Zero 9 after header not found!";
            if (bytes[offset] != 0) return file;
            offset++;

            file.Error = "Zero 10 after header not found!";
            if (bytes[offset] != 0) return file;
            offset++;
            #endregion

            #region 2 constants (05 and 01, same in all object files)
            file.Error = "Constant 1 is " + bytes[offset] + ", not 5 or 7!";
            if (bytes[offset] != 5 && bytes[offset] != 7) return file;
            if (bytes[offset] == 7) isAnimation = true;
            offset++;

            file.Error = "Constant 2 is " + bytes[offset] + ", not 1, 4 or 20!";
            if (bytes[offset] != 1 && bytes[offset] != 4 && bytes[offset] != 20) return file;
            offset++;
            #endregion

            #region numTriangles
            file.Error = "Invalid numTriangles found!";
            int numTriangles = bytes[offset] | (bytes[offset + 1] << 8);
            if (numTriangles == 0) return file;
            offset++;
            offset++;
            #endregion

            #region numVertices
            file.Error = "Invalid numVertices found!";
            int numVertices = bytes[offset] | (bytes[offset + 1] << 8);
            if (numVertices == 0) return file;
            offset++;
            offset++;
            #endregion

            #region 2 constants (4F and 00, same in all object files)
            file.Error = "Constant 3 did not match!";
            if (bytes[offset] != 0x4f) return file;
            offset++;

            file.Error = "Constant 4 did not match!";
            if (bytes[offset] != 0) return file;
            offset++;
            #endregion

            #region read triangle definitions
            offset = HEADER_LENGTH;

            for(int i = 0; i < numTriangles; i++)
            {
                byte type = bytes[offset];

                // Animations have some zeros before actual triangle data.
                // I couldn't find a regularity in the number of bytes used.
                while (isAnimation && type == 0) 
                {
                    offset++;
                    type = bytes[offset];
                }

                if (type != 5 && type != 4)
                {
                    file.Error = "Triangle #" + (i+1) + "has unknown type " + type;
                    return file;
                }

                offset++;
                offset++; // skip zero

                // 3 indices of the triangle
                var triangle = new Triangle
                {
                    A = bytes[offset + 0],
                    B = bytes[offset + 1],
                    C = bytes[offset + 2]
                };

                offset += 3;

                if (type == 5) offset += 7; // presumably 1 byte texture atlas id, 6 bytes tex coords
                else offset ++;

                file.Triangles.Add(triangle);
            }
            #endregion

            #region read vertices
            for(int i = 0; i < numVertices; i++)
            {
                var vertex = new Vertex();
                vertex.X = BitConverter.ToInt16(new byte[] { bytes[offset], bytes[offset + 1] }, 0) / 1024f;
                offset += 2;
                vertex.Y = BitConverter.ToInt16(new byte[] { bytes[offset], bytes[offset + 1] }, 0) / 1024f;
                offset += 2;
                vertex.Z = BitConverter.ToInt16(new byte[] { bytes[offset], bytes[offset + 1] }, 0) / 1024f;
                offset += 2;
                file.Vertices.Add(vertex);
            }
            #endregion

            // we don't know what the rest does so we skip it
            file.Error = "";

            return file;
        }

        public string Filename { get; private set; }
        public bool Ready { get; private set; }
        public string Error = "";


        public List<Triangle> Triangles { get; protected set; } = new List<Triangle>();
        public List<Vertex> Vertices { get; protected set; } = new List<Vertex>();

        public ObjectDatFile(string filename)
        {
            Filename = filename;
            Ready = false;
        }

        public string ToWavefrontOBJ()
        {
            string code = "o " + Path.GetFileNameWithoutExtension(Filename) + Environment.NewLine;
            
            foreach(var vertex in Vertices)
            {
                code += $"v {vertex.X.ToString("0.000000")} {vertex.Y.ToString("0.000000")} {vertex.Z.ToString("0.000000")}" + Environment.NewLine;
            }

            foreach(var triangle in Triangles)
            {
                // OBJ is using 1-based indices
                code += $"f {triangle.A + 1} {triangle.B + 1} {triangle.C + 1}" + Environment.NewLine;
            }

            return code;
        }
    }
}
