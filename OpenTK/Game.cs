using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;


namespace Open_TK
{

	public class Shader
	{
		public int shaderHandle;

		public void LoadShader()
		{
			shaderHandle = GL.CreateProgram();

			int vertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexShader, LoadShaderSource("shader.vert"));
			GL.CompileShader(vertexShader);

			GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int success1);
			if (success1 == 0)
			{
				string infoLog = GL.GetShaderInfoLog(vertexShader);
				Console.WriteLine(infoLog);
			}

			int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShader, LoadShaderSource("shader.frag"));
			GL.CompileShader(fragmentShader);

			GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int success2);
			if (success2 == 0)
			{
				string infoLog = GL.GetShaderInfoLog(fragmentShader);
				Console.WriteLine(infoLog);
			}

			GL.AttachShader(shaderHandle, vertexShader);
			GL.AttachShader(shaderHandle, fragmentShader);

			GL.LinkProgram(shaderHandle);

			GL.DeleteShader(vertexShader);
			GL.DeleteShader(fragmentShader);
		}

		public void UseShader()
		{
			GL.UseProgram(shaderHandle);
		}
		public void DeleteShader()
		{
			GL.DeleteProgram(shaderHandle);
		}

		public static string LoadShaderSource(string filepath)
		{
			string shaderSource = "";
			try
			{
				using (StreamReader reader = new StreamReader("../../../Shaders/" + filepath))
				{
					shaderSource = reader.ReadToEnd();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Failed to load shader source file: " + e.Message);
			}
			return shaderSource;
		}
		

	}



	internal class Game : GameWindow
	{
		int width, height;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> wall_vertices = new List<Vector3>()
		{	
			//front face
			new Vector3(-5f,  5f, 5f), //top-left vertice
			new Vector3( 5f,  5f, 5f), //top-right vertice
			new Vector3( 5f, -5f, 5f), //bottom-right vertice
			new Vector3(-5f, -5f, 5f), //botom-left vertice
			//right face
			new Vector3( 5f,  5f, 5f), //top-left vertice
			new Vector3( 5f,  5f, -5f), //top-right vertice
			new Vector3( 5f, -5f, -5f), //bottom-right vertice
			new Vector3( 5f, -5f, 5f), //botom-left vertice
			//back face
			new Vector3( 5f,  5f, 5f), //top-left vertice
			new Vector3( 5f,  5f, -5f), //top-right vertice
			new Vector3( 5f, -5f, -5f), //bottom-right vertice
			new Vector3( 5f, -5f, 5f), //botom-left vertice
			//left face
			new Vector3( -5f,  5f, 5f), //top-left vertice
			new Vector3( -5f,  5f, -5f), //top-right vertice
			new Vector3( -5f, -5f, -5f), //bottom-right vertice
			new Vector3( -5f, -5f, 5f), //botom-left vertice
			// top face
			new Vector3( 5f,  5f, 5f), //top-left vertice
			new Vector3( 5f,  5f, -5f), //top-right vertice
			new Vector3( 5f, -5f, -5f), //bottom-right vertice
			new Vector3( 5f, -5f, 5f), //botom-left vertice
			//bottom face
			new Vector3( 5f,  5f, 5f), //top-left vertice
			new Vector3( 5f,  5f, -5f), //top-right vertice
			new Vector3( 5f, -5f, -5f), //bottom-right vertice
			new Vector3( 5f, -5f, 5f), //botom-left vertice
		};

		List<Vector2> texCoords_wall = new List<Vector2>()
		{ 
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),

			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),

			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),

			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),

			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),

			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),
		};

		uint[] wall_indices =
		{
			0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
			6, 7, 4,

			8, 9, 10,
			10, 11, 8,

			12, 13, 14,
			14, 15, 12,

			16, 17, 18,
			18, 19, 16,

			20, 21, 22,
			22, 23, 20
		};


		int wall_VAO;
		int wall_VBO;
		int wall_EBO;
		
		int textureID_wall;
		int textureVBO_wall;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> wall1_vertices = new List<Vector3>()
        {	
			//front face
		new Vector3(-5f,  5f, 5f), //top-left vertice
		new Vector3( 5f,  5f, 5f), //top-right vertice
		new Vector3( 5f, -5f, 5f), //bottom-right vertice
		new Vector3(-5f, -5f, 5f), //botom-left vertice
		//right face
		new Vector3( 5f,  5f, 5f), //top-left vertice
		new Vector3( 5f,  5f, 1f), //top-right vertice
		new Vector3( 5f, -5f, 1f), //bottom-right vertice
		new Vector3( 5f, -5f, 5f), //botom-left vertice
		//back face
		new Vector3(-5f,  5f, 1f), //top-left vertice
		new Vector3( 5f,  5f, 1f), //top-right vertice
		new Vector3( 5f, -5f, 1f), //bottom-right vertice
		new Vector3(-5f, -5f, 1f), //botom-left vertice
		//left face
		new Vector3( -5f,  5f, 5f), //top-left vertice
		new Vector3( -5f,  5f, 1f), //top-right vertice
		new Vector3( -5f, -5f, 1f), //bottom-right vertice
		new Vector3( -5f, -5f, 5f), //botom-left vertice
		// top face
		new Vector3(-5f,  5f, 5f), //top-left vertice
		new Vector3( 5f,  5f, 5f), //top-right vertice
		new Vector3( 5f, -5f, 5f), //bottom-right vertice
		new Vector3(-5f, -5f, 5f), //botom-left vertice
		//bottom face
		new Vector3(-5f,  5f, 5f), //top-left vertice
		new Vector3( 5f,  5f, 5f), //top-right vertice
		new Vector3( 5f, -5f, 5f), //bottom-right vertice
		new Vector3(-5f, -5f, 5f), //botom-left vertice
		};

        List<Vector2> texCoords_wall1 = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] wall1_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int wall1_VAO;
        int wall1_VBO;
        int wall1_EBO;

        int textureID_wall1;
        int textureVBO_wall1;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> monument_vertices = new List<Vector3>()
        {	
			//front face
		new Vector3(-1f,  0.5f, 1f), //top-left vertice
		new Vector3( 1f,  0.5f, 1f), //top-right vertice
		new Vector3( 1f, -0.5f, 1f), //bottom-right vertice
		new Vector3(-1f, -0.5f, 1f), //botom-left vertice
		//right face
		new Vector3( 1f,  0.5f, 1f), //top-left vertice
		new Vector3( 1f,  0.5f, 2f), //top-right vertice
		new Vector3( 1f, -0.5f, 2f), //bottom-right vertice
		new Vector3( 1f, -0.5f, 1f), //botom-left vertice
		//back face
		new Vector3(-1f,  0.5f, 2f), //top-left vertice
		new Vector3( 1f,  0.5f, 2f), //top-right vertice
		new Vector3( 1f, -0.5f, 2f), //bottom-right vertice
		new Vector3(-1f, -0.5f, 2f), //botom-left vertice
		//left face
		new Vector3( -1f,  0.5f, 1f), //top-left vertice
		new Vector3( -1f,  0.5f, 2f), //top-right vertice
		new Vector3( -1f, -0.5f, 2f), //bottom-right vertice
		new Vector3( -1f, -0.5f, 1f), //botom-left vertice
		// top face
		new Vector3(-1f,  0.5f, 2f), //top-left vertice
		new Vector3( 1f,  0.5f, 2f), //top-right vertice
		new Vector3( 1f, 0.5f, 1f), //bottom-right vertice
		new Vector3(-1f, 0.5f, 1f), //botom-left vertice
		//bottom face
		new Vector3(-1f,  -0.5f, 2f), //top-left vertice
		new Vector3( 1f,  -0.5f, 2f), //top-right vertice
		new Vector3( 1f, -0.5f, 1f), //bottom-right vertice
		new Vector3(-1f, -0.5f, 1f), //botom-left vertice
		};

        List<Vector2> texCoords_monument = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] monument_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int monument_VAO;
        int monument_VBO;
        int monument_EBO;

        int textureID_monument;
        int textureVBO_monument;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> monument1_vertices = new List<Vector3>()
        {	
			//front face
		new Vector3( 1f,  0.5f, 1f), //top-left vertice
		new Vector3( 1f,  0.5f, 2f), //top-right vertice
		new Vector3( 1f, -0.5f, 2f), //bottom-right vertice
		new Vector3( 1f, -0.5f, 1f), //botom-left vertice
		//right face
		new Vector3( 1f,  0.5f, 1f), //top-left vertice
		new Vector3( 1f,  0.5f, 2f), //top-right vertice
		new Vector3( 1f, -0.5f, 2f), //bottom-right vertice
		new Vector3( 1f, -0.5f, 1f), //botom-left vertice
		//back face
		new Vector3(-1f,  0.5f, 2f), //top-left vertice
		new Vector3( 1f,  0.5f, 2f), //top-right vertice
		new Vector3( 1f, -0.5f, 2f), //bottom-right vertice
		new Vector3(-1f, -0.5f, 2f), //botom-left vertice
		//left face
		new Vector3( -1f,  0.5f, 1f), //top-left vertice
		new Vector3( -1f,  0.5f, 2f), //top-right vertice
		new Vector3( -1f, -0.5f, 2f), //bottom-right vertice
		new Vector3( -1f, -0.5f, 1f), //botom-left vertice
		// top face
		new Vector3(-1f,  0.5f, 2f), //top-left vertice
		new Vector3( 1f,  0.5f, 2f), //top-right vertice
		new Vector3( 1f, 0.5f, 1f), //bottom-right vertice
		new Vector3(-1f, 0.5f, 1f), //botom-left vertice
		//bottom face
		new Vector3(-1f,  -0.5f, 2f), //top-left vertice
		new Vector3( 1f,  -0.5f, 2f), //top-right vertice
		new Vector3( 1f, -0.5f, 1f), //bottom-right vertice
		new Vector3(-1f, -0.5f, 1f), //botom-left vertice
		};

        List<Vector2> texCoords_monument1 = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] monument1_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int monument1_VAO;
        int monument1_VBO;
        int monument1_EBO;

        int textureID_monument1;
        int textureVBO_monument1;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> rakushka_vertices = new List<Vector3>()
        {	
			//front face
		new Vector3(-1f,  0.25f, 1f), //top-left vertice
		new Vector3( 1f,  0.25f, 1f), //top-right vertice
		new Vector3( 1f, -0.5f, 1f), //bottom-right vertice
		new Vector3(-1f, -0.5f, 1f), //botom-left vertice
		//right face
		new Vector3( 1f,  0.25f, 1f), //top-left vertice
		new Vector3( 1f,  0.25f, 2f), //top-right vertice
		new Vector3( 1f, -0.5f, 2f), //bottom-right vertice
		new Vector3( 1f, -0.5f, 1f), //botom-left vertice
		//back face
		new Vector3(-1f,  0.25f, 2f), //top-left vertice
		new Vector3( 1f,  0.25f, 2f), //top-right vertice
		new Vector3( 1f, -0.5f, 2f), //bottom-right vertice
		new Vector3(-1f, -0.5f, 2f), //botom-left vertice
		//left face
		new Vector3( -1f,  0.25f, 1f), //top-left vertice
		new Vector3( -1f,  0.25f, 2f), //top-right vertice
		new Vector3( -1f, -0.5f, 2f), //bottom-right vertice
		new Vector3( -1f, -0.5f, 1f), //botom-left vertice
		// top face
		new Vector3(-1f,  0.25f, 2f), //top-left vertice
		new Vector3( 1f,  0.25f, 2f), //top-right vertice
		new Vector3( 1f, 0.25f, 1f), //bottom-right vertice
		new Vector3(-1f, 0.25f, 1f), //botom-left vertice
		//bottom face
		new Vector3(-1f,  -0.5f, 2f), //top-left vertice
		new Vector3( 1f,  -0.5f, 2f), //top-right vertice
		new Vector3( 1f, -0.5f, 1f), //bottom-right vertice
		new Vector3(-1f, -0.5f, 1f), //botom-left vertice
		};

        List<Vector2> texCoords_rakushka = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] rakushka_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int rakushka_VAO;
        int rakushka_VBO;
        int rakushka_EBO;

        int textureID_rakushka;
        int textureVBO_rakushka;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> floor_vertices = new List<Vector3>()
        {	
			//front face
			new Vector3(-5f,  -5f, -5f), //top-left vertice
			new Vector3( 5f,  -5f, -5f), //top-right vertice
			new Vector3( 5f, -5f, 5f), //bottom-right vertice
			new Vector3(-5f, -5f, 5f), //botom-left vertice
			//right face
			new Vector3(-5f,  -5f, -5f), //top-left vertice
			new Vector3( 5f,  -5f, -5f), //top-right vertice
			new Vector3( 5f, -5f, 5f), //bottom-right vertice
			new Vector3(-5f, -5f, 5f), //botom-left vertice
			//back face
			new Vector3(-5f,  -5f, -5f), //top-left vertice
			new Vector3( 5f,  -5f, -5f), //top-right vertice
			new Vector3( 5f, -5f, 5f), //bottom-right vertice
			new Vector3(-5f, -5f, 5f), //botom-left vertice
			//left face
			new Vector3(-5f,  -5f, -5f), //top-left vertice
			new Vector3( 5f,  -5f, -5f), //top-right vertice
			new Vector3( 5f, -5f, 5f), //bottom-right vertice
			new Vector3(-5f, -5f, 5f), //botom-left vertice
			// top face
			new Vector3(-5f,  -5f, -5f), //top-left vertice
			new Vector3( 5f,  -5f, -5f), //top-right vertice
			new Vector3( 5f, -5f, 5f), //bottom-right vertice
			new Vector3(-5f, -5f, 5f), //botom-left vertice
			//bottom face
			new Vector3(-5f,  -5f, -5f), //top-left vertice
			new Vector3( 5f,  -5f, -5f), //top-right vertice
			new Vector3( 5f, -5f, 5f), //bottom-right vertice
			new Vector3(-5f, -5f, 5f), //botom-left vertice
		};

        List<Vector2> texCoords_floor = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] floor_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int floor_VAO;
        int floor_VBO;
        int floor_EBO;

        int textureID_floor;
        int textureVBO_floor;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> ceiling_vertices = new List<Vector3>()
        {	
			//front face
			new Vector3(-5f,  5f, -5f), //top-left vertice
			new Vector3( 5f,  5f, -5f), //top-right vertice
			new Vector3( 5f, 5f, 5f), //bottom-right vertice
			new Vector3(-5f, 5f, 5f), //botom-left vertice
			//right face
			new Vector3(-5f,  5f, -5f), //top-left vertice
			new Vector3( 5f,  5f, -5f), //top-right vertice
			new Vector3( 5f, 5f, 5f), //bottom-right vertice
			new Vector3(-5f, 5f, 5f), //botom-left vertice
			//back face
			new Vector3(-5f,  5f, -5f), //top-left vertice
			new Vector3( 5f,  5f, -5f), //top-right vertice
			new Vector3( 5f, 5f, 5f), //bottom-right vertice
			new Vector3(-5f, 5f, 5f), //botom-left vertice
			//left face
			new Vector3(-5f,  5f, -5f), //top-left vertice
			new Vector3( 5f,  5f, -5f), //top-right vertice
			new Vector3( 5f, 5f, 5f), //bottom-right vertice
			new Vector3(-5f, 5f, 5f), //botom-left vertice
			// top face
			new Vector3(-5f,  5f, -5f), //top-left vertice
			new Vector3( 5f,  5f, -5f), //top-right vertice
			new Vector3( 5f, 5f, 5f), //bottom-right vertice
			new Vector3(-5f, 5f, 5f), //botom-left vertice
			//bottom face
			new Vector3(-5f,  5f, -5f), //top-left vertice
			new Vector3( 5f,  5f, -5f), //top-right vertice
			new Vector3( 5f, 5f, 5f), //bottom-right vertice
			new Vector3(-5f, 5f, 5f), //botom-left vertice
		};

        List<Vector2> texCoords_ceiling = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] ceiling_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int ceiling_VAO;
        int ceiling_VBO;
        int ceiling_EBO;

        int textureID_ceiling;
        int textureVBO_ceiling;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> globe_Vertices;
        List<Vector2> globe_TexCoords;
        uint[] globe_Indices;
        int globe_VAO;
        int globe_VBO;
        int globe_EBO;
        int textureID_globe;
        int textureVBO_globe;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> ruka_vertices = new List<Vector3>()
        {	
			//front face
		new Vector3(-0.75f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.75f,  1f, -0.75f), //top-right vertice
		new Vector3( -0.75f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, 0.75f), //botom-left vertice
			//right face
		new Vector3(-0.75f,  1f, -0.75f), //top-left vertice
		new Vector3( -0.8f,  1f, -0.75f), //top-right vertice
		new Vector3( -0.8f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, -0.75f), //botom-left vertice
			//back face
		new Vector3(-0.75f,  1f, -0.75f), //top-left vertice
		new Vector3( -0.8f,  1f, -0.75f), //top-right vertice
		new Vector3( -0.8f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, -0.75f), //botom-left vertice
			//left face
		new Vector3(-0.8f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.75f,  1f, 0.75f), //top-right vertice
		new Vector3( -0.75f, -1f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -1f, 0.75f), //botom-left vertice
			// top face
		new Vector3(-0.75f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.8f, 1f, 0.75f), //top-right vertice
		new Vector3( -0.8f, 1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, 1f, -0.75f), //botom-left vertice
			//bottom face
		new Vector3(-0.75f, -1f, 0.75f), //top-left vertice
		new Vector3( -0.8f, -1f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, -0.75f), //botom-left vertice
		};

        List<Vector2> texCoords_ruka = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] ruka_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int ruka_VAO;
        int ruka_VBO;
        int ruka_EBO;

        int textureID_ruka;
        int textureVBO_ruka;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> wall2_vertices = new List<Vector3>()
        {	
			//front face
			new Vector3( 5f,  5f, 5f), //top-left vertice
			new Vector3( 5f,  5f, -5f), //top-right vertice
			new Vector3( 5f, -5f, -5f), //bottom-right vertice
			new Vector3( 5f, -5f, 5f), //botom-left vertice
			//right face
			new Vector3( 5f,  5f, 5f), //top-left vertice
			new Vector3( 5f,  5f, -5f), //top-right vertice
			new Vector3( 5f, -5f, -5f), //bottom-right vertice
			new Vector3( 5f, -5f, 5f), //botom-left vertice
			//back face
			new Vector3( -5f,  5f, -5f), //top-left vertice
			new Vector3( 5f,  5f, -5f), //top-right vertice
			new Vector3( 5f, -5f, -5f), //bottom-right vertice
			new Vector3( -5f, -5f, -5f), //botom-left vertice
			//left face
			new Vector3( -5f,  5f, 5f), //top-left vertice
			new Vector3( -5f,  5f, -5f), //top-right vertice
			new Vector3( -5f, -5f, -5f), //bottom-right vertice
			new Vector3( -5f, -5f, 5f), //botom-left vertice
			// top face
			new Vector3( 5f,  5f, 5f), //top-left vertice
			new Vector3( 5f,  5f, -5f), //top-right vertice
			new Vector3( 5f, -5f, -5f), //bottom-right vertice
			new Vector3( 5f, -5f, 5f), //botom-left vertice
			//bottom face
			new Vector3( 5f,  5f, 5f), //top-left vertice
			new Vector3( 5f,  5f, -5f), //top-right vertice
			new Vector3( 5f, -5f, -5f), //bottom-right vertice
			new Vector3( 5f, -5f, 5f), //botom-left vertice
		};

        List<Vector2> texCoords_wall2 = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] wall2_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int wall2_VAO;
        int wall2_VBO;
        int wall2_EBO;

        int textureID_wall2;
        int textureVBO_wall2;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> floor2_vertices = new List<Vector3>()
        {	
			//front face
			new Vector3(-5f,  -5f, -5f), //top-left vertice
			new Vector3( 5f,  -5f, -5f), //top-right vertice
			new Vector3( 5f, -5f, 5f), //bottom-right vertice
			new Vector3(-5f, -5f, 5f), //botom-left vertice
			//right face
			new Vector3(-5f,  -5f, -5f), //top-left vertice
			new Vector3( 5f,  -5f, -5f), //top-right vertice
			new Vector3( 5f, -5f, 5f), //bottom-right vertice
			new Vector3(-5f, -5f, 5f), //botom-left vertice
			//back face
			new Vector3(-5f,  -5f, -5f), //top-left vertice
			new Vector3( 5f,  -5f, -5f), //top-right vertice
			new Vector3( 5f, -5f, 5f), //bottom-right vertice
			new Vector3(-5f, -5f, 5f), //botom-left vertice
			//left face
			new Vector3(-5f,  -5f, -5f), //top-left vertice
			new Vector3( 5f,  -5f, -5f), //top-right vertice
			new Vector3( 5f, -5f, 5f), //bottom-right vertice
			new Vector3(-5f, -5f, 5f), //botom-left vertice
			// top face
			new Vector3(-5f,  -5f, -5f), //top-left vertice
			new Vector3( 5f,  -5f, -5f), //top-right vertice
			new Vector3( 5f, -5f, 5f), //bottom-right vertice
			new Vector3(-5f, -5f, 5f), //botom-left vertice
			//bottom face
			new Vector3(-5f,  -5f, -5f), //top-left vertice
			new Vector3( 5f,  -5f, -5f), //top-right vertice
			new Vector3( 5f, -5f, 5f), //bottom-right vertice
			new Vector3(-5f, -5f, 5f), //botom-left vertice
		};

        List<Vector2> texCoords_floor2 = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] floor2_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int floor2_VAO;
        int floor2_VBO;
        int floor2_EBO;

        int textureID_floor2;
        int textureVBO_floor2;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> ceiling2_vertices = new List<Vector3>()
        {	
			//front face
			new Vector3(-5f,  5f, -5f), //top-left vertice
			new Vector3( 5f,  5f, -5f), //top-right vertice
			new Vector3( 5f, 5f, 5f), //bottom-right vertice
			new Vector3(-5f, 5f, 5f), //botom-left vertice
			//right face
			new Vector3(-5f,  5f, -5f), //top-left vertice
			new Vector3( 5f,  5f, -5f), //top-right vertice
			new Vector3( 5f, 5f, 5f), //bottom-right vertice
			new Vector3(-5f, 5f, 5f), //botom-left vertice
			//back face
			new Vector3(-5f,  5f, -5f), //top-left vertice
			new Vector3( 5f,  5f, -5f), //top-right vertice
			new Vector3( 5f, 5f, 5f), //bottom-right vertice
			new Vector3(-5f, 5f, 5f), //botom-left vertice
			//left face
			new Vector3(-5f,  5f, -5f), //top-left vertice
			new Vector3( 5f,  5f, -5f), //top-right vertice
			new Vector3( 5f, 5f, 5f), //bottom-right vertice
			new Vector3(-5f, 5f, 5f), //botom-left vertice
			// top face
			new Vector3(-5f,  5f, -5f), //top-left vertice
			new Vector3( 5f,  5f, -5f), //top-right vertice
			new Vector3( 5f, 5f, 5f), //bottom-right vertice
			new Vector3(-5f, 5f, 5f), //botom-left vertice
			//bottom face
			new Vector3(-5f,  5f, -5f), //top-left vertice
			new Vector3( 5f,  5f, -5f), //top-right vertice
			new Vector3( 5f, 5f, 5f), //bottom-right vertice
			new Vector3(-5f, 5f, 5f), //botom-left vertice
		};

        List<Vector2> texCoords_ceiling2 = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] ceiling2_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int ceiling2_VAO;
        int ceiling2_VBO;
        int ceiling2_EBO;

        int textureID_ceiling2;
        int textureVBO_ceiling2;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> laser_vertices = new List<Vector3>()
        {	
			//front face
		new Vector3(-0.8f,  5f, 0.8f), //top-left vertice
		new Vector3( -0.8f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -5f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -5f, 0.8f), //botom-left vertice
			//right face
        new Vector3(-0.8f,  5f, 0.8f), //top-left vertice
		new Vector3( -0.75f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.75f, -5f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -5f, 0.8f), //botom-left vertice
			//back face
		new Vector3(-0.8f,  5f, 0.8f), //top-left vertice
		new Vector3( -0.8f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -5f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -5f, 0.8f), //botom-left vertice
			//left face
        new Vector3( -0.75f, 5f, 0.75f), //top-left vertice
		new Vector3( -0.8f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -5f, 0.75f), //bottom-right vertice
		new Vector3( -0.75f, -5f, 0.75f), //botom-left vertice
			// top face
        new Vector3(-0.8f,  5f, 0.8f), //top-left vertice
		new Vector3( -0.8f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -5f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -5f, 0.8f), //botom-left vertice
			//bottom face
        new Vector3(-0.8f,  5f, 0.8f), //top-left vertice
		new Vector3( -0.8f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -5f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -5f, 0.8f), //botom-left vertice
		};

        List<Vector2> texCoords_laser = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] laser_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int laser_VAO;
        int laser_VBO;
        int laser_EBO;

        int textureID_laser;
        int textureVBO_laser;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> planeta_Vertices;
        List<Vector2> planeta_TexCoords;
        uint[] planeta_Indices;
        int planeta_VAO;
        int planeta_VBO;
        int planeta_EBO;
        int textureID_planeta;
        int textureVBO_planeta;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> laser2_vertices = new List<Vector3>()
        {	
			//front face
		new Vector3(-0.8f,  5f, 0.8f), //top-left vertice
		new Vector3( -0.8f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -5f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -5f, 0.8f), //botom-left vertice
			//right face
        new Vector3(-0.8f,  5f, 0.8f), //top-left vertice
		new Vector3( -0.75f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.75f, -5f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -5f, 0.8f), //botom-left vertice
			//back face
		new Vector3(-0.8f,  5f, 0.8f), //top-left vertice
		new Vector3( -0.8f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -5f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -5f, 0.8f), //botom-left vertice
			//left face
        new Vector3( -0.75f, 5f, 0.75f), //top-left vertice
		new Vector3( -0.8f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -5f, 0.75f), //bottom-right vertice
		new Vector3( -0.75f, -5f, 0.75f), //botom-left vertice
			// top face
        new Vector3(-0.8f,  5f, 0.8f), //top-left vertice
		new Vector3( -0.8f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -5f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -5f, 0.8f), //botom-left vertice
			//bottom face
        new Vector3(-0.8f,  5f, 0.8f), //top-left vertice
		new Vector3( -0.8f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -5f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -5f, 0.8f), //botom-left vertice
		};

        List<Vector2> texCoords_laser2 = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] laser2_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int laser2_VAO;
        int laser2_VBO;
        int laser2_EBO;

        int textureID_laser2;
        int textureVBO_laser2;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> laser3_vertices = new List<Vector3>()
        {	
			//front face
		new Vector3(-0.8f,  5f, 0.8f), //top-left vertice
		new Vector3( -0.8f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -5f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -5f, 0.8f), //botom-left vertice
			//right face
        new Vector3(-0.8f,  5f, 0.8f), //top-left vertice
		new Vector3( -0.75f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.75f, -5f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -5f, 0.8f), //botom-left vertice
			//back face
		new Vector3(-0.8f,  5f, 0.8f), //top-left vertice
		new Vector3( -0.8f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -5f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -5f, 0.8f), //botom-left vertice
			//left face
        new Vector3( -0.75f, 5f, 0.75f), //top-left vertice
		new Vector3( -0.8f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -5f, 0.75f), //bottom-right vertice
		new Vector3( -0.75f, -5f, 0.75f), //botom-left vertice
			// top face
        new Vector3(-0.8f,  5f, 0.8f), //top-left vertice
		new Vector3( -0.8f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -5f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -5f, 0.8f), //botom-left vertice
			//bottom face
        new Vector3(-0.8f,  5f, 0.8f), //top-left vertice
		new Vector3( -0.8f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -5f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -5f, 0.8f), //botom-left vertice
		};

        List<Vector2> texCoords_laser3 = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] laser3_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int laser3_VAO;
        int laser3_VBO;
        int laser3_EBO;

        int textureID_laser3;
        int textureVBO_laser3;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> monument2_vertices = new List<Vector3>()
        {	
			//front face
		new Vector3(-0.5f,  0.5f, 1f), //top-left vertice
		new Vector3( 0.5f,  0.5f, 1f), //top-right vertice
		new Vector3( 0.5f, -0.5f, 1f), //bottom-right vertice
		new Vector3(-0.5f, -0.5f, 1f), //botom-left vertice
		//right face
		new Vector3( 0.5f,  0.5f, 1f), //top-left vertice
		new Vector3( 0.5f,  0.5f, 2f), //top-right vertice
		new Vector3( 0.5f, -0.5f, 2f), //bottom-right vertice
		new Vector3( 0.5f, -0.5f, 1f), //botom-left vertice
		//back face
		new Vector3(-0.5f,  0.5f, 2f), //top-left vertice
		new Vector3( 0.5f,  0.5f, 2f), //top-right vertice
		new Vector3( 0.5f, -0.5f, 2f), //bottom-right vertice
		new Vector3(-0.5f, -0.5f, 2f), //botom-left vertice
		//left face
		new Vector3( -0.5f,  0.5f, 1f), //top-left vertice
		new Vector3( -0.5f,  0.5f, 2f), //top-right vertice
		new Vector3( -0.5f, -0.5f, 2f), //bottom-right vertice
		new Vector3( -0.5f, -0.5f, 1f), //botom-left vertice
		// top face
		new Vector3(-0.5f,  0.5f, 2f), //top-left vertice
		new Vector3( 0.5f,  0.5f, 2f), //top-right vertice
		new Vector3( 0.5f, 0.5f, 1f), //bottom-right vertice
		new Vector3(-0.5f, 0.5f, 1f), //botom-left vertice
		//bottom face
		new Vector3(-0.5f,  -0.5f, 2f), //top-left vertice
		new Vector3( 0.5f,  -0.5f, 2f), //top-right vertice
		new Vector3( 0.5f, -0.5f, 1f), //bottom-right vertice
		new Vector3(-0.5f, -0.5f, 1f), //botom-left vertice
		};

        List<Vector2> texCoords_monument2 = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] monument2_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int monument2_VAO;
        int monument2_VBO;
        int monument2_EBO;

        int textureID_monument2;
        int textureVBO_monument2;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> laser4_vertices = new List<Vector3>()
        {	
			//front face
		new Vector3(-0.8f,  5f, 0.8f), //top-left vertice
		new Vector3( -0.8f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -5f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -5f, 0.8f), //botom-left vertice
			//right face
        new Vector3(-0.8f,  5f, 0.8f), //top-left vertice
		new Vector3( -0.75f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.75f, -5f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -5f, 0.8f), //botom-left vertice
			//back face
		new Vector3(-0.8f,  5f, 0.8f), //top-left vertice
		new Vector3( -0.8f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -5f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -5f, 0.8f), //botom-left vertice
			//left face
        new Vector3( -0.75f, 5f, 0.75f), //top-left vertice
		new Vector3( -0.8f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -5f, 0.75f), //bottom-right vertice
		new Vector3( -0.75f, -5f, 0.75f), //botom-left vertice
			// top face
        new Vector3(-0.8f,  5f, 0.8f), //top-left vertice
		new Vector3( -0.8f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -5f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -5f, 0.8f), //botom-left vertice
			//bottom face
        new Vector3(-0.8f,  5f, 0.8f), //top-left vertice
		new Vector3( -0.8f, 5f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -5f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -5f, 0.8f), //botom-left vertice
		};

        List<Vector2> texCoords_laser4 = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] laser4_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int laser4_VAO;
        int laser4_VBO;
        int laser4_EBO;

        int textureID_laser4;
        int textureVBO_laser4;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> kartina1_vertices = new List<Vector3>()
        {	
			//front face
		new Vector3(-0.75f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.75f,  1f, -0.75f), //top-right vertice
		new Vector3( -0.75f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, 0.75f), //botom-left vertice
			//right face
		new Vector3(-0.75f,  1f, -0.75f), //top-left vertice
		new Vector3( -0.8f,  1f, -0.75f), //top-right vertice
		new Vector3( -0.8f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, -0.75f), //botom-left vertice
			//back face
		new Vector3(-0.8f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.8f,  1f, -0.75f), //top-right vertice
		new Vector3( -0.8f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.8f, -1f, 0.75f), //botom-left vertice
			//left face
		new Vector3(-0.8f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.75f,  1f, 0.75f), //top-right vertice
		new Vector3( -0.75f, -1f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -1f, 0.75f), //botom-left vertice
			// top face
		new Vector3(-0.75f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.8f, 1f, 0.75f), //top-right vertice
		new Vector3( -0.8f, 1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, 1f, -0.75f), //botom-left vertice
			//bottom face
		new Vector3(-0.75f, -1f, 0.75f), //top-left vertice
		new Vector3( -0.8f, -1f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, -0.75f), //botom-left vertice
		};

        List<Vector2> texCoords_kartina1 = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] kartina1_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int kartina1_VAO;
        int kartina1_VBO;
        int kartina1_EBO;

        int textureID_kartina1;
        int textureVBO_kartina1;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> kartina2_vertices = new List<Vector3>()
        {	
			//front face
		new Vector3(-0.75f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.75f,  1f, -0.75f), //top-right vertice
		new Vector3( -0.75f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, 0.75f), //botom-left vertice
			//right face
		new Vector3(-0.75f,  1f, -0.75f), //top-left vertice
		new Vector3( -0.8f,  1f, -0.75f), //top-right vertice
		new Vector3( -0.8f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, -0.75f), //botom-left vertice
			//back face
		new Vector3(-0.8f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.8f,  1f, -0.75f), //top-right vertice
		new Vector3( -0.8f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.8f, -1f, 0.75f), //botom-left vertice
			//left face
		new Vector3(-0.8f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.75f,  1f, 0.75f), //top-right vertice
		new Vector3( -0.75f, -1f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -1f, 0.75f), //botom-left vertice
			// top face
		new Vector3(-0.75f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.8f, 1f, 0.75f), //top-right vertice
		new Vector3( -0.8f, 1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, 1f, -0.75f), //botom-left vertice
			//bottom face
		new Vector3(-0.75f, -1f, 0.75f), //top-left vertice
		new Vector3( -0.8f, -1f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, -0.75f), //botom-left vertice
		};

        List<Vector2> texCoords_kartina2 = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] kartina2_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int kartina2_VAO;
        int kartina2_VBO;
        int kartina2_EBO;

        int textureID_kartina2;
        int textureVBO_kartina2;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> kartina3_vertices = new List<Vector3>()
        {	
			//front face
		new Vector3(-0.75f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.75f,  1f, -0.75f), //top-right vertice
		new Vector3( -0.75f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, 0.75f), //botom-left vertice
			//right face
		new Vector3(-0.75f,  1f, -0.75f), //top-left vertice
		new Vector3( -0.8f,  1f, -0.75f), //top-right vertice
		new Vector3( -0.8f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, -0.75f), //botom-left vertice
			//back face
		new Vector3(-0.8f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.8f,  1f, -0.75f), //top-right vertice
		new Vector3( -0.8f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.8f, -1f, 0.75f), //botom-left vertice
			//left face
		new Vector3(-0.8f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.75f,  1f, 0.75f), //top-right vertice
		new Vector3( -0.75f, -1f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -1f, 0.75f), //botom-left vertice
			// top face
		new Vector3(-0.75f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.8f, 1f, 0.75f), //top-right vertice
		new Vector3( -0.8f, 1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, 1f, -0.75f), //botom-left vertice
			//bottom face
		new Vector3(-0.75f, -1f, 0.75f), //top-left vertice
		new Vector3( -0.8f, -1f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, -0.75f), //botom-left vertice
		};

        List<Vector2> texCoords_kartina3 = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] kartina3_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int kartina3_VAO;
        int kartina3_VBO;
        int kartina3_EBO;

        int textureID_kartina3;
        int textureVBO_kartina3;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> kartina4_vertices = new List<Vector3>()
        {	
			//front face
		new Vector3(-0.75f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.75f,  1f, -0.75f), //top-right vertice
		new Vector3( -0.75f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, 0.75f), //botom-left vertice
			//right face
		new Vector3(-0.75f,  1f, -0.75f), //top-left vertice
		new Vector3( -0.8f,  1f, -0.75f), //top-right vertice
		new Vector3( -0.8f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, -0.75f), //botom-left vertice
			//back face
		new Vector3(-0.8f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.8f,  1f, -0.75f), //top-right vertice
		new Vector3( -0.8f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.8f, -1f, 0.75f), //botom-left vertice
			//left face
		new Vector3(-0.8f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.75f,  1f, 0.75f), //top-right vertice
		new Vector3( -0.75f, -1f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -1f, 0.75f), //botom-left vertice
			// top face
		new Vector3(-0.75f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.8f, 1f, 0.75f), //top-right vertice
		new Vector3( -0.8f, 1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, 1f, -0.75f), //botom-left vertice
			//bottom face
		new Vector3(-0.75f, -1f, 0.75f), //top-left vertice
		new Vector3( -0.8f, -1f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, -0.75f), //botom-left vertice
		};

        List<Vector2> texCoords_kartina4 = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] kartina4_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int kartina4_VAO;
        int kartina4_VBO;
        int kartina4_EBO;

        int textureID_kartina4;
        int textureVBO_kartina4;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> kartina5_vertices = new List<Vector3>()
        {	
			//front face
		new Vector3(-0.75f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.75f,  1f, -0.75f), //top-right vertice
		new Vector3( -0.75f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, 0.75f), //botom-left vertice
			//right face
		new Vector3(-0.75f,  1f, -0.75f), //top-left vertice
		new Vector3( -0.8f,  1f, -0.75f), //top-right vertice
		new Vector3( -0.8f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, -0.75f), //botom-left vertice
			//back face
		new Vector3(-0.8f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.8f,  1f, -0.75f), //top-right vertice
		new Vector3( -0.8f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.8f, -1f, 0.75f), //botom-left vertice
			//left face
		new Vector3(-0.8f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.75f,  1f, 0.75f), //top-right vertice
		new Vector3( -0.75f, -1f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -1f, 0.75f), //botom-left vertice
			// top face
		new Vector3(-0.75f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.8f, 1f, 0.75f), //top-right vertice
		new Vector3( -0.8f, 1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, 1f, -0.75f), //botom-left vertice
			//bottom face
		new Vector3(-0.75f, -1f, 0.75f), //top-left vertice
		new Vector3( -0.8f, -1f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, -0.75f), //botom-left vertice
		};

        List<Vector2> texCoords_kartina5 = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] kartina5_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int kartina5_VAO;
        int kartina5_VBO;
        int kartina5_EBO;

        int textureID_kartina5;
        int textureVBO_kartina5;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> kartina6_vertices = new List<Vector3>()
        {	
		    // front face (было: левая грань)
    new Vector3(0.75f,  1f, 0.75f),    // top-left
    new Vector3(-0.75f, 1f, 0.75f),     // top-right
    new Vector3(-0.75f, -1f, 0.75f),    // bottom-right
    new Vector3(0.75f, -1f, 0.75f),     // bottom-left

    // right face (было: передняя грань)
    new Vector3(-0.75f, 1f, 0.75f),     // top-left
    new Vector3(-0.75f, 1f, 0.8f),      // top-right
    new Vector3(-0.75f, -1f, 0.8f),     // bottom-right
    new Vector3(-0.75f, -1f, 0.75f),    // bottom-left

    // back face (было: правая грань)
    new Vector3(0.75f, 1f, 0.8f),       // top-left
    new Vector3(-0.75f, 1f, 0.8f),      // top-right
    new Vector3(-0.75f, -1f, 0.8f),     // bottom-right
    new Vector3(0.75f, -1f, 0.8f),      // bottom-left

    // left face (было: задняя грань)
    new Vector3(0.75f, 1f, 0.8f),       // top-left
    new Vector3(0.75f, 1f, 0.75f),      // top-right
    new Vector3(0.75f, -1f, 0.75f),     // bottom-right
    new Vector3(0.75f, -1f, 0.8f),      // bottom-left

    // top face (без изменений, кроме поворота X и Z)
    new Vector3(0.75f, 1f, 0.75f),      // top-left
    new Vector3(0.75f, 1f, 0.8f),       // top-right
    new Vector3(-0.75f, 1f, 0.8f),      // bottom-right
    new Vector3(-0.75f, 1f, 0.75f),     // bottom-left

    // bottom face (без изменений, кроме поворота X и Z)
    new Vector3(0.75f, -1f, 0.75f),     // top-left
    new Vector3(0.75f, -1f, 0.8f),      // top-right
    new Vector3(-0.75f, -1f, 0.8f),     // bottom-right
    new Vector3(-0.75f, -1f, 0.75f)     // bottom-left
		};

        List<Vector2> texCoords_kartina6 = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] kartina6_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int kartina6_VAO;
        int kartina6_VBO;
        int kartina6_EBO;

        int textureID_kartina6;
        int textureVBO_kartina6;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> kartina7_vertices = new List<Vector3>()
        {	
		    // front face (было: левая грань)
    new Vector3(0.75f,  1f, 0.75f),    // top-left
    new Vector3(-0.75f, 1f, 0.75f),     // top-right
    new Vector3(-0.75f, -1f, 0.75f),    // bottom-right
    new Vector3(0.75f, -1f, 0.75f),     // bottom-left

    // right face (было: передняя грань)
    new Vector3(-0.75f, 1f, 0.75f),     // top-left
    new Vector3(-0.75f, 1f, 0.8f),      // top-right
    new Vector3(-0.75f, -1f, 0.8f),     // bottom-right
    new Vector3(-0.75f, -1f, 0.75f),    // bottom-left

    // back face (было: правая грань)
    new Vector3(0.75f, 1f, 0.8f),       // top-left
    new Vector3(-0.75f, 1f, 0.8f),      // top-right
    new Vector3(-0.75f, -1f, 0.8f),     // bottom-right
    new Vector3(0.75f, -1f, 0.8f),      // bottom-left

    // left face (было: задняя грань)
    new Vector3(0.75f, 1f, 0.8f),       // top-left
    new Vector3(0.75f, 1f, 0.75f),      // top-right
    new Vector3(0.75f, -1f, 0.75f),     // bottom-right
    new Vector3(0.75f, -1f, 0.8f),      // bottom-left

    // top face (без изменений, кроме поворота X и Z)
    new Vector3(0.75f, 1f, 0.75f),      // top-left
    new Vector3(0.75f, 1f, 0.8f),       // top-right
    new Vector3(-0.75f, 1f, 0.8f),      // bottom-right
    new Vector3(-0.75f, 1f, 0.75f),     // bottom-left

    // bottom face (без изменений, кроме поворота X и Z)
    new Vector3(0.75f, -1f, 0.75f),     // top-left
    new Vector3(0.75f, -1f, 0.8f),      // top-right
    new Vector3(-0.75f, -1f, 0.8f),     // bottom-right
    new Vector3(-0.75f, -1f, 0.75f)     // bottom-left
		};

        List<Vector2> texCoords_kartina7 = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] kartina7_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int kartina7_VAO;
        int kartina7_VBO;
        int kartina7_EBO;

        int textureID_kartina7;
        int textureVBO_kartina7;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> kartina8_vertices = new List<Vector3>()
        {	
			//front face
		new Vector3(-0.75f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.75f,  1f, -0.75f), //top-right vertice
		new Vector3( -0.75f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, 0.75f), //botom-left vertice
			//right face
		new Vector3(-0.75f,  1f, -0.75f), //top-left vertice
		new Vector3( -0.8f,  1f, -0.75f), //top-right vertice
		new Vector3( -0.8f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, -0.75f), //botom-left vertice
			//back face
		new Vector3(-0.8f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.8f,  1f, -0.75f), //top-right vertice
		new Vector3( -0.8f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.8f, -1f, 0.75f), //botom-left vertice
			//left face
		new Vector3(-0.8f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.75f,  1f, 0.75f), //top-right vertice
		new Vector3( -0.75f, -1f, 0.75f), //bottom-right vertice
		new Vector3(-0.8f, -1f, 0.75f), //botom-left vertice
			// top face
		new Vector3(-0.75f,  1f, 0.75f), //top-left vertice
		new Vector3( -0.8f, 1f, 0.75f), //top-right vertice
		new Vector3( -0.8f, 1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, 1f, -0.75f), //botom-left vertice
			//bottom face
		new Vector3(-0.75f, -1f, 0.75f), //top-left vertice
		new Vector3( -0.8f, -1f, 0.75f), //top-right vertice
		new Vector3( -0.8f, -1f, -0.75f), //bottom-right vertice
		new Vector3(-0.75f, -1f, -0.75f), //botom-left vertice
		};

        List<Vector2> texCoords_kartina8 = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] kartina8_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int kartina8_VAO;
        int kartina8_VBO;
        int kartina8_EBO;

        int textureID_kartina8;
        int textureVBO_kartina8;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> door_vertices = new List<Vector3>()
        {	
			   // front face (было: левая грань)
    new Vector3(0.75f,  1f, 0.75f),    // top-left
    new Vector3(-0.75f, 1f, 0.75f),     // top-right
    new Vector3(-0.75f, -2f, 0.75f),    // bottom-right
    new Vector3(0.75f, -2f, 0.75f),     // bottom-left

    // right face (было: передняя грань)
    new Vector3(-0.75f, 1f, 0.75f),     // top-left
    new Vector3(-0.75f, 1f, 0.8f),      // top-right
    new Vector3(-0.75f, -2f, 0.8f),     // bottom-right
    new Vector3(-0.75f, -2f, 0.75f),    // bottom-left

    // back face (было: правая грань)
    new Vector3(0.75f,  1f, 0.8f),    // top-left (было: -0.8f → теперь 0.8f)
    new Vector3(-0.75f, 1f, 0.8f),     // top-right
    new Vector3(-0.75f, -2f, 0.8f),    // bottom-right
    new Vector3(0.75f, -2f, 0.8f),     // bottom-left (исправлено с 0.75f на 0.8f)

    // left face (было: задняя грань)
    new Vector3(0.75f, 1f, 0.8f),       // top-left
    new Vector3(0.75f, 1f, 0.75f),      // top-right
    new Vector3(0.75f, -2f, 0.75f),     // bottom-right
    new Vector3(0.75f, -2f, 0.8f),      // bottom-left

    // top face (без изменений, кроме поворота X и Z)
    new Vector3(0.75f, 1f, 0.75f),      // top-left
    new Vector3(0.75f, 1f, 0.8f),       // top-right
    new Vector3(-0.75f, 1f, 0.8f),      // bottom-right
    new Vector3(-0.75f, 1f, 0.75f),     // bottom-left

    // bottom face (без изменений, кроме поворота X и Z)
    new Vector3(0.75f, -2f, 0.75f),     // top-left
    new Vector3(0.75f, -2f, 0.8f),      // top-right
    new Vector3(-0.75f, -2f, 0.8f),     // bottom-right
    new Vector3(-0.75f, -2f, 0.75f)     // bottom-left
		};

        List<Vector2> texCoords_door = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] door_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int door_VAO;
        int door_VBO;
        int door_EBO;

        int textureID_door;
        int textureVBO_door;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> box1_vertices = new List<Vector3>()
        {	
//front face
			new Vector3(-0.5f,  0.5f, 0.5f), //top-left vertice
			new Vector3( 0.5f,  0.5f, 0.5f), //top-right vertice
			new Vector3( 0.5f, -0.5f, 0.5f), //bottom-right vertice
			new Vector3(-0.5f, -0.5f, 0.5f), //botom-left vertice
			//right face
			new Vector3( 0.5f,  0.5f, 0.5f), //top-left vertice
			new Vector3( 0.5f,  0.5f, -0.5f), //top-right vertice
			new Vector3( 0.5f, -0.5f, -0.5f), //bottom-right vertice
			new Vector3( 0.5f, -0.5f, 0.5f), //botom-left vertice
			//back face
			new Vector3(-0.5f,  0.5f, -0.5f), //top-left vertice
			new Vector3( 0.5f,  0.5f, -0.5f), //top-right vertice
			new Vector3( 0.5f, -0.5f, -0.5f), //bottom-right vertice
			new Vector3(-0.5f, -0.5f, -0.5f), //botom-left vertice
			//left face
			new Vector3( -0.5f,  0.5f, 0.5f), //top-left vertice
			new Vector3( -0.5f,  0.5f, -0.5f), //top-right vertice
			new Vector3( -0.5f, -0.5f, -0.5f), //bottom-right vertice
			new Vector3( -0.5f, -0.5f, 0.5f), //botom-left vertice
			// top face
			new Vector3(-0.5f,  0.5f, -0.5f), //top-left vertice
			new Vector3( 0.5f,  0.5f, -0.5f), //top-right vertice
			new Vector3( 0.5f, 0.5f, 0.5f), //bottom-right vertice
			new Vector3(-0.5f, 0.5f, 0.5f), //botom-left vertice
			//bottom face
			new Vector3(-0.5f,  -0.5f, -0.5f), //top-left vertice
			new Vector3( 0.5f,  -0.5f, -0.5f), //top-right vertice
			new Vector3( 0.5f, -0.5f, 0.5f), //bottom-right vertice
			new Vector3(-0.5f, -0.5f, 0.5f), //botom-left vertice
		};

        List<Vector2> texCoords_box1 = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] box1_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int box1_VAO;
        int box1_VBO;
        int box1_EBO;

        int textureID_box1;
        int textureVBO_box1;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> box2_vertices = new List<Vector3>()
        {	
			//front face
		new Vector3(-0.5f,  0.5f, 1f), //top-left vertice
		new Vector3( 0.5f,  0.5f, 1f), //top-right vertice
		new Vector3( 0.5f, -0.5f, 1f), //bottom-right vertice
		new Vector3(-0.5f, -0.5f, 1f), //botom-left vertice
		//right face
		new Vector3( 0.5f,  0.5f, 1f), //top-left vertice
		new Vector3( 0.5f,  0.5f, 2f), //top-right vertice
		new Vector3( 0.5f, -0.5f, 2f), //bottom-right vertice
		new Vector3( 0.5f, -0.5f, 1f), //botom-left vertice
		//back face
		new Vector3(-0.5f,  0.5f, 2f), //top-left vertice
		new Vector3( 0.5f,  0.5f, 2f), //top-right vertice
		new Vector3( 0.5f, -0.5f, 2f), //bottom-right vertice
		new Vector3(-0.5f, -0.5f, 2f), //botom-left vertice
		//left face
		new Vector3( -0.5f,  0.5f, 1f), //top-left vertice
		new Vector3( -0.5f,  0.5f, 2f), //top-right vertice
		new Vector3( -0.5f, -0.5f, 2f), //bottom-right vertice
		new Vector3( -0.5f, -0.5f, 1f), //botom-left vertice
		// top face
		new Vector3(-0.5f,  0.5f, 2f), //top-left vertice
		new Vector3( 0.5f,  0.5f, 2f), //top-right vertice
		new Vector3( 0.5f, 0.5f, 1f), //bottom-right vertice
		new Vector3(-0.5f, 0.5f, 1f), //botom-left vertice
		//bottom face
		new Vector3(-0.5f,  -0.5f, 2f), //top-left vertice
		new Vector3( 0.5f,  -0.5f, 2f), //top-right vertice
		new Vector3( 0.5f, -0.5f, 1f), //bottom-right vertice
		new Vector3(-0.5f, -0.5f, 1f), //botom-left vertice
		};

        List<Vector2> texCoords_box2 = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] box2_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int box2_VAO;
        int box2_VBO;
        int box2_EBO;

        int textureID_box2;
        int textureVBO_box2;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> box3_vertices = new List<Vector3>()
        {	
			//front face
		new Vector3(-0.5f,  0.5f, 1f), //top-left vertice
		new Vector3( 0.5f,  0.5f, 1f), //top-right vertice
		new Vector3( 0.5f, -0.5f, 1f), //bottom-right vertice
		new Vector3(-0.5f, -0.5f, 1f), //botom-left vertice
		//right face
		new Vector3( 0.5f,  0.5f, 1f), //top-left vertice
		new Vector3( 0.5f,  0.5f, 2f), //top-right vertice
		new Vector3( 0.5f, -0.5f, 2f), //bottom-right vertice
		new Vector3( 0.5f, -0.5f, 1f), //botom-left vertice
		//back face
		new Vector3(-0.5f,  0.5f, 2f), //top-left vertice
		new Vector3( 0.5f,  0.5f, 2f), //top-right vertice
		new Vector3( 0.5f, -0.5f, 2f), //bottom-right vertice
		new Vector3(-0.5f, -0.5f, 2f), //botom-left vertice
		//left face
		new Vector3( -0.5f,  0.5f, 1f), //top-left vertice
		new Vector3( -0.5f,  0.5f, 2f), //top-right vertice
		new Vector3( -0.5f, -0.5f, 2f), //bottom-right vertice
		new Vector3( -0.5f, -0.5f, 1f), //botom-left vertice
		// top face
		new Vector3(-0.5f,  0.5f, 2f), //top-left vertice
		new Vector3( 0.5f,  0.5f, 2f), //top-right vertice
		new Vector3( 0.5f, 0.5f, 1f), //bottom-right vertice
		new Vector3(-0.5f, 0.5f, 1f), //botom-left vertice
		//bottom face
		new Vector3(-0.5f,  -0.5f, 2f), //top-left vertice
		new Vector3( 0.5f,  -0.5f, 2f), //top-right vertice
		new Vector3( 0.5f, -0.5f, 1f), //bottom-right vertice
		new Vector3(-0.5f, -0.5f, 1f), //botom-left vertice
		};

        List<Vector2> texCoords_box3 = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] box3_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int box3_VAO;
        int box3_VBO;
        int box3_EBO;

        int textureID_box3;
        int textureVBO_box3;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Vector3> box4_vertices = new List<Vector3>()
        {	
			//front face
		new Vector3(-0.5f,  0.5f, 1f), //top-left vertice
		new Vector3( 0.5f,  0.5f, 1f), //top-right vertice
		new Vector3( 0.5f, -0.5f, 1f), //bottom-right vertice
		new Vector3(-0.5f, -0.5f, 1f), //botom-left vertice
		//right face
		new Vector3( 0.5f,  0.5f, 1f), //top-left vertice
		new Vector3( 0.5f,  0.5f, 2f), //top-right vertice
		new Vector3( 0.5f, -0.5f, 2f), //bottom-right vertice
		new Vector3( 0.5f, -0.5f, 1f), //botom-left vertice
		//back face
		new Vector3(-0.5f,  0.5f, 2f), //top-left vertice
		new Vector3( 0.5f,  0.5f, 2f), //top-right vertice
		new Vector3( 0.5f, -0.5f, 2f), //bottom-right vertice
		new Vector3(-0.5f, -0.5f, 2f), //botom-left vertice
		//left face
		new Vector3( -0.5f,  0.5f, 1f), //top-left vertice
		new Vector3( -0.5f,  0.5f, 2f), //top-right vertice
		new Vector3( -0.5f, -0.5f, 2f), //bottom-right vertice
		new Vector3( -0.5f, -0.5f, 1f), //botom-left vertice
		// top face
		new Vector3(-0.5f,  0.5f, 2f), //top-left vertice
		new Vector3( 0.5f,  0.5f, 2f), //top-right vertice
		new Vector3( 0.5f, 0.5f, 1f), //bottom-right vertice
		new Vector3(-0.5f, 0.5f, 1f), //botom-left vertice
		//bottom face
		new Vector3(-0.5f,  -0.5f, 2f), //top-left vertice
		new Vector3( 0.5f,  -0.5f, 2f), //top-right vertice
		new Vector3( 0.5f, -0.5f, 1f), //bottom-right vertice
		new Vector3(-0.5f, -0.5f, 1f), //botom-left vertice
		};

        List<Vector2> texCoords_box4 = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] box4_indices =
        {
            0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };


        int box4_VAO;
        int box4_VBO;
        int box4_EBO;

        int textureID_box4;
        int textureVBO_box4;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////








        Shader shaderProgram = new Shader();
        Camera camera;

		float yRot = 0f;
        //float yRotKartina = 45f;
        float yRotPlanet = 0f;
        float yRotLaser = 0f;
        float yRotLaser2 = 90f;
        float yRotLaser3 = 180f;

        public Game (int width, int height): base (GameWindowSettings.Default, NativeWindowSettings.Default)
		{
			this.CenterWindow(new Vector2i(width, height));
			this.height = height;
			this.width = width;
		}
		
		protected override void OnLoad()
		{
			base.OnLoad();

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            wall_VAO = GL.GenVertexArray();
            //Create VBO
            wall_VBO = GL.GenBuffer();
			//Bind the VBO 
			GL.BindBuffer(BufferTarget.ArrayBuffer, wall_VBO);
			//Copy vertices data to the buffer
			GL.BufferData(BufferTarget.ArrayBuffer, wall_vertices.Count * Vector3.SizeInBytes, wall_vertices.ToArray(), BufferUsageHint.StaticDraw);
			//Bind the VAO
			GL.BindVertexArray(wall_VAO);
			//Point a slot number 0
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			//Enable the slot
			GL.EnableVertexArrayAttrib(wall_VAO, 0);

			//Unbind the VBO
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            wall_EBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, wall_EBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, wall_indices.Length * sizeof(uint), wall_indices, BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

			//Create, bind texture
			textureVBO_wall = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_wall);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoords_wall.Count * Vector3.SizeInBytes , texCoords_wall.ToArray(), BufferUsageHint.StaticDraw);
			//Point a slot number 1
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			//Enable the slot
			GL.EnableVertexArrayAttrib(wall_VAO, 1);


			//Delete everything
			GL.BindVertexArray(0);

			shaderProgram.LoadShader();

            // Texture Loading
            textureID_wall = GL.GenTexture(); //Generate empty texture
			GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
			GL.BindTexture(TextureTarget.Texture2D, textureID_wall); //Bind texture

			//Texture parameters
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			//Load image
			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult wallTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/wall.jpg"), ColorComponents.RedGreenBlueAlpha);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, wallTexture.Width, wallTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, wallTexture.Data);

			//Unbind the texture
			GL.BindTexture(TextureTarget.Texture2D, 0);


			GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            wall1_VAO = GL.GenVertexArray();
            //Create VBO
            wall1_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, wall1_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, wall1_vertices.Count * Vector3.SizeInBytes, wall1_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(wall1_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(wall1_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            wall1_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, wall1_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, wall1_indices.Length * sizeof(uint), wall1_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_wall1 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_wall1);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_wall1.Count * Vector3.SizeInBytes, texCoords_wall1.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(wall1_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_wall1 = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_wall1); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult wall1Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/wall.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, wall1Texture.Width, wall1Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, wall1Texture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            monument_VAO = GL.GenVertexArray();
            //Create VBO
            monument_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, monument_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, monument_vertices.Count * Vector3.SizeInBytes, monument_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(monument_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(monument_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            monument_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, monument_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, monument_indices.Length * sizeof(uint), monument_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_monument = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_monument);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_monument.Count * Vector3.SizeInBytes, texCoords_monument.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(monument_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_monument = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_monument); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult monumentTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/monument.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, monumentTexture.Width, monumentTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, monumentTexture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            monument1_VAO = GL.GenVertexArray();
            //Create VBO
            monument1_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, monument1_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, monument1_vertices.Count * Vector3.SizeInBytes, monument1_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(monument1_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(monument1_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            monument1_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, monument1_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, monument1_indices.Length * sizeof(uint), monument1_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_monument1 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_monument1);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_monument1.Count * Vector3.SizeInBytes, texCoords_monument1.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(monument1_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_monument1 = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_monument1); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult monument1Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/monument.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, monument1Texture.Width, monument1Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, monument1Texture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            rakushka_VAO = GL.GenVertexArray();
            //Create VBO
            rakushka_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, rakushka_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, rakushka_vertices.Count * Vector3.SizeInBytes, rakushka_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(rakushka_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(rakushka_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            rakushka_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, rakushka_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, rakushka_indices.Length * sizeof(uint), rakushka_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_rakushka = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_rakushka);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_rakushka.Count * Vector3.SizeInBytes, texCoords_rakushka.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(rakushka_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_rakushka = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_rakushka); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult rakushkaTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/rakushka.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, rakushkaTexture.Width, rakushkaTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, rakushkaTexture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            floor_VAO = GL.GenVertexArray();
            //Create VBO
            floor_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, floor_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, floor_vertices.Count * Vector3.SizeInBytes, floor_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(floor_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(floor_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            floor_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, floor_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, floor_indices.Length * sizeof(uint), floor_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_floor = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_floor);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_floor.Count * Vector3.SizeInBytes, texCoords_floor.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(floor_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_floor = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_floor); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult floorTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/floor.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, floorTexture.Width, floorTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, floorTexture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            ceiling_VAO = GL.GenVertexArray();
            //Create VBO
            ceiling_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, ceiling_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, ceiling_vertices.Count * Vector3.SizeInBytes, ceiling_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(ceiling_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(ceiling_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            ceiling_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ceiling_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, ceiling_indices.Length * sizeof(uint), ceiling_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_ceiling = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_ceiling);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_ceiling.Count * Vector3.SizeInBytes, texCoords_ceiling.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(ceiling_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_ceiling = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_ceiling); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult ceilingTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/ceiling.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, ceilingTexture.Width, ceilingTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ceilingTexture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            var (globeVertices, globeTexCoords, globeIndices) = GenerateSphereVertices(0.5f, 1024);

            globe_Vertices = globeVertices;
            globe_TexCoords = globeTexCoords;
            globe_Indices = globeIndices;

            // Создание VAO/VBO/EBO для глобуса
            globe_VAO = GL.GenVertexArray();
            globe_VBO = GL.GenBuffer();
            globe_EBO = GL.GenBuffer();

            GL.BindVertexArray(globe_VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, globe_VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, globeVertices.Count * Vector3.SizeInBytes, globeVertices.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(globe_VAO, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, globe_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, globeIndices.Length * sizeof(uint), globeIndices, BufferUsageHint.StaticDraw);

            textureVBO_globe = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_globe);
            GL.BufferData(BufferTarget.ArrayBuffer, globeTexCoords.Count * Vector2.SizeInBytes, globeTexCoords.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(globe_VAO, 1);

            GL.BindVertexArray(0);

            // Загрузка текстуры глобуса
            textureID_globe = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureID_globe);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            // Замените путь на вашу текстуру глобуса
            ImageResult globeTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/planeta.jpg"), ColorComponents.RedGreenBlueAlpha);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, globeTexture.Width, globeTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, globeTexture.Data);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            ruka_VAO = GL.GenVertexArray();
            //Create VBO
            ruka_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, ruka_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, ruka_vertices.Count * Vector3.SizeInBytes, ruka_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(ruka_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(ruka_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            ruka_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ruka_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, ruka_indices.Length * sizeof(uint), ruka_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_ruka = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_ruka);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_ruka.Count * Vector3.SizeInBytes, texCoords_ruka.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(ruka_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_ruka = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_ruka); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult rukaTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/ruka.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, rukaTexture.Width, rukaTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, rukaTexture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            wall2_VAO = GL.GenVertexArray();
            //Create VBO
            wall2_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, wall2_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, wall2_vertices.Count * Vector3.SizeInBytes, wall2_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(wall2_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(wall2_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            wall2_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, wall2_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, wall2_indices.Length * sizeof(uint), wall2_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_wall2 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_wall2);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_wall2.Count * Vector3.SizeInBytes, texCoords_wall2.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(wall2_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_wall2 = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_wall2); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult wall2Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/wall.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, wall2Texture.Width, wall2Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, wall2Texture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            floor2_VAO = GL.GenVertexArray();
            //Create VBO
            floor2_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, floor2_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, floor2_vertices.Count * Vector3.SizeInBytes, floor2_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(floor2_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(floor2_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            floor2_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, floor2_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, floor2_indices.Length * sizeof(uint), floor2_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_floor2 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_floor2);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_floor2.Count * Vector3.SizeInBytes, texCoords_floor2.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(floor2_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_floor2 = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_floor2); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult floor2Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/floor.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, floor2Texture.Width, floor2Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, floor2Texture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            ceiling2_VAO = GL.GenVertexArray();
            //Create VBO
            ceiling2_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, ceiling2_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, ceiling2_vertices.Count * Vector3.SizeInBytes, ceiling2_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(ceiling2_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(ceiling2_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            ceiling2_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ceiling2_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, ceiling2_indices.Length * sizeof(uint), ceiling2_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_ceiling2 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_ceiling2);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_ceiling2.Count * Vector3.SizeInBytes, texCoords_ceiling2.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(ceiling2_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_ceiling2 = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_ceiling2); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult ceiling2Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/ceiling.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, ceiling2Texture.Width, ceiling2Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ceiling2Texture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            laser_VAO = GL.GenVertexArray();
            //Create VBO
            laser_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, laser_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, laser_vertices.Count * Vector3.SizeInBytes, laser_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(laser_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(laser_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            laser_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, laser_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, laser_indices.Length * sizeof(uint), laser_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_laser = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_laser);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_laser.Count * Vector3.SizeInBytes, texCoords_laser.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(laser_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_laser = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_laser); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult laserTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/laser.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, laserTexture.Width, laserTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, laserTexture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            var (planetaVertices, planetaTexCoords, planetaIndices) = GenerateSphereVertices(0.5f, 1024);

            planeta_Vertices = planetaVertices;
            planeta_TexCoords = planetaTexCoords;
            planeta_Indices = planetaIndices;

            // Создание VAO/VBO/EBO для глобуса
            planeta_VAO = GL.GenVertexArray();
            planeta_VBO = GL.GenBuffer();
            planeta_EBO = GL.GenBuffer();

            GL.BindVertexArray(planeta_VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, planeta_VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, planetaVertices.Count * Vector3.SizeInBytes, planetaVertices.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(planeta_VAO, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, planeta_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, planetaIndices.Length * sizeof(uint), planetaIndices, BufferUsageHint.StaticDraw);

            textureVBO_planeta = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_planeta);
            GL.BufferData(BufferTarget.ArrayBuffer, planetaTexCoords.Count * Vector2.SizeInBytes, planetaTexCoords.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(planeta_VAO, 1);

            GL.BindVertexArray(0);

            // Загрузка текстуры глобуса
            textureID_planeta = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureID_planeta);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            // Замените путь на вашу текстуру глобуса
            ImageResult planetaTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/planeta2.jpg"), ColorComponents.RedGreenBlueAlpha);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, planetaTexture.Width, planetaTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, planetaTexture.Data);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            laser2_VAO = GL.GenVertexArray();
            //Create VBO
            laser2_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, laser2_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, laser2_vertices.Count * Vector3.SizeInBytes, laser2_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(laser2_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(laser2_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            laser2_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, laser2_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, laser2_indices.Length * sizeof(uint), laser2_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_laser2 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_laser2);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_laser2.Count * Vector3.SizeInBytes, texCoords_laser2.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(laser2_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_laser2 = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_laser2); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult laser2Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/laser.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, laser2Texture.Width, laser2Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, laser2Texture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            laser3_VAO = GL.GenVertexArray();
            //Create VBO
            laser3_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, laser3_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, laser3_vertices.Count * Vector3.SizeInBytes, laser3_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(laser3_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(laser3_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            laser3_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, laser3_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, laser3_indices.Length * sizeof(uint), laser3_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_laser3 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_laser3);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_laser3.Count * Vector3.SizeInBytes, texCoords_laser3.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(laser3_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_laser3 = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_laser3); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult laser3Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/laser.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, laser3Texture.Width, laser3Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, laser3Texture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            monument2_VAO = GL.GenVertexArray();
            //Create VBO
            monument2_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, monument2_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, monument2_vertices.Count * Vector3.SizeInBytes, monument2_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(monument2_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(monument2_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            monument2_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, monument2_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, monument2_indices.Length * sizeof(uint), monument2_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_monument2 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_monument2);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_monument2.Count * Vector3.SizeInBytes, texCoords_monument2.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(monument2_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_monument2 = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_monument2); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult monument2Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/monument2.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, monument2Texture.Width, monument2Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, monument2Texture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            laser4_VAO = GL.GenVertexArray();
            //Create VBO
            laser4_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, laser4_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, laser4_vertices.Count * Vector3.SizeInBytes, laser4_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(laser4_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(laser4_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            laser4_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, laser4_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, laser4_indices.Length * sizeof(uint), laser4_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_laser4 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_laser4);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_laser4.Count * Vector3.SizeInBytes, texCoords_laser4.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(laser4_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_laser4 = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_laser4); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult laser4Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/sterjn.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, laser4Texture.Width, laser4Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, laser4Texture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            kartina1_VAO = GL.GenVertexArray();
            //Create VBO
            kartina1_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, kartina1_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, kartina1_vertices.Count * Vector3.SizeInBytes, kartina1_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(kartina1_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(kartina1_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            kartina1_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, kartina1_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, kartina1_indices.Length * sizeof(uint), kartina1_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_kartina1 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_kartina1);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_kartina1.Count * Vector3.SizeInBytes, texCoords_kartina1.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(kartina1_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_kartina1 = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_kartina1); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult kartina1Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/kartina1.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, kartina1Texture.Width, kartina1Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, kartina1Texture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            kartina2_VAO = GL.GenVertexArray();
            //Create VBO
            kartina2_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, kartina2_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, kartina2_vertices.Count * Vector3.SizeInBytes, kartina2_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(kartina2_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(kartina2_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            kartina2_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, kartina2_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, kartina2_indices.Length * sizeof(uint), kartina2_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_kartina2 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_kartina2);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_kartina2.Count * Vector3.SizeInBytes, texCoords_kartina2.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(kartina2_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_kartina2 = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_kartina2); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult kartina2Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/kartina2.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, kartina2Texture.Width, kartina2Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, kartina2Texture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            kartina3_VAO = GL.GenVertexArray();
            //Create VBO
            kartina3_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, kartina3_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, kartina3_vertices.Count * Vector3.SizeInBytes, kartina3_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(kartina3_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(kartina3_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            kartina3_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, kartina3_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, kartina3_indices.Length * sizeof(uint), kartina3_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_kartina3 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_kartina3);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_kartina3.Count * Vector3.SizeInBytes, texCoords_kartina3.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(kartina3_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_kartina3 = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_kartina3); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult kartina3Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/kartina3.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, kartina3Texture.Width, kartina3Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, kartina3Texture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            kartina4_VAO = GL.GenVertexArray();
            //Create VBO
            kartina4_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, kartina4_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, kartina4_vertices.Count * Vector3.SizeInBytes, kartina4_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(kartina4_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(kartina4_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            kartina4_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, kartina4_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, kartina4_indices.Length * sizeof(uint), kartina4_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_kartina4 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_kartina4);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_kartina4.Count * Vector3.SizeInBytes, texCoords_kartina4.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(kartina4_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_kartina4 = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_kartina4); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult kartina4Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/kartina4.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, kartina4Texture.Width, kartina4Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, kartina4Texture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            kartina5_VAO = GL.GenVertexArray();
            //Create VBO
            kartina5_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, kartina5_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, kartina5_vertices.Count * Vector3.SizeInBytes, kartina5_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(kartina5_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(kartina5_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            kartina5_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, kartina5_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, kartina5_indices.Length * sizeof(uint), kartina5_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_kartina5 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_kartina5);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_kartina5.Count * Vector3.SizeInBytes, texCoords_kartina5.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(kartina5_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_kartina5 = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_kartina5); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult kartina5Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/kartina5.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, kartina5Texture.Width, kartina5Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, kartina5Texture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            kartina6_VAO = GL.GenVertexArray();
            //Create VBO
            kartina6_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, kartina6_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, kartina6_vertices.Count * Vector3.SizeInBytes, kartina6_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(kartina6_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(kartina6_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            kartina6_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, kartina6_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, kartina6_indices.Length * sizeof(uint), kartina6_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_kartina6 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_kartina6);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_kartina6.Count * Vector3.SizeInBytes, texCoords_kartina6.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(kartina6_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_kartina6 = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_kartina6); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult kartina6Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/kartina6.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, kartina6Texture.Width, kartina6Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, kartina6Texture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            kartina7_VAO = GL.GenVertexArray();
            //Create VBO
            kartina7_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, kartina7_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, kartina7_vertices.Count * Vector3.SizeInBytes, kartina7_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(kartina7_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(kartina7_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            kartina7_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, kartina7_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, kartina7_indices.Length * sizeof(uint), kartina7_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_kartina7 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_kartina7);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_kartina7.Count * Vector3.SizeInBytes, texCoords_kartina7.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(kartina7_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_kartina7 = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_kartina7); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult kartina7Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/kartina7.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, kartina7Texture.Width, kartina7Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, kartina7Texture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            kartina8_VAO = GL.GenVertexArray();
            //Create VBO
            kartina8_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, kartina8_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, kartina8_vertices.Count * Vector3.SizeInBytes, kartina8_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(kartina8_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(kartina8_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            kartina8_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, kartina8_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, kartina8_indices.Length * sizeof(uint), kartina8_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_kartina8 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_kartina8);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_kartina8.Count * Vector3.SizeInBytes, texCoords_kartina8.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(kartina8_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_kartina8 = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_kartina8); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult kartina8Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/kartina8.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, kartina8Texture.Width, kartina8Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, kartina8Texture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            door_VAO = GL.GenVertexArray();
            //Create VBO
            door_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, door_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, door_vertices.Count * Vector3.SizeInBytes, door_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(door_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(door_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            door_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, door_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, door_indices.Length * sizeof(uint), door_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_door = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_door);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_door.Count * Vector3.SizeInBytes, texCoords_door.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(door_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_door = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_door); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult doorTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/door.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, doorTexture.Width, doorTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, doorTexture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            box2_VAO = GL.GenVertexArray();
            //Create VBO
            box2_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, box2_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, box2_vertices.Count * Vector3.SizeInBytes, box2_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(box2_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(box2_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            box2_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, box2_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, box2_indices.Length * sizeof(uint), box2_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_box2 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_box2);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_box2.Count * Vector3.SizeInBytes, texCoords_box2.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(box2_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_box2 = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_box2); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult box2Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/box2.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, box2Texture.Width, box2Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, box2Texture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            box1_VAO = GL.GenVertexArray();
            //Create VBO
            box1_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, box1_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, box1_vertices.Count * Vector3.SizeInBytes, box1_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(box1_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(box1_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            box1_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, box1_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, box1_indices.Length * sizeof(uint), box1_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_box1 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_box1);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_box1.Count * Vector3.SizeInBytes, texCoords_box1.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(box1_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_box1 = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_box1); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult box1Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/box1.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, box1Texture.Width, box1Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, box1Texture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            box3_VAO = GL.GenVertexArray();
            //Create VBO
            box3_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, box3_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, box3_vertices.Count * Vector3.SizeInBytes, box3_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(box3_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(box3_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            box3_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, box3_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, box3_indices.Length * sizeof(uint), box3_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_box3 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_box3);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_box3.Count * Vector3.SizeInBytes, texCoords_box3.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(box3_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_box3 = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_box3); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult box3Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/box3.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, box3Texture.Width, box3Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, box3Texture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Create VAO
            box4_VAO = GL.GenVertexArray();
            //Create VBO
            box4_VBO = GL.GenBuffer();
            //Bind the VBO 
            GL.BindBuffer(BufferTarget.ArrayBuffer, box4_VBO);
            //Copy vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, box4_vertices.Count * Vector3.SizeInBytes, box4_vertices.ToArray(), BufferUsageHint.StaticDraw);
            //Bind the VAO
            GL.BindVertexArray(box4_VAO);
            //Point a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(box4_VAO, 0);

            //Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //EBO 
            box4_EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, box4_EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, box4_indices.Length * sizeof(uint), box4_indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO_box4 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO_box4);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords_box4.Count * Vector3.SizeInBytes, texCoords_box4.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(box4_VAO, 1);


            //Delete everything
            GL.BindVertexArray(0);

            shaderProgram.LoadShader();

            // Texture Loading
            textureID_box4 = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID_box4); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult box4Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/box4.jpg"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, box4Texture.Width, box4Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, box4Texture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.Enable(EnableCap.DepthTest);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////










            camera = new Camera(width, height, new Vector3(4, 5, 3));
			CursorState = CursorState.Grabbed;
		}

		protected override void OnUnload()
		{
			base.OnUnload();

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			
            GL.DeleteBuffer(wall_VAO);
			GL.DeleteBuffer(wall_VBO);
			GL.DeleteBuffer(wall_EBO);
			GL.DeleteTexture(textureID_wall);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(wall1_VAO);
            GL.DeleteBuffer(wall1_VBO);
            GL.DeleteBuffer(wall1_EBO);
            GL.DeleteTexture(textureID_wall1);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(monument_VAO);
            GL.DeleteBuffer(monument_VBO);
            GL.DeleteBuffer(monument_EBO);
            GL.DeleteTexture(textureID_monument);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            
            GL.DeleteBuffer(monument1_VAO);
            GL.DeleteBuffer(monument1_VBO);
            GL.DeleteBuffer(monument1_EBO);
            GL.DeleteTexture(textureID_monument1);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(rakushka_VAO);
            GL.DeleteBuffer(rakushka_VBO);
            GL.DeleteBuffer(rakushka_EBO);
            GL.DeleteTexture(textureID_rakushka);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(floor_VAO);
            GL.DeleteBuffer(floor_VBO);
            GL.DeleteBuffer(floor_EBO);
            GL.DeleteTexture(textureID_floor);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(ceiling_VAO);
            GL.DeleteBuffer(ceiling_VBO);
            GL.DeleteBuffer(ceiling_EBO);
            GL.DeleteTexture(textureID_ceiling);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(globe_VAO);
            GL.DeleteBuffer(globe_VBO);
            GL.DeleteBuffer(globe_EBO);
            GL.DeleteTexture(textureID_globe);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(ruka_VAO);
            GL.DeleteBuffer(ruka_VBO);
            GL.DeleteBuffer(ruka_EBO);
            GL.DeleteTexture(textureID_ruka);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(wall2_VAO);
            GL.DeleteBuffer(wall2_VBO);
            GL.DeleteBuffer(wall2_EBO);
            GL.DeleteTexture(textureID_wall2);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(floor2_VAO);
            GL.DeleteBuffer(floor2_VBO);
            GL.DeleteBuffer(floor2_EBO);
            GL.DeleteTexture(textureID_floor2);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(ceiling2_VAO);
            GL.DeleteBuffer(ceiling2_VBO);
            GL.DeleteBuffer(ceiling2_EBO);
            GL.DeleteTexture(textureID_ceiling2);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(laser_VAO);
            GL.DeleteBuffer(laser_VBO);
            GL.DeleteBuffer(laser_EBO);
            GL.DeleteTexture(textureID_laser);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(planeta_VAO);
            GL.DeleteBuffer(planeta_VBO);
            GL.DeleteBuffer(planeta_EBO);
            GL.DeleteTexture(textureID_planeta);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(laser2_VAO);
            GL.DeleteBuffer(laser2_VBO);
            GL.DeleteBuffer(laser2_EBO);
            GL.DeleteTexture(textureID_laser2);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(laser3_VAO);
            GL.DeleteBuffer(laser3_VBO);
            GL.DeleteBuffer(laser3_EBO);
            GL.DeleteTexture(textureID_laser3);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(monument2_VAO);
            GL.DeleteBuffer(monument2_VBO);
            GL.DeleteBuffer(monument2_EBO);
            GL.DeleteTexture(textureID_monument2);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(laser4_VAO);
            GL.DeleteBuffer(laser4_VBO);
            GL.DeleteBuffer(laser4_EBO);
            GL.DeleteTexture(textureID_laser4);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(kartina1_VAO);
            GL.DeleteBuffer(kartina1_VBO);
            GL.DeleteBuffer(kartina1_EBO);
            GL.DeleteTexture(textureID_kartina1);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(kartina2_VAO);
            GL.DeleteBuffer(kartina2_VBO);
            GL.DeleteBuffer(kartina2_EBO);
            GL.DeleteTexture(textureID_kartina2);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(kartina3_VAO);
            GL.DeleteBuffer(kartina3_VBO);
            GL.DeleteBuffer(kartina3_EBO);
            GL.DeleteTexture(textureID_kartina3);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(kartina4_VAO);
            GL.DeleteBuffer(kartina4_VBO);
            GL.DeleteBuffer(kartina4_EBO);
            GL.DeleteTexture(textureID_kartina4);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(kartina5_VAO);
            GL.DeleteBuffer(kartina5_VBO);
            GL.DeleteBuffer(kartina5_EBO);
            GL.DeleteTexture(textureID_kartina5);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(kartina6_VAO);
            GL.DeleteBuffer(kartina6_VBO);
            GL.DeleteBuffer(kartina6_EBO);
            GL.DeleteTexture(textureID_kartina6);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(kartina7_VAO);
            GL.DeleteBuffer(kartina7_VBO);
            GL.DeleteBuffer(kartina7_EBO);
            GL.DeleteTexture(textureID_kartina7);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(kartina8_VAO);
            GL.DeleteBuffer(kartina8_VBO);
            GL.DeleteBuffer(kartina8_EBO);
            GL.DeleteTexture(textureID_kartina8);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(door_VAO);
            GL.DeleteBuffer(door_VBO);
            GL.DeleteBuffer(door_EBO);
            GL.DeleteTexture(textureID_door);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(box1_VAO);
            GL.DeleteBuffer(box1_VBO);
            GL.DeleteBuffer(box1_EBO);
            GL.DeleteTexture(textureID_box1);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(box2_VAO);
            GL.DeleteBuffer(box2_VBO);
            GL.DeleteBuffer(box2_EBO);
            GL.DeleteTexture(textureID_box2);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(box3_VAO);
            GL.DeleteBuffer(box3_VBO);
            GL.DeleteBuffer(box3_EBO);
            GL.DeleteTexture(textureID_box3);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.DeleteBuffer(box4_VAO);
            GL.DeleteBuffer(box4_VBO);
            GL.DeleteBuffer(box4_EBO);
            GL.DeleteTexture(textureID_box4);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////








            shaderProgram.DeleteShader();

		}

		protected override void OnRenderFrame(FrameEventArgs args)
		{
			GL.ClearColor(0.3f, 0.3f, 1f, 1f);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			shaderProgram.UseShader();

            //Transformation
            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = camera.GetProjection();
            int modelLocation = GL.GetUniformLocation(shaderProgram.shaderHandle, "model");
            int viewLocation = GL.GetUniformLocation(shaderProgram.shaderHandle, "view");
            int projectionLocation = GL.GetUniformLocation(shaderProgram.shaderHandle, "projection");

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_wall);

            Matrix4 wallModel = Matrix4.CreateTranslation(0f, 8f, 3f);

			GL.UniformMatrix4(modelLocation, true, ref wallModel);
            GL.BindVertexArray(wall_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, wall_EBO);
            GL.DrawElements(PrimitiveType.Triangles, wall_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_wall1);


            Matrix4 wall1Model = Matrix4.CreateTranslation(-4f, 8f, 0f);

            GL.UniformMatrix4(modelLocation, true, ref wall1Model);
            GL.BindVertexArray(wall1_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, wall1_EBO);
            GL.DrawElements(PrimitiveType.Triangles, wall1_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_monument);


            Matrix4 monumentModel = Matrix4.CreateTranslation(-4f, 3.5f, 6f);

            GL.UniformMatrix4(modelLocation, true, ref monumentModel);
            GL.BindVertexArray(monument_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, monument_EBO);
            GL.DrawElements(PrimitiveType.Triangles, monument_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_monument1);


            Matrix4 monument1Model = Matrix4.CreateTranslation(-1f, 3.5f, 4f);

            GL.UniformMatrix4(modelLocation, true, ref monument1Model);
            GL.BindVertexArray(monument1_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, monument1_EBO);
            GL.DrawElements(PrimitiveType.Triangles, monument1_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_rakushka);


            Matrix4 rakushkaModel = Matrix4.CreateTranslation(-4f, 4.5f, 6f);

            GL.UniformMatrix4(modelLocation, true, ref rakushkaModel);
            GL.BindVertexArray(rakushka_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, rakushka_EBO);
            GL.DrawElements(PrimitiveType.Triangles, rakushka_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_floor);

            Matrix4 floorModel = Matrix4.CreateTranslation(0f, 8f, 3f);

            GL.UniformMatrix4(modelLocation, true, ref floorModel);
            GL.BindVertexArray(floor_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, floor_EBO);
            GL.DrawElements(PrimitiveType.Triangles, floor_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_ceiling);

            Matrix4 ceilingModel = Matrix4.CreateTranslation(0f, 8f, 3f);

            GL.UniformMatrix4(modelLocation, true, ref ceilingModel);
            GL.BindVertexArray(ceiling_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ceiling_EBO);
            GL.DrawElements(PrimitiveType.Triangles, ceiling_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_globe);

            Matrix4 globeModel = Matrix4.CreateRotationY(yRot) * Matrix4.CreateTranslation(-1f, 4.5f, 5.5f);
            yRot += 0.001f;

            GL.UniformMatrix4(modelLocation, true, ref globeModel);
            GL.BindVertexArray(globe_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, globe_EBO);
            GL.DrawElements(PrimitiveType.Triangles, globe_Indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_ruka);

            Matrix4 rukaModel = Matrix4.CreateTranslation(1.8f, 5f, 3f);

            GL.UniformMatrix4(modelLocation, true, ref rukaModel);
            GL.BindVertexArray(ruka_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ruka_EBO);
            GL.DrawElements(PrimitiveType.Triangles, ruka_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_wall2);

            Matrix4 wall2Model = Matrix4.CreateTranslation(0f, 8f, -7f);

            GL.UniformMatrix4(modelLocation, true, ref wall2Model);
            GL.BindVertexArray(wall2_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, wall2_EBO);
            GL.DrawElements(PrimitiveType.Triangles, wall2_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_floor2);

            Matrix4 floor2Model = Matrix4.CreateTranslation(0f, 8f, -7f);

            GL.UniformMatrix4(modelLocation, true, ref floor2Model);
            GL.BindVertexArray(floor2_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, floor2_EBO);
            GL.DrawElements(PrimitiveType.Triangles, floor2_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_ceiling2);

            Matrix4 ceiling2Model = Matrix4.CreateTranslation(0f, 8f, -7f);

            GL.UniformMatrix4(modelLocation, true, ref ceiling2Model);
            GL.BindVertexArray(ceiling2_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ceiling2_EBO);
            GL.DrawElements(PrimitiveType.Triangles, ceiling2_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_laser);

            Matrix4 laserModel = Matrix4.CreateRotationY(yRotLaser) * Matrix4.CreateTranslation(0f, 8f, -7f);
            yRotLaser += 0.001f;
            GL.UniformMatrix4(modelLocation, true, ref laserModel);
            GL.BindVertexArray(laser_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, laser_EBO);
            GL.DrawElements(PrimitiveType.Triangles, laser_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_planeta);

            Matrix4 planetaModel = Matrix4.CreateRotationY(yRotPlanet) * Matrix4.CreateTranslation(0f, 5f, -7f);
            yRotPlanet -= 0.01f;

            GL.UniformMatrix4(modelLocation, true, ref planetaModel);
            GL.BindVertexArray(planeta_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, planeta_EBO);
            GL.DrawElements(PrimitiveType.Triangles, planeta_Indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_laser2);

            Matrix4 laser2Model = Matrix4.CreateRotationY(yRotLaser2) * Matrix4.CreateTranslation(0f, 8f, -7f);
            yRotLaser2 += 0.001f;
            GL.UniformMatrix4(modelLocation, true, ref laser2Model);
            GL.BindVertexArray(laser2_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, laser2_EBO);
            GL.DrawElements(PrimitiveType.Triangles, laser2_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_laser3);

            Matrix4 laser3Model = Matrix4.CreateRotationY(yRotLaser3) * Matrix4.CreateTranslation(0f, 8f, -7f);
            yRotLaser3 += 0.001f;
            GL.UniformMatrix4(modelLocation, true, ref laser3Model);
            GL.BindVertexArray(laser3_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, laser3_EBO);
            GL.DrawElements(PrimitiveType.Triangles, laser3_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_monument2);


            Matrix4 monument2Model = Matrix4.CreateTranslation(0f, 3.5f, -8.5f);

            GL.UniformMatrix4(modelLocation, true, ref monument2Model);
            GL.BindVertexArray(monument2_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, monument2_EBO);
            GL.DrawElements(PrimitiveType.Triangles, monument2_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_laser4);

            Matrix4 laser4Model = Matrix4.CreateTranslation(0.77f, 8f, -7.75f);

            GL.UniformMatrix4(modelLocation, true, ref laser4Model);
            GL.BindVertexArray(laser4_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, laser4_EBO);
            GL.DrawElements(PrimitiveType.Triangles, laser4_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_kartina1);

            Matrix4 kartina1Model = Matrix4.CreateTranslation(-4.2f, 5f, -3f);

            GL.UniformMatrix4(modelLocation, true, ref kartina1Model);
            GL.BindVertexArray(kartina1_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, kartina1_EBO);
            GL.DrawElements(PrimitiveType.Triangles, kartina1_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_kartina2);

            Matrix4 kartina2Model = Matrix4.CreateTranslation(-4.2f, 5f, -9f);

            GL.UniformMatrix4(modelLocation, true, ref kartina2Model);
            GL.BindVertexArray(kartina2_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, kartina2_EBO);
            GL.DrawElements(PrimitiveType.Triangles, kartina2_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_kartina3);

            Matrix4 kartina3Model = Matrix4.CreateTranslation(5.75f, 5f, -3f);

            GL.UniformMatrix4(modelLocation, true, ref kartina3Model);
            GL.BindVertexArray(kartina3_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, kartina3_EBO);
            GL.DrawElements(PrimitiveType.Triangles, kartina3_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_kartina4);

            Matrix4 kartina4Model = Matrix4.CreateTranslation(5.75f, 5f, -9f);

            GL.UniformMatrix4(modelLocation, true, ref kartina4Model);
            GL.BindVertexArray(kartina4_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, kartina4_EBO);
            GL.DrawElements(PrimitiveType.Triangles, kartina4_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_kartina5);

            Matrix4 kartina5Model = Matrix4.CreateTranslation(-4.2f, 9.5f, -6f);

            GL.UniformMatrix4(modelLocation, true, ref kartina5Model);
            GL.BindVertexArray(kartina5_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, kartina5_EBO);
            GL.DrawElements(PrimitiveType.Triangles, kartina5_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_kartina6);

            Matrix4 kartina6Model = Matrix4.CreateTranslation(-2f, 8f, -12.75f);

            GL.UniformMatrix4(modelLocation, true, ref kartina6Model);
            GL.BindVertexArray(kartina6_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, kartina6_EBO);
            GL.DrawElements(PrimitiveType.Triangles, kartina6_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_kartina7);

            Matrix4 kartina7Model = Matrix4.CreateTranslation(2f, 5f, -12.75f);

            GL.UniformMatrix4(modelLocation, true, ref kartina7Model);
            GL.BindVertexArray(kartina7_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, kartina7_EBO);
            GL.DrawElements(PrimitiveType.Triangles, kartina7_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_kartina8);

            Matrix4 kartina8Model = Matrix4.CreateTranslation(5.75f, 9.5f, -6f);

            GL.UniformMatrix4(modelLocation, true, ref kartina8Model);
            GL.BindVertexArray(kartina8_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, kartina8_EBO);
            GL.DrawElements(PrimitiveType.Triangles, kartina8_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_door);

            Matrix4 doorModel = Matrix4.CreateTranslation(-2f, 5f, 0.225f);

            GL.UniformMatrix4(modelLocation, true, ref doorModel);
            GL.BindVertexArray(door_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, door_EBO);
            GL.DrawElements(PrimitiveType.Triangles, door_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_box1);


            Matrix4 box1Model = Matrix4.CreateRotationY(-45f) * Matrix4.CreateTranslation(-4f, 3.5f, 4f);

            GL.UniformMatrix4(modelLocation, true, ref box1Model);
            GL.BindVertexArray(box1_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, box1_EBO);
            GL.DrawElements(PrimitiveType.Triangles, box1_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_box2);


            Matrix4 box2Model = Matrix4.CreateTranslation(-1f, 3.5f, 2.75f);

            GL.UniformMatrix4(modelLocation, true, ref box2Model);
            GL.BindVertexArray(box2_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, box2_EBO);
            GL.DrawElements(PrimitiveType.Triangles, box2_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_box3);


            Matrix4 box3Model = Matrix4.CreateTranslation(-4.5f, 3.5f, 0.5f);

            GL.UniformMatrix4(modelLocation, true, ref box3Model);
            GL.BindVertexArray(box3_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, box3_EBO);
            GL.DrawElements(PrimitiveType.Triangles, box3_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GL.BindTexture(TextureTarget.Texture2D, textureID_box4);


            Matrix4 box4Model = Matrix4.CreateRotationY(7f) * Matrix4.CreateTranslation(-1f, 3.5f, 0.75f);

            GL.UniformMatrix4(modelLocation, true, ref box4Model);
            GL.BindVertexArray(box4_VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, box4_EBO);
            GL.DrawElements(PrimitiveType.Triangles, box4_indices.Length, DrawElementsType.UnsignedInt, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////















            GL.UniformMatrix4(viewLocation, true, ref view);
			GL.UniformMatrix4(projectionLocation, true, ref projection);


			Context.SwapBuffers();

			base.OnRenderFrame(args);
		}

		protected override void OnUpdateFrame(FrameEventArgs args)
		{
			
			if (KeyboardState.IsKeyDown(Keys.Escape))
			{
				Close();
			}
			MouseState mouse = MouseState;
			KeyboardState input = KeyboardState;

			base.OnUpdateFrame(args);
			camera.Update(input, mouse, args);

		}

		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);
			GL.Viewport(0, 0, e.Width, e.Height);
			this.width = e.Width;
			this.height = e.Height;
		}

        private (List<Vector3>, List<Vector2>, uint[]) GenerateSphereVertices(float radius, int segments)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> texCoords = new List<Vector2>();
            List<uint> indices = new List<uint>();

            for (int y = 0; y <= segments; y++)
            {
                for (int x = 0; x <= segments; x++)
                {
                    float xSegment = (float)x / segments;
                    float ySegment = (float)y / segments;

                    float xPos = radius * MathF.Cos(xSegment * 2 * MathF.PI) * MathF.Sin(ySegment * MathF.PI);
                    float yPos = radius * MathF.Cos(ySegment * MathF.PI);
                    float zPos = radius * MathF.Sin(xSegment * 2 * MathF.PI) * MathF.Sin(ySegment * MathF.PI);

                    vertices.Add(new Vector3(xPos, yPos, zPos));
                    texCoords.Add(new Vector2(xSegment, ySegment));
                }
            }

            for (int y = 0; y < segments; y++)
            {
                for (int x = 0; x < segments; x++)
                {
                    indices.Add((uint)(y * (segments + 1) + x));
                    indices.Add((uint)((y + 1) * (segments + 1) + x));
                    indices.Add((uint)((y + 1) * (segments + 1) + x + 1));

                    indices.Add((uint)(y * (segments + 1) + x));
                    indices.Add((uint)((y + 1) * (segments + 1) + x + 1));
                    indices.Add((uint)(y * (segments + 1) + x + 1));
                }
            }

            return (vertices, texCoords, indices.ToArray());
        }
    }
}
