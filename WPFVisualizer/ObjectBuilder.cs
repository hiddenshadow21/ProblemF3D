using Cysterny;
using HelixToolkit.Wpf;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WPFVisualizer
{
    public static class ObjectBuilder
    {
        public static ModelVisual3D DodajObiektyZadania(Zadanie z)
        {
            Model3DGroup model3DGroup = new Model3DGroup();
            double przesuniecie = 0;
            foreach (var obj in z.cysterny)
            {
                try
                {
                    var x = Buduj(obj, z.PoziomWody, ref przesuniecie);
                    if (x != null) model3DGroup.Children.Add(x);
                }
                catch (Exception e)
                {
                    Thread t = new Thread(delegate ()
                    {
                        MessageBox.Show(e.Message);
                    });
                    t.Start();
                }
            }
            ModelVisual3D model = new ModelVisual3D();
            model.Content = model3DGroup;
            return model;
        }

        public static Model3D Buduj(Cysterna obj, double poziomWody, ref double przesuniecieX)
        {
            ModelImporter mi = new ModelImporter();
            Model3DGroup obiekty = new Model3DGroup();
            Model3D wireframe;
            MeshGeometry3D water = new MeshGeometry3D();
            switch (obj)
            {
                case Prostopadloscian _:
                    {
                        var o = obj as Prostopadloscian;
                        wireframe = mi.Load("..\\..\\Models\\cubeWireframe.obj");
                        Transform3DGroup tg = new Transform3DGroup();
                        ScaleTransform3D st = new ScaleTransform3D();
                        st.ScaleX = o.Dlugosc;
                        st.ScaleY = o.Szerokosc;
                        st.ScaleZ = o.Wysokosc;
                        TranslateTransform3D trt = new TranslateTransform3D();
                        trt.OffsetX = przesuniecieX;
                        trt.OffsetY = 0;
                        trt.OffsetZ = o.PoziomPodstawy + o.Wysokosc / 2.0;
                        tg.Children.Add(st);
                        tg.Children.Add(trt);
                        wireframe.Transform = tg;
                        obiekty.Children.Add(wireframe);

                        double p = poziomWody - o.PoziomPodstawy;
                        Model3D water2 = mi.Load("..\\..\\Models\\cubeWater.obj");
                        if (poziomWody > 0)
                        {
                            tg = new Transform3DGroup();
                            st = new ScaleTransform3D();
                            st.ScaleX = o.Dlugosc;
                            st.ScaleY = o.Szerokosc;
                            if (p < o.Wysokosc)
                                st.ScaleZ = p;
                            else
                                st.ScaleZ = o.Wysokosc;
                            trt = new TranslateTransform3D();
                            trt.OffsetX = przesuniecieX;
                            trt.OffsetY = 0;
                            trt.OffsetZ = wireframe.Bounds.Z + st.ScaleZ * water2.Bounds.SizeZ / 2;
                            tg.Children.Add(st);
                            tg.Children.Add(trt);
                        }
                        water2.Transform = tg;
                        if (p > 0 || poziomWody == -1)
                        {
                            obiekty.Children.Add(water2);
                        }

                        przesuniecieX += o.Dlugosc * 2;
                        return obiekty;
                    }

                case Kula _:
                    {
                        var o = obj as Kula;
                        wireframe = mi.Load("..\\..\\Models\\sphereWireframe.obj");
                        Transform3DGroup tg = new Transform3DGroup();
                        ScaleTransform3D st = new ScaleTransform3D();
                        st.ScaleX = o.Promien;
                        st.ScaleY = o.Promien;
                        st.ScaleZ = o.Promien;
                        TranslateTransform3D trt = new TranslateTransform3D();
                        trt.OffsetX = przesuniecieX + o.Promien;
                        trt.OffsetY = 0;
                        trt.OffsetZ = o.PoziomPodstawy + o.Promien;
                        tg.Children.Add(st);
                        tg.Children.Add(trt);
                        wireframe.Transform = tg;
                        obiekty.Children.Add(wireframe);

                        double p = poziomWody - o.PoziomPodstawy;
                        double wspolczynnik;
                        if (p > 0)
                            wspolczynnik = p / (2 * o.Promien);
                        else
                            wspolczynnik = 1;

                        AddSphere(water, new Point3D(0, 0, 0), o.Promien, wspolczynnik, 16, 32);
                        var x = new Quaternion(new Vector3D(1, 0, 0), -90);
                        RotateTransform3D rotate = new RotateTransform3D(new QuaternionRotation3D(x));
                        GeometryModel3D model1 = new GeometryModel3D(water, SetMaterial(Colors.Blue));
                        tg = new Transform3DGroup();
                        tg.Children.Add(rotate);
                        trt = new TranslateTransform3D();
                        trt.OffsetX = przesuniecieX + o.Promien;
                        trt.OffsetY = 0;
                        trt.OffsetZ = o.PoziomPodstawy + o.Promien;
                        tg.Children.Add(trt);
                        model1.Transform = tg;
                        if (p > 0 || poziomWody == -1)
                        {
                            obiekty.Children.Add(model1);
                        }

                        przesuniecieX += o.Promien * 2;
                        return obiekty;
                    }

                case Stozek _:
                    {
                        var o = obj as Stozek;
                        wireframe = mi.Load("..\\..\\Models\\coneWireframe.obj");
                        Transform3DGroup tg = new Transform3DGroup();
                        ScaleTransform3D st = new ScaleTransform3D();
                        st.ScaleX = o.Promien;
                        st.ScaleY = o.Promien;
                        st.ScaleZ = o.Wysokosc;
                        TranslateTransform3D trt = new TranslateTransform3D();
                        trt.OffsetX = przesuniecieX + o.Promien;
                        trt.OffsetY = 0;
                        trt.OffsetZ = o.PoziomPodstawy + o.Wysokosc / 2.0;
                        tg.Children.Add(st);
                        tg.Children.Add(trt);
                        wireframe.Transform = tg;
                        obiekty.Children.Add(wireframe);
                        przesuniecieX += o.Promien * 2 + 1;

                        double r;
                        double p = poziomWody - o.PoziomPodstawy;
                        if (p > 0 && p < o.Wysokosc)
                        {
                            r = o.Promien * (o.Wysokosc - p) / o.Wysokosc;
                            AddCone(water, new Point3D(trt.OffsetX, trt.OffsetY, wireframe.Bounds.Z), new Vector3D(0, 0, p), o.Promien, r, 32);
                        }
                        else
                            AddCone(water, new Point3D(trt.OffsetX, trt.OffsetY, wireframe.Bounds.Z), new Vector3D(0, 0, o.Wysokosc), o.Promien, 0, 32);


                        GeometryModel3D model1 = new GeometryModel3D(water, SetMaterial(Colors.Blue));


                        if (p > 0 || poziomWody == -1)
                        {
                            obiekty.Children.Add(model1);
                        }
                        return obiekty;
                    }

                case Cylinder _:
                    {
                        var o = obj as Cylinder;
                        wireframe = mi.Load("..\\..\\Models\\cylinderWireframe.obj");
                        Transform3DGroup tg = new Transform3DGroup();
                        ScaleTransform3D st = new ScaleTransform3D();
                        st.ScaleX = o.Promien;
                        st.ScaleY = o.Promien;
                        st.ScaleZ = o.Wysokosc;
                        TranslateTransform3D trt = new TranslateTransform3D();
                        trt.OffsetX = przesuniecieX + o.Promien;
                        trt.OffsetY = 0;
                        trt.OffsetZ = o.PoziomPodstawy + o.Wysokosc / 2.0;
                        tg.Children.Add(st);
                        tg.Children.Add(trt);
                        wireframe.Transform = tg;
                        obiekty.Children.Add(wireframe);
                        przesuniecieX += trt.OffsetX + o.Promien;

                        double p = poziomWody - o.PoziomPodstawy;
                        if (p > 0 && p < o.Wysokosc)
                        {
                            AddCylinder(water, new Point3D(trt.OffsetX, trt.OffsetY, wireframe.Bounds.Z), new Vector3D(0, 0, p), o.Promien, 32);
                        }
                        else
                            AddCylinder(water, new Point3D(trt.OffsetX, trt.OffsetY, wireframe.Bounds.Z), new Vector3D(0, 0, o.Wysokosc), o.Promien, 32);

                        GeometryModel3D model1 = new GeometryModel3D(water, SetMaterial(Colors.Blue));


                        if (p > 0 || poziomWody == -1)
                        {
                            obiekty.Children.Add(model1);
                        }
                        return obiekty;
                    }
                default:
                    throw new Exception("Unknown type");
            }
        }

        public static Material SetMaterial(Color color)
        {
            SolidColorBrush brush = new SolidColorBrush(color);
            DiffuseMaterial material = new DiffuseMaterial(brush);
            return material;
        }

        public static void AddTriangle(MeshGeometry3D mesh, Point3D point1, Point3D point2, Point3D point3)
        {
            // Create the points.
            int index1 = mesh.Positions.Count;
            mesh.Positions.Add(point1);
            mesh.Positions.Add(point2);
            mesh.Positions.Add(point3);

            // Create the triangle.
            mesh.TriangleIndices.Add(index1++);
            mesh.TriangleIndices.Add(index1++);
            mesh.TriangleIndices.Add(index1);
        }

        public static void AddCylinder(MeshGeometry3D mesh, Point3D end_point, Vector3D axis, double radius, int num_sides)
        {
            // Get two vectors perpendicular to the axis.
            Vector3D v1;
            if ((axis.Z < -0.01) || (axis.Z > 0.01))
                v1 = new Vector3D(axis.Z, axis.Z, -axis.X - axis.Y);
            else
                v1 = new Vector3D(-axis.Y - axis.Z, axis.X, axis.X);
            Vector3D v2 = Vector3D.CrossProduct(v1, axis);

            // Make the vectors have length radius.
            v1 *= (radius / v1.Length);
            v2 *= (radius / v2.Length);

            // Make the top end cap.
            double theta = 0;
            double dtheta = 2 * Math.PI / num_sides;
            for (int i = 0; i < num_sides; i++)
            {
                Point3D p1 = end_point +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2;
                theta += dtheta;
                Point3D p2 = end_point +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2;
                AddTriangle(mesh, end_point, p1, p2);
            }

            // Make the bottom end cap.
            Point3D end_point2 = end_point + axis;
            theta = 0;
            for (int i = 0; i < num_sides; i++)
            {
                Point3D p1 = end_point2 +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2;
                theta += dtheta;
                Point3D p2 = end_point2 +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2;
                AddTriangle(mesh, end_point2, p2, p1);
            }

            // Make the sides.
            theta = 0;
            for (int i = 0; i < num_sides; i++)
            {
                Point3D p1 = end_point +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2;
                theta += dtheta;
                Point3D p2 = end_point +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2;

                Point3D p3 = p1 + axis;
                Point3D p4 = p2 + axis;

                AddTriangle(mesh, p1, p3, p2);
                AddTriangle(mesh, p2, p3, p4);
            }
        }

        public static void AddSphere(MeshGeometry3D mesh, Point3D center, double radius, double wspolczynnikWypelnienia, int num_phi, int num_theta)
        {
            double phi0, theta0;
            double dphi = Math.PI * wspolczynnikWypelnienia / num_phi;
            double dtheta = 2 * Math.PI / num_theta;

            phi0 = 0;
            double y0 = radius * Math.Cos(phi0);
            double r0 = radius * Math.Sin(phi0);
            for (int i = 0; i < num_phi; i++)
            {
                double phi1 = phi0 + dphi;
                double y1 = radius * Math.Cos(phi1);
                double r1 = radius * Math.Sin(phi1);

                // Point ptAB has phi value A and theta value B.
                // For example, pt01 has phi = phi0 and theta = theta1.
                // Find the points with theta = theta0.
                theta0 = 0;
                Point3D pt00 = new Point3D(
                    center.X + r0 * Math.Cos(theta0),
                    center.Y + y0,
                    center.Z + r0 * Math.Sin(theta0));
                Point3D pt10 = new Point3D(
                    center.X + r1 * Math.Cos(theta0),
                    center.Y + y1,
                    center.Z + r1 * Math.Sin(theta0));
                for (int j = 0; j < num_theta; j++)
                {
                    // Find the points with theta = theta1.
                    double theta1 = theta0 + dtheta;
                    Point3D pt01 = new Point3D(
                        center.X + r0 * Math.Cos(theta1),
                        center.Y + y0,
                        center.Z + r0 * Math.Sin(theta1));
                    Point3D pt11 = new Point3D(
                        center.X + r1 * Math.Cos(theta1),
                        center.Y + y1,
                        center.Z + r1 * Math.Sin(theta1));

                    // Create the triangles.
                    AddTriangle(mesh, pt00, pt11, pt10);
                    AddTriangle(mesh, pt00, pt01, pt11);

                    // Move to the next value of theta.
                    theta0 = theta1;
                    pt00 = pt01;
                    pt10 = pt11;
                }

                // Move to the next value of phi.
                phi0 = phi1;
                y0 = y1;
                r0 = r1;
            }
            if (wspolczynnikWypelnienia < 1)
            {
                double r = Math.Sin(Math.PI * wspolczynnikWypelnienia) * radius;
                double h = Math.Cos(Math.PI * wspolczynnikWypelnienia) * radius;
                AddCone(mesh, new Point3D(0, h, 0), new Vector3D(0, 0.1, 0), r, 0, 32);
            }
        }

        public static void AddCone(MeshGeometry3D mesh, Point3D end_point, Vector3D axis, double radius1, double radius2, int num_sides)
        {
            // Get two vectors perpendicular to the axis.
            Vector3D top_v1;
            if ((axis.Z < -0.01) || (axis.Z > 0.01))
                top_v1 = new Vector3D(axis.Z, axis.Z, -axis.X - axis.Y);
            else
                top_v1 = new Vector3D(-axis.Y - axis.Z, axis.X, axis.X);
            Vector3D top_v2 = Vector3D.CrossProduct(top_v1, axis);

            Vector3D bot_v1 = top_v1;
            Vector3D bot_v2 = top_v2;

            // Make the vectors have length radius.
            top_v1 *= (radius1 / top_v1.Length);
            top_v2 *= (radius1 / top_v2.Length);

            bot_v1 *= (radius2 / bot_v1.Length);
            bot_v2 *= (radius2 / bot_v2.Length);

            // Make the top end cap.
            double theta = 0;
            double dtheta = 2 * Math.PI / num_sides;
            for (int i = 0; i < num_sides; i++)
            {
                Point3D p1 = end_point +
                    Math.Cos(theta) * top_v1 +
                    Math.Sin(theta) * top_v2;
                theta += dtheta;
                Point3D p2 = end_point +
                    Math.Cos(theta) * top_v1 +
                    Math.Sin(theta) * top_v2;
                AddTriangle(mesh, end_point, p1, p2);
            }

            // Make the bottom end cap.
            Point3D end_point2 = end_point + axis;
            theta = 0;
            for (int i = 0; i < num_sides; i++)
            {
                Point3D p1 = end_point2 +
                    Math.Cos(theta) * bot_v1 +
                    Math.Sin(theta) * bot_v2;
                theta += dtheta;
                Point3D p2 = end_point2 +
                    Math.Cos(theta) * bot_v1 +
                    Math.Sin(theta) * bot_v2;
                AddTriangle(mesh, end_point2, p2, p1);
            }

            // Make the sides.
            theta = 0;
            for (int i = 0; i < num_sides; i++)
            {
                Point3D p1 = end_point +
                    Math.Cos(theta) * top_v1 +
                    Math.Sin(theta) * top_v2;
                Point3D p3 = end_point + axis +
                    Math.Cos(theta) * bot_v1 +
                    Math.Sin(theta) * bot_v2;
                theta += dtheta;
                Point3D p2 = end_point +
                    Math.Cos(theta) * top_v1 +
                    Math.Sin(theta) * top_v2;
                Point3D p4 = end_point + axis +
                    Math.Cos(theta) * bot_v1 +
                    Math.Sin(theta) * bot_v2;

                AddTriangle(mesh, p1, p3, p2);
                AddTriangle(mesh, p2, p3, p4);
            }
        }

    }
}
