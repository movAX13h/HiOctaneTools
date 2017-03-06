using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using OpenTK;
using LevelEditor.Engine.Models;
using LevelEditor.Utils;
using OpenTK.Graphics.OpenGL;

namespace LevelEditor.Engine.Resources.Loaders
{
    /*
        TODO (if required):
            - support groups
            - extract mtl filename
            - parse mtl file
    */
    public class WavefrontLoader
    {
        static List<Vector3> vertices;
        static List<Vector3> uniqueNormals;
        static List<Vector3> normals;
        static List<Vector2> uniqueUVs;
        static List<Vector2> uvs;
        static List<int> indices;

        static char[] splitCharacters = new char[] { ' ' };
        static char[] faceParameterSplitter = new char[] { '/' };

        public static bool Load(WavefrontModel model, string fileName)
        {
            try
            {
                using (StreamReader streamReader = new StreamReader(Config.DATA_FOLDER + fileName))
                {
                    bool success = Load(model, streamReader);
                    streamReader.Close();

                    // cleanup
                    vertices = null;
                    uniqueNormals = null;
                    normals = null;
                    uniqueUVs = null;
                    uvs = null;
                    indices = null;

                    return success;
                }
            }
            catch(Exception e)
            {
                Log.WriteLine(Log.LOG_ERROR, "WavefrontLoader: " + e.Message);
                return false;
            }
        }

        static bool Load(WavefrontModel model, TextReader textReader)
        {
            // if help needed: https://github.com/53V3N1X/SevenEngine/blob/1e20e0ea38854a44966557e3bfc0bca605214aaa/SevenEngine/MANAGERS/StaticModelManager.cs

            bool usingNormals = false;

            vertices = new List<Vector3>();
            normals = new List<Vector3>();
            uniqueNormals = new List<Vector3>();
            uvs = new List<Vector2>();
            uniqueUVs = new List<Vector2>();
            indices = new List<int>();

            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-us");
            string line = "";

            PrimitiveType faceType = PrimitiveType.Triangles;

            while ((line = textReader.ReadLine()) != null)
            {
                line = line.Trim(splitCharacters);
                line = line.Replace("  ", " ");

                string[] parameters = line.Split(splitCharacters);

                switch (parameters[0])
                {
                    case "p": // Point
                        break;

                    case "v": // vertex
                        float x = float.Parse(parameters[1], culture);
                        float y = float.Parse(parameters[2], culture);
                        float z = float.Parse(parameters[3], culture);
                        vertices.Add(new Vector3(x, y, z));
                        uvs.Add(Vector2.Zero);
                        normals.Add(Vector3.Zero);
                        break;

                    case "vn": // vertex normal
                        usingNormals = true;
                        float nx = float.Parse(parameters[1], culture);
                        float ny = float.Parse(parameters[2], culture);
                        float nz = float.Parse(parameters[3], culture);
                        uniqueNormals.Add(new Vector3(nx, ny, nz));
                        break;

                    case "vt": // vertex texCoord (UV)
                        float u = float.Parse(parameters[1], culture);
                        float v = float.Parse(parameters[2], culture);
                        uniqueUVs.Add(new Vector2(u, v));
                        break;

                    case "f": // face
                        int id;
                        faceType = parameters.Length == 4 ? PrimitiveType.Triangles : PrimitiveType.Quads;

                        switch (faceType)
                        {
                            case PrimitiveType.Triangles: // 3 vertices per face
                                id = indexFromFaceParameter(parameters[1], 0);
                                uvs[id] = uniqueUVs[indexFromFaceParameter(parameters[1], 1)];
                                if (usingNormals) normals[id] = uniqueNormals[indexFromFaceParameter(parameters[1], 2)];
                                indices.Add(id);

                                id = indexFromFaceParameter(parameters[2], 0);
                                uvs[id] = uniqueUVs[indexFromFaceParameter(parameters[2], 1)];
                                if (usingNormals) normals[id] = uniqueNormals[indexFromFaceParameter(parameters[2], 2)];
                                indices.Add(id);

                                id = indexFromFaceParameter(parameters[3], 0);
                                uvs[id] = uniqueUVs[indexFromFaceParameter(parameters[3], 1)];
                                if (usingNormals) normals[id] = uniqueNormals[indexFromFaceParameter(parameters[3], 2)];
                                indices.Add(id);

                                break;

                            case PrimitiveType.Quads: // 4 vertices per face
                                id = indexFromFaceParameter(parameters[1], 0);
                                uvs[id] = uniqueUVs[indexFromFaceParameter(parameters[1], 1)];
                                if (usingNormals) normals[id] = uniqueNormals[indexFromFaceParameter(parameters[1], 2)];
                                indices.Add(id);

                                id = indexFromFaceParameter(parameters[2], 0);
                                uvs[id] = uniqueUVs[indexFromFaceParameter(parameters[2], 1)];
                                if (usingNormals) normals[id] = uniqueNormals[indexFromFaceParameter(parameters[2], 2)];
                                indices.Add(id);

                                id = indexFromFaceParameter(parameters[3], 0);
                                uvs[id] = uniqueUVs[indexFromFaceParameter(parameters[3], 1)];
                                if (usingNormals) normals[id] = uniqueNormals[indexFromFaceParameter(parameters[3], 2)];
                                indices.Add(id);

                                id = indexFromFaceParameter(parameters[4], 0);
                                uvs[id] = uniqueUVs[indexFromFaceParameter(parameters[4], 1)];
                                if (usingNormals) normals[id] = uniqueNormals[indexFromFaceParameter(parameters[4], 2)];
                                indices.Add(id);

                                break;

                            default:
                                Log.WriteLine(Log.LOG_ERROR, "WavefrontLoader: invalid number of vertices per face (" + (parameters.Length - 1).ToString() + ")");
                                return false; // different formats are not supported
                        }
                        break;
                }
            }

            model.PolygonType = faceType;
            model.Vertices = vertices.ToArray();
            model.Indices = indices.ToArray();
            model.TexCoords = uvs.ToArray();
            if (!usingNormals) normals = new List<Vector3>();
            model.Normals = normals.ToArray();

            return true;
        }

        static int indexFromFaceParameter(string faceParameter, int part)
        {
            string[] parameters = faceParameter.Split(faceParameterSplitter);
            int id = -1;
            if (part < parameters.Length)
            {
                if (Int32.TryParse(parameters[part], out id)) return id - 1;
            }

            return -1;
        }

    }
}
