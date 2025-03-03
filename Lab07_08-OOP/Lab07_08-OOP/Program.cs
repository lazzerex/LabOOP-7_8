using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;


public class Point
{
    private static Random random = new Random();
    private static int nextId = 0;
    private char name;

    private double x;
    private double y;

    public Point()
    {
        this.x = 0;
        this.y = 0;
        this.name = (char)('A' + (nextId++ % 26));
    }
   
    public Point(double x, double y)
    {
        this.x = x;
        this.y = y;
        this.name = (char)('A' + (nextId++ % 26));
    }

    public Point(Point other)
    {
        this.x = other.x;
        this.y = other.y;
        this.name = other.name;
    }

    public double X
    {
        get { return x; }
        set { x = value; }
    }

    public double Y
    {
        get { return y; }
        set { y = value; }
    }

    public override string ToString()
    {
        //return $"{(char)('A' + new Random().Next(26))}({x}, {y})";
        return $"{name}({x}, {y})";
    }

    public double Distance(Point other)
    {
        return Math.Sqrt(Math.Pow(this.x - other.x, 2) + Math.Pow(this.y - other.y, 2));
    }
}

public class Cluster
{
    private List<Point> points;

    public Cluster()
    {
        this.points = new List<Point>();
    }

    public Cluster(Point point)
    {
        this.points = new List<Point>();
        this.points.Add(new Point(point));
    }

    public Cluster(List<Point> points)
    {
        this.points = new List<Point>();
        foreach (Point p in points)
        {
            this.points.Add(new Point(p));
        }
    }

    public void Add(Point point)
    {
        this.points.Add(new Point(point));
    }

    public List<Point> Points
    {
        get { return points; }
    }

    public override string ToString()
    {
        string result = "{";
        for (int i = 0; i < points.Count; i++)
        {
            result += points[i].ToString();
            if (i < points.Count - 1)
            {
                result += ", ";
            }
        }
        result += "}";
        return result;
    }

    public double Distance(Cluster other)
    {
        if (this.points.Count == 0 || other.points.Count == 0)
        {
            return double.MaxValue;
        }

        double minDistance = double.MaxValue;
        foreach (Point p1 in this.points)
        {
            foreach (Point p2 in other.points)
            {
                double distance = p1.Distance(p2);
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }
        }
        return minDistance;
    }

    public static Cluster operator +(Cluster a, Cluster b)
    {
        Cluster result = new Cluster();
        foreach (Point p in a.points)
        {
            result.Add(new Point(p));
        }
        foreach (Point p in b.points)
        {
            result.Add(new Point(p));
        }
        return result;
    }

    public static List<Cluster> HierarchicalClustering(List<Point> dataPoints)
    {
        
        List<Cluster> clusters = new List<Cluster>();
        foreach (Point p in dataPoints)
        {
            clusters.Add(new Cluster(p));
        }

        
        while (clusters.Count > 1)
        {
            
            int closestI = 0;
            int closestJ = 1;
            double minDistance = clusters[closestI].Distance(clusters[closestJ]);

            for (int i = 0; i < clusters.Count; i++)
            {
                for (int j = i + 1; j < clusters.Count; j++)
                {
                    double distance = clusters[i].Distance(clusters[j]);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestI = i;
                        closestJ = j;
                    }
                }
            }

           
            Cluster mergedCluster = clusters[closestI] + clusters[closestJ];
            clusters.RemoveAt(closestJ); 
            clusters.RemoveAt(closestI);
            clusters.Add(mergedCluster);
        }

        return clusters;
    }

    
    public List<Cluster> HierarchicalClustering()
    {
        return HierarchicalClustering(this.points);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.Unicode;

        Point p1 = new Point(1, 5);
        Point p2 = new Point(5, 3);
        Point p3 = new Point(8, 7);
        Point p4 = new Point(9, 8);
        Point p5 = new Point(4, 12);

        
        Console.WriteLine("các điểm đang có:");
        Console.WriteLine(p1);
        Console.WriteLine(p2);
        Console.WriteLine(p3);
        Console.WriteLine(p4);
        Console.WriteLine(p5);

        Console.WriteLine("\nkhoảng cách 2 điểm:");
        Console.WriteLine("Distance p1-p2: " + p1.Distance(p2));
        Console.WriteLine("Distance p2-p3: " + p2.Distance(p3));

        Cluster c1 = new Cluster();
        c1.Add(p1);
        c1.Add(p2);

        Cluster c2 = new Cluster();
        c2.Add(p3);
        c2.Add(p4);

        Cluster c3 = new Cluster();
        c3.Add(p5);

        Console.WriteLine("\ncác cluster:");
        Console.WriteLine("Cluster 1: " + c1);
        Console.WriteLine("Cluster 2: " + c2);
        Console.WriteLine("Cluster 3: " + c3);

        Console.WriteLine("\nkhoảng cách các cluster:");
        Console.WriteLine("Distance c1-c2: " + c1.Distance(c2));
        Console.WriteLine("Distance c2-c3: " + c2.Distance(c3));
        Console.WriteLine("Distance c1-c3: " + c1.Distance(c3));

        Cluster c4 = c1 + c2;
        Console.WriteLine("\nhợp cluster 1 và 2: " + c4);

        List<Point> allPoints = new List<Point>();
        allPoints.Add(p1);
        allPoints.Add(p2);
        allPoints.Add(p3);
        allPoints.Add(p4);
        allPoints.Add(p5);

        Console.WriteLine("\nHierarchical Clustering:");

        
        Cluster initialCluster = new Cluster(allPoints);
        Console.WriteLine("Ban đầu: " + initialCluster);

      
        List<Cluster> result = initialCluster.HierarchicalClustering();

        Console.WriteLine("kết quả thu được: ");
        foreach (Cluster c in result)
        {
            Console.WriteLine(c);
        }
    }
}