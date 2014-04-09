//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace Delauney
{
	public class Point2D
	{
		public float x,y;
		public Point2D()
		{
			x = 0;
			y = 0;
		}
		public Point2D(float a_X,float a_Y)
		{
			x = a_X;
			y = a_Y;
		}
		public float Distance(Point2D A, Point2D B)
		{
			return (float) Math.Sqrt((B.x +A.x)*(B.x +A.x) +(B.y +A.y)*(B.y +A.y));
		}
		public float Distance(Point2D B)
		{
			return (float) Math.Sqrt((B.x +x)*(B.x +x) +(B.y +y)*(B.y +y));
		}
	}
	public class Edge
	{
		public Point2D A,B; 
		public Edge(){}
		public Edge(Point2D a_A,Point2D a_B)
		{
			A = a_A;
			B = a_B;
		}


	}
	public class Circle
	{
		public Point2D center;
		public float radius;
		
		public Circle()
		{
			radius = 0;
			center = new Point2D();
		}
		public bool Inside(Point2D B)
		{
			if(radius > center.Distance(B))
				return true;
			return false;
		}
		
	}
	public class Triangle
	{
		public Point2D A,B,C;
		public Circle Circle;
		public Edge AB
		{
			get{return new Edge(A,B);}
		}
		public Edge BC
		{
			get{return new Edge(B,C);}
		}
		public Edge CA
		{
			get{return new Edge(C,A);}
		}
		public Triangle(){}

		public Triangle(Point2D a_A,Point2D a_B,Point2D a_C)
		{
			A = a_A;
			B = a_B;
			C = a_C;
			CircumCircle();
		}

		public Circle CircumCircle()
		{
			Circle = new Circle();
			if(Circle.radius == 0)
			{
				Circle.center = CircumCenter();
				Circle.radius = Circle.center.Distance(A);
			}
			return Circle;

		}
		public Point2D CircumCenter()
		{
			Point2D center = new Point2D();
			float Divisor = 2*(A.x*(B.y -C.y) + B.x*(C.y - A.y) + C.x*(A.y - B.y));
			center.x = ((((A.x*A.x) + (A.y*A.y)) * (B.y-C.y)) + (((B.x*B.x) + (B.y*B.y)) * (C.y-A.y)) + (((C.x*C.x) + (C.y*C.y))* (A.y-B.y)))/Divisor;
			center.y = ((((A.x*A.x) + (A.y*A.y)) * (C.x-B.x)) + (((B.x*B.x) + (B.y*B.y)) * (A.x-C.x)) + (((C.x*C.x) + (C.y*C.y))* (B.x-A.x)))/Divisor;
			return center;
		}
	}



	public class Delaunay
	{

		public List<Triangle> triList;
		public List<Point2D> vertexList;

		public Delaunay ()
		{
		}
		public void Triangulate(List<Point2D> inputList)
		{
			vertexList = new List<Point2D>();
			triList = new List<Triangle>();
			vertexList = inputList;
			triList.Add(new Triangle(vertexList[0],vertexList[1],vertexList[2]));

			for(int i = 3; i < vertexList.Count;i++)
			{
				for(int j = triList.Count; j > 0; j--)
				{
					bool tri1Check = false;
					bool tri2Check = false;
					bool tri3Check = false;

					Triangle tri1 = new Triangle(triList[j-1].A,triList[j-1].B,vertexList[i]);
					Triangle tri2 = new Triangle(triList[j-1].C,triList[j-1].B,vertexList[i]);
					Triangle tri3 = new Triangle(triList[j-1].A,triList[j-1].C,vertexList[i]);

					for(int k = 0; k <= i; k++ )
					{
						if(!tri1Check)
							tri1Check = tri1.Circle.Inside(vertexList[k]);

						if(!tri2Check)
							tri2Check = tri2.Circle.Inside(vertexList[k]);

						if(!tri3Check)
							tri3Check = tri3.Circle.Inside(vertexList[k]);
					
					}
					if(!tri1Check)
						triList.Add(tri1);
					if(!tri2Check)
						triList.Add(tri2);
					if(!tri3Check)
						triList.Add(tri3);

				}
			}
			int z = 0;

		}

	}
}

